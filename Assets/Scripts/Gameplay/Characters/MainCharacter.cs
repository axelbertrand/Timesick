using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace uqac.timesick.gameplay
{
    public class MainCharacter : Character
    {
        #region Variables
        #region Public
        [SerializeField]
        private GameObject popup = null;
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


        #endregion
        #endregion




        #region MonoBehaviour
        void Start()
        {
            selected = null;
            inRange = new HashSet<GameObject>();
            updatePopup(null);

        }


        void Update()
        {

            //Moove the mainCharacter if he presses the movement keys
            if(currentAction == null || !inQTE)
            {
                HandleMovements();
            }

            //Execute the current action (if one available) if the mainCharacter press the actions's button
            HandleAction();

            //Update the current selection of the nearest Interactive
            UpdateSelection();




        }

        #endregion

        #region Movement
        private void HandleMovements()
        {

            //Read the direction of the current inputs
            Vector2 inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (inputDirection.magnitude < Mathf.Epsilon)
            {
                return;
            }

            //They are normalized for constant speed in all directions.
            inputDirection = inputDirection.normalized;

            MoveToward(Position + inputDirection);

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
                    text.fontSize = 12;
                    button.text = InputManager.GetButtonName(action.button);
                    combos.text = "";
                    popup.GetComponentInChildren<Slider>().gameObject.transform.localScale = new Vector3(0, 0, 0);
                }

                popup.transform.position = screenPos;
                popup.gameObject.SetActive(true);
            }
            else
            {
                if (popup.activeSelf)
                {
                    popup.GetComponentInChildren<Slider>().gameObject.transform.localScale = new Vector3(0, 0, 0);
                    popup.gameObject.SetActive(false);
                }
            }
        }


        //Collision with Interactives
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // On ajoute l'objet à portée dans la liste des objets potentiellement sélectionnable
            Interactive interactive = collision.gameObject.GetComponent<Interactive>();
            if (interactive != null)
            {
                inRange.Add(collision.gameObject);
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

        //Abstraction functions
        public void CollectMedicine(MedicineContainer medicineContainer)
        {

            if (medicineContainer.StealMedicine())
            {
                hasMedicine = true;
            }

            //TODO, Start the escape process
        }
        #endregion
    }

}
