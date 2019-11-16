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

        [BoxGroup("Stamina bar"), SerializeField]
        private int delayAfterStaminaRegeneration = 3;

        [BoxGroup("Stamina bar"), SerializeField]
        private int staminaRegenerationInterval = 1;

        [BoxGroup("Invisibility"), SerializeField]
        [ProgressBar(0, "maxStamina", ColorMember = "GetStaminaBarColor", Segmented = true)]
        private int invisibilityCost = 1;

        [BoxGroup("Invisibility"), SerializeField]
        private float invisibilityTime = 2f;

        private SpriteRenderer spriteRenderer;
        private bool isInvisible;
        private float staminaRegenerationTimer = 0f;

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
                staminaRegenerationTimer = 0f;
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
                Debug.Log("Start of invisibility");

                Invoke("WaitAndSetVisible", invisibilityTime);
            }
            else if (currentStamina < maxStamina)
            {
                if (staminaRegenerationTimer >= delayAfterStaminaRegeneration)
                {
                    currentStamina = Math.Min(currentStamina + 1, maxStamina);
                }
                else
                {
                    staminaRegenerationTimer += Time.deltaTime;
                }
            }
        }

        private void WaitAndSetVisible()
        {
            IsInvisible = false;
            staminaRegenerationTimer = 0f;
            Debug.Log("End of invisibility");
        }

        //Get the color of the stamina bar in the Inspector's UI. (ODIN)
        private Color GetStaminaBarColor(int value)
        {
            return Color.Lerp(Color.yellow, Color.green, (float)value / maxStamina);
        }
    }
}
