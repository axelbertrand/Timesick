using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cawotte.Toolbox.Audio;

namespace uqac.timesick.gameplay
{
    public class MainCharacter : Character, IDetectable
    {
        [BoxGroup("Stamina bar"), SerializeField]
        [ProgressBar(0, "maxStamina", ColorMember = "GetStaminaBarColor", Segmented = true)]
        private int currentStamina;

        #region Variables
        #region Public
        [SerializeField]
        private GameObject popup = null;
        [SerializeField]
        private GameObject footsteps = null;
        [SerializeField]
        private GameObject noiseDevice = null;
        [SerializeField]
        public GameObject mole;
        #endregion
        #region Private
        //True if the mainCharacter has stolen the medicine successfully
        private bool hasMedicine = false;
        //Contains the list of all interactive in range
        private HashSet<GameObject> inRange;
        //Contains the current action selected
        private UserAction currentAction;
        //Contains the interactive GameObject currently selected 
        private GameObject selected = null;
        //True if the mainCharacter is doing a QTE
        private bool inQTE = false;
        
        //the name is self explanatory 
        private float timeSinceLastFootstep = 0f;
        //same
        [SerializeField]
        private float timeBetweenFootsteps = 0.5f;

        bool waitingTheMole = true;
        #endregion
        #endregion

        [BoxGroup("Stamina bar"), SerializeField]
        private int maxStamina = 10;

        
        /**
         * Delay after starting of stamina regeneration
         */
        [BoxGroup("Stamina bar"), SerializeField]
        private float staminaRegenerationDelay = 3f;

        /**
         * Interval between each stamina regeneration
         */
        [BoxGroup("Stamina bar"), SerializeField]
        private float staminaRegenerationInterval = 1f;

        [BoxGroup("Invisibility"), SerializeField]
        [ProgressBar(0, "maxStamina", ColorMember = "GetStaminaBarColor", Segmented = true)]
        private int invisibilityCost = 1;

        [BoxGroup("Invisibility"), SerializeField]
        private float invisibilityTime = 2f;

        [BoxGroup("Invisibility"), SerializeField, ReadOnly]
        private bool isInvisible = false;

        private SpriteRenderer spriteRenderer;


        private float staminaRegenerationDelayTimer = 0f;
        private float staminaRegenerationIntervalTimer = 0f;

        protected Action<int, int> OnStaminaChange = null;

        //endregion

        public int CurrentStamina
        {
            get => currentStamina;
            set
            {
                OnStaminaChange?.Invoke(currentStamina, value);
                currentStamina = value;
            }
        }

        public bool IsInvisible
        {
            get => isInvisible;
            set
            {
                if (isInvisible == value) return;
                isInvisible = value;

                float alpha = isInvisible ? 0.5f : 1f;
                Color color = spriteRenderer.color;
                color.a = alpha;
                spriteRenderer.color = color;
            }
        }


        protected override void Awake()
        {
            base.Awake();

            OnPositionChange += (oldP, newP) => RotateToward(newP); //rotate on movement

            spriteRenderer = GetComponent<SpriteRenderer>();

            OnHealthChange += UpdateDamageEffect;
            OnStaminaChange += UpdateStaminaBar;
        }

        // Update is called once per frame

        #region MonoBehaviour
        void Start()
        {
            MapManager.Instance.RegisterPlayer(this);

            selected = null;
            inRange = new HashSet<GameObject>();
            updatePopup(null);
            timeSinceLastFootstep = float.PositiveInfinity;
            UIManager.Instance.InitializeStaminaBar(maxStamina,currentStamina);
            OnDeath += Die;
        }


        void Update()
        {

            //Moove the mainCharacter if he presses the movement keys
            HandleMovements();


            //Execute the current action (if one available) if the mainCharacter press the actions's button
            HandleAction();

            //Update the current selection of the nearest Interactive
            UpdateSelection();

            HandleNoiseDevice();

            HandleSkills();
        }

        #endregion

