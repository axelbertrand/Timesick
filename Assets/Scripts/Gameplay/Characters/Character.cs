﻿
namespace uqac.timesick.gameplay
{
    using Sirenix.OdinInspector;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class Character : SerializedMonoBehaviour, IDetectable
    {

        [ShowInInspector]
        protected float speed = 3f;

        [BoxGroup("Health bar"), SerializeField]
        [ProgressBar(0, "maxHealth", ColorMember = "GetHealthBarColor", Segmented = true)]
        protected int currentHealth;

        [BoxGroup("Health bar"), SerializeField]
        protected int maxHealth = 10;

        [BoxGroup("Stamina bar"), SerializeField]
        [ProgressBar(0, "maxStamina", ColorMember = "GetStaminaBarColor", Segmented = true)]
        protected int currentStamina;

        [BoxGroup("Stamina bar"), SerializeField]
        protected int maxStamina = 10;


        private Rigidbody2D rb;

            //Trigger on position change, (PreviousPos, NewPos)
        protected Action<Vector2, Vector2> OnPositionChange = null;

            // PreviousHealth, NewHealth
        protected Action<int, int> OnHealthChange = null;
        protected Action OnDeath = null;

        #region Properties

        public Vector2 Position
        {
            get => transform.position;
            set
            {
                OnPositionChange?.Invoke(rb.position, value);
                rb.MovePosition(value);
            }
        }

        public bool IsInvisible
        {
            get;
            set;
        }

        public int CurrentHealth
        {
            get => currentHealth;
            protected set
            {
                //clamp the value within the correct values
                value = Mathf.Clamp(value, 0, maxHealth);

                if (currentHealth == value)
                    return;

                OnHealthChange?.Invoke(currentHealth, value);
                currentHealth = value;
            }
        }

        #endregion


        #region MonoBehaviour Loop

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

            OnPositionChange += (oldP, newP) => RotateToward(newP); //lambda function

        }

        #endregion

        public virtual void DealDamage(int damage)
        {
            currentHealth -= damage;

            if (currentHealth == 0)
            {
                OnDeath?.Invoke();
            }
        }

        #region Movements

        protected void MoveToward(Vector2 worldPos)
        {

            Vector2 direction = (worldPos - rb.position).normalized;

            Vector2 deltaMovement = speed * Time.deltaTime * direction;

            Position += deltaMovement;
        }

        protected void TryRotateTowardNewPosition(Vector2 oldP, Vector2 newP)
        {
            Vector2 dir = newP - oldP;
            RotateToward(newP);
        }

        protected void RotateToward(Vector2 worldPos)
        {

            Vector2 remainingDistance = (worldPos - (Vector2)transform.position);
            Vector2 direction = remainingDistance.normalized;

            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = q;

            //transform.rotation = Quaternion.Slerp(transform.rotation, q, RotateSpeed * Time.deltaTime);
        }

        #endregion


        //Get the color of the health bar in the Inspector's UI. (ODIN)
        private Color GetHealthBarColor(int value)
        {
            return Color.Lerp(Color.yellow, Color.green, (float)value / maxHealth);
        }

        //Get the color of the stamina bar in the Inspector's UI. (ODIN)
        private Color GetStaminaBarColor(int value)
        {
            return Color.Lerp(Color.yellow, Color.green, (float)value / maxStamina);
        }
    }
}
