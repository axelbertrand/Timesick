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
        private int delayAfterStaminaRestauration = 3;

        [BoxGroup("Stamina bar"), SerializeField]
        private int staminaRestaurationTime = 1;

        [BoxGroup("Invisibility"), SerializeField]
        [ProgressBar(0, "maxStamina", ColorMember = "GetStaminaBarColor", Segmented = true)]
        private int invisibilityCost = 1;

        [BoxGroup("Invisibility"), SerializeField]
        private float invisibilityTime = 2f;

        SpriteRenderer spriteRenderer;
        private bool isInvisible;

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
            }

            //They are normalized for constant speed in all directions.
            inputDirection = inputDirection.normalized;

            MoveToward(Position + inputDirection);

        }

        private void HandleSkills()
        {
            // Invisibility
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (IsInvisible || currentStamina < invisibilityCost)
                {
                    return;
                }

                IsInvisible = true;
                currentStamina -= invisibilityCost;
                Invoke("RestaureStamina", delayAfterStaminaRestauration);
                Debug.Log("Start of invisibility");

                Invoke("WaitAndSetVisible", invisibilityTime);
            }
        }

        private void WaitAndSetVisible()
        {
            IsInvisible = false;
            Debug.Log("End of invisibility");
        }

        private void RestaureStamina()
        {
            currentStamina = Math.Min(currentStamina + 1, maxStamina);
            if (currentStamina < maxStamina)
            {
                Invoke("RestaureStamina", staminaRestaurationTime);
            }
        }

        //Get the color of the stamina bar in the Inspector's UI. (ODIN)
        private Color GetStaminaBarColor(int value)
        {
            return Color.Lerp(Color.yellow, Color.green, (float)value / maxStamina);
        }
    }
}