        #region Movement
        private void HandleMovements()
        {
            if (currentAction == null || !inQTE)
            {
                //Read the direction of the current inputs
                Vector2 inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

                if (inputDirection.magnitude < Mathf.Epsilon)
                {
                    return;
                }
                
                /*
                if (IsInvisible)
                {
                    IsInvisible = false;
                    staminaRegenerationDelayTimer = 0f;
                }*/

                //They are normalized for constant speed in all directions.
                inputDirection = inputDirection.normalized;
                
                //Handle the change of speed if the mainCharacter is sprinting
                bool sprinting=false;
                if (InputManager.GetButton(Button.SPRINT))
                {
                    sprinting = true;
                    if(timeSinceLastFootstep < timeBetweenFootsteps)
                    {
                        timeSinceLastFootstep += Time.deltaTime;
                    }
                    else
                    {
                        Instantiate(footsteps, transform.position, Quaternion.identity);
                        timeSinceLastFootstep = 0f;
                    }
                }

                MoveToward(Position + inputDirection,sprinting);
            }
        }

        private void HandleFootsteps()
        {

        }
        #endregion

        #region Interations
        //Helpers
        public void HandleAction()
        {
            if (currentAction != null)
            {
                if (InputManager.GetButton(currentAction.button))
                {
                    currentAction.Do();
                    updatePopup(currentAction);
                    if (currentAction.IsDone())
                    {
                        currentAction = null;
                        inQTE = false;
                    }
                    else
                    {
                        inQTE = true;
                    }
                }
                else
                {
                    inQTE = false;
                    currentAction = null;
                }
            }
        }
        public void UpdateSelection()
        {
            if (currentAction == null)
            {
                if (inRange.Count > 0)
                {
                    // On cherche l'objet intéractif le plus proche
                    UserAction bestAction = null;
                    float distanceMin = float.PositiveInfinity;
                    GameObject nearest = null;

                    foreach (GameObject o in inRange)
                    {
                        UserAction action = o.GetComponent<Interactive>().GetAction(this);
                        float distance = (o.transform.position - transform.position).magnitude;
                        if (action != null && distance < distanceMin )
                        {
                            distanceMin = distance;
                            nearest = o;
                            bestAction = action;
                        }
                    }

                    // On désélectionne l'objet sélectionné auparavant
                    if (selected != null )
                    {
                        selected.GetComponent<Interactive>().Deselect();
                    }

                    selected = nearest;
                    // Si l'interactive le plus proche a une action disponible, alors on la selectionne
                    if ( nearest != null)
                    {
                        Interactive interactive = selected.GetComponent<Interactive>();
                        interactive.Select();
                    }

                    //L'action de l'object selectionner devient notre nouvelle action courante
                    currentAction = bestAction;

                    updatePopup(currentAction);
                }
                else
                {

                    updatePopup(null);
                    if (selected != null)
                    {
                        selected.GetComponent<Interactive>().Deselect();
                        selected = null;
                    }
                }
            }
        }

        //Display of the popup
        private void updatePopup(UserAction action)
        {
            if (action != null)
            {
                Text text = popup.GetComponentsInChildren<Text>()[0];
                Text button = popup.GetComponentsInChildren<Text>()[1];
                Text combos = popup.GetComponentsInChildren<Text>()[2];
                Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, transform.GetComponentInChildren<Renderer>().bounds.size.y));

                if (inQTE)
                {
                    if (action.combos == null)
                    {
                        text.text = "";
                        button.text = "";
                        combos.text = "";
                    }
                    else
                    {
                        text.text = "";
                        combos.text = "";
                        foreach (Button b in action.combos)
                        {
                            combos.text += InputManager.GetButtonName(b) + " ";
                        }

                        combos.text = combos.text.Remove(combos.text.Length - 1);
                        button.text = "";
                    }

                    Slider slider = popup.GetComponentInChildren<Slider>();
                    slider.transform.localScale = new Vector3(1, 1, 1);
                    slider.value = action.progression;
                    slider.gameObject.SetActive(true);
                }
                else
                {
                    text.text = action.name;
                    text.fontSize = 28;
                    button.text = InputManager.GetButtonName(action.button);
                    combos.text = "";
                    popup.GetComponentInChildren<Slider>().gameObject.transform.localScale = new Vector3(0, 0, 0);
                }

