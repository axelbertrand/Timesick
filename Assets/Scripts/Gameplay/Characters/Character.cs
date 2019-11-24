
namespace uqac.timesick.gameplay
{
    using Sirenix.OdinInspector;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Cawotte.Toolbox.Audio;

    [Serializable]
    public class Character : SerializedMonoBehaviour, IDetectable
    {
        protected AudioSourcePlayer player;

        [ShowInInspector]
        protected float walkingSpeed = 5f;
        [ShowInInspector]
        protected float sprintingSpeed = 9f;

        [SerializeField]
        private float rotateSpeed = 10f;
        private float currentAngle = 0f;

        [BoxGroup("Health bar"), SerializeField]
        [ProgressBar(0, "maxHealth", ColorMember = "GetHealthBarColor", Segmented = true)]
        protected int currentHealth;

        [BoxGroup("Health bar"), SerializeField]
        protected int maxHealth = 10;


        [BoxGroup("Health bar"), SerializeField]
        protected float invincibilityFrame = 1f;

        [ShowInInspector, ReadOnly]
        protected bool isInvincible = false;

        [ShowInInspector, ReadOnly]
        protected bool isMoving = false;

        private Rigidbody2D rb;
        private Vector2 lastPosition = Vector2.zero;

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
                lastPosition = Position;
                OnPositionChange?.Invoke(rb.position, value);
                rb.MovePosition(value);
            }
        }

        public bool IsInvisible
        {
            get;
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

        public bool IsMoving { get => isMoving; }

        #endregion


        #region MonoBehaviour Loop

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

            OnPositionChange += (oldP, newP) => RotateToward(newP); //lambda function
            player = gameObject.AddComponent<AudioSourcePlayer>();
        }


        #endregion

        public virtual void DealDamage(int damage)
        {
            if (isInvincible)
            {
                return;
            }

            CurrentHealth -= damage;

            if (currentHealth == 0)
            {
                OnDeath?.Invoke();
            }
            else
            {
                StartCoroutine(_InvincibilityFrames(invincibilityFrame));
            }
        }

        #region Movements

        protected void MoveToward(Vector2 worldPos, bool sprinting)
        {

            Vector2 direction = (worldPos - rb.position).normalized;

            Vector2 deltaMovement;
            if (sprinting)
            {
                deltaMovement = sprintingSpeed * Time.deltaTime * direction;
            }
            else
            {
                deltaMovement = walkingSpeed * Time.deltaTime * direction;
            }
            Position += deltaMovement;
        }

        public void RotateInStepDirection()
        {
            RotateToward(lastPosition - Position);
        }
        public void RotateToward(Vector2 worldPos)
        {

            Vector2 remainingDistance = (worldPos - (Vector2)transform.position);
            Vector2 direction = remainingDistance.normalized;

            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

            //max rotation per frame
            float deltaAngle = angle - currentAngle;

            deltaAngle = deltaAngle * Time.deltaTime * rotateSpeed;
            currentAngle += deltaAngle;

            Quaternion q = Quaternion.AngleAxis(currentAngle, Vector3.forward);

            transform.rotation = q;

            //transform.rotation = Quaternion.Slerp(transform.rotation, q, (rotateSpeed * Time.deltaTime) / rotateSpeed);
        }

        #endregion

        private IEnumerator _InvincibilityFrames(float invincibilityLenght)
        {
            isInvincible = true;

            yield return new WaitForSeconds(invincibilityLenght);

            isInvincible = false;
        }

        //Get the color of the health bar in the Inspector's UI. (ODIN)
        private Color GetHealthBarColor(int value)
        {
            return Color.Lerp(Color.yellow, Color.green, (float)value / maxHealth);
        }
    }
}
