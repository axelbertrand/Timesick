using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace uqac.timesick.gameplay
{
    public class MainCharacter : Character
    {
        [BoxGroup("Stamina bar"), SerializeField]
        [ProgressBar(0, "maxStamina", ColorMember = "GetStaminaBarColor", Segmented = true)]
        private int currentStamina;

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

        private SpriteRenderer spriteRenderer;
        private bool isInvisible;
        private float staminaRegenerationDelayTimer = 0f;
        private float staminaRegenerationIntervalTimer = 0f;

        //endregion
        public new bool IsInvisible
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

        //region MonoBehaviour Loop

        protected override void Awake()
        {
            base.Awake();

            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            HandleMovements();
            HandleSkills();
        }

        //endregion

        private void HandleMovements()
        {

            //Read the direction of the current inputs
            Vector2 inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (inputDirection.magnitude < Mathf.Epsilon)
            {
                return;
            }

            if (IsInvisible)
            {
                IsInvisible = false;
                staminaRegenerationDelayTimer = 0f;
            }

            //They are normalized for constant speed in all directions.
            inputDirection = inputDirection.normalized;

            MoveToward(Position + inputDirection);

        }

        private void HandleSkills()
        {
            // Invisibility
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Return if invisibility skill can't be used
                if (IsInvisible || currentStamina < invisibilityCost)
                {
                    return;
                }

                IsInvisible = true;
                currentStamina -= invisibilityCost;
                staminaRegenerationDelayTimer = 0f;
                Debug.Log("Start of invisibility");

                Invoke("WaitAndSetVisible", invisibilityTime);
            }
            else if (currentStamina < maxStamina && !IsInvisible)
            {
                if (staminaRegenerationDelayTimer >= staminaRegenerationDelay)
                {
                    // Delay after stamina regeneration is over

                    if (staminaRegenerationIntervalTimer >= staminaRegenerationInterval)
                    {
                        currentStamina = Math.Min(currentStamina + 1, maxStamina);
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
            Debug.Log("End of invisibility");
        }

        //Get the color of the stamina bar in the Inspector's UI. (ODIN)
        private Color GetStaminaBarColor(int value)
        {
            return Color.Lerp(Color.yellow, Color.green, (float)value / maxStamina);
        }
    }
}