                popup.transform.position = screenPos;
                popup.gameObject.SetActive(true);
            }
            else
            {
                if ( popup != null && popup.activeSelf)
                {
                    popup.GetComponentInChildren<Slider>().gameObject.transform.localScale = new Vector3(0, 0, 0);
                    popup.gameObject.SetActive(false);
                }
            }
        }
        public void CollectMedicine(MedicineContainer medicineContainer)
        {

            if (medicineContainer.StealMedicine())
            {
                hasMedicine = true;
            }

            CallTheMoleForTheRescue();

        }
        #endregion

        #region Collisions
        //Collision with Interactives
        private void OnTriggerEnter2D(Collider2D other)
        {
            // On ajoute l'objet à portée dans la liste des objets potentiellement sélectionnable
            Interactive interactive = other.gameObject.GetComponent<Interactive>();
            if (interactive != null)
            {
                Debug.Log("Int");
                inRange.Add(other.gameObject);
                return;
            }

            if(other.name.Contains("Escape Route"))
            {
                GetComponent<SpriteRenderer>().color = Color.red;
                Debug.Log("ON SE BARRE");
                Destroy(this.gameObject);
                return;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            // On enlève l'objet qui était à portée
            Interactive interactive = collision.gameObject.GetComponent<Interactive>();
            if (interactive != null)
            {
                inRange.Remove(collision.gameObject);
            }
        }


        #endregion

        #region Escaping
        private void CallTheMoleForTheRescue()
        {
            waitingTheMole = true;
            Instantiate(mole, new Vector2(0, 0), Quaternion.identity);
        }
        #endregion

        #region Abilities
        private void HandleNoiseDevice()
        {
            if (!inQTE && InputManager.GetButtonDown(Button.NOISEDEVICE))
            {
                Instantiate(noiseDevice,transform.position, Quaternion.identity);
            }
        }
        #endregion
        private void HandleSkills()
        {
            // Invisibility
            if (InputManager.GetButtonDown(Button.INVISIBILITY))
            {
                // Return if invisibility skill can't be used
                if (IsInvisible || CurrentStamina < invisibilityCost)
                {
                    return;
                }

                IsInvisible = true;
                CurrentStamina -= invisibilityCost;
                staminaRegenerationDelayTimer = 0f;
                Debug.Log("Start of invisibility");
                //TODO
                AudioManager.Instance.PlaySound("Invisibility");
                Invoke("WaitAndSetVisible", invisibilityTime);
            }
            else if (CurrentStamina < maxStamina && !IsInvisible)
            {
                if (staminaRegenerationDelayTimer >= staminaRegenerationDelay)
                {
                    // Delay after stamina regeneration is over

                    if (staminaRegenerationIntervalTimer >= staminaRegenerationInterval)
                    {
                        CurrentStamina = Math.Min(CurrentStamina + 1, maxStamina);
                        staminaRegenerationIntervalTimer = 0f;
                    }
                    else
                    {
                        staminaRegenerationIntervalTimer += Time.deltaTime;
                    }
                }
                else
                {
                    staminaRegenerationDelayTimer += Time.deltaTime;
                }
            }
        }

        private void WaitAndSetVisible()
        {
            IsInvisible = false;
            staminaRegenerationDelayTimer = 0f;
        }

        //Get the color of the stamina bar in the Inspector's UI. (ODIN)
        private Color GetStaminaBarColor(int value)
        {
            return Color.Lerp(Color.yellow, Color.green, (float)value / maxStamina);
        }

        private void UpdateDamageEffect(int oldValue,int newValue)
        {
            float healthPercentage = (float)newValue / maxHealth;
            CameraEffectsManager.Instance.UpdateDamageEffect(healthPercentage);
        }

        private void UpdateStaminaBar(int oldValue,int newValue)
        {
            UIManager.Instance.UpdateBar(newValue);
        }

        private void Die()
        {
            GameManager.Instance.OnDeath();
        }
    }

}
