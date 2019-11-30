
namespace uqac.timesick.gameplay
{
    using Sirenix.OdinInspector;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Cawotte.Toolbox.Audio;

    [Serializable]
    public class Character : SerializedMonoBehaviour
    {
        protected AudioSourcePlayer player;

        [SerializeField]
        protected float walkingSpeed = 5f;
        [SerializeField]
        protected float sprintingSpeed = 9f;

        [SerializeField]
        private float rotateSpeed = 10f;

        [ShowInInspector, ReadOnly]
        protected float currentAngle = 0f;

        [BoxGroup("Health bar"), SerializeField]
        [ProgressBar(0, "maxHealth", ColorMember = "GetHealthBarColor", Segmented = true)]
        protected int currentHealth = 10;

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
        public Action<Vector2, Vector2> OnPositionChange = null;

            // PreviousHealth, NewHealth
        protected Action<int, int> OnHealthChange = null;
        protected Action OnDeath = null;

        [SerializeField]
        public Animator animator;

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

            player = gameObject.AddComponent<AudioSourcePlayer>();

            currentHealth = maxHealth;
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
            RotateToward(Position + (Position - lastPosition).normalized );
        }

        public void RotateToward(Vector2 worldPos, bool transformRotation=false, bool instantRotation=false)
        {

            Vector2 remainingDistance = (worldPos - (Vector2)transform.position);
            Vector2 direction = remainingDistance.normalized;

            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            //max rotation per frame
            float deltaAngle = angle - currentAngle;

            if (Mathf.Abs(deltaAngle) > 180)
            {
                deltaAngle += (deltaAngle > 180) ? -360 : 360;
            }
                

            //Apply a max rotation, but makes sure the multiplier doesn't make it higher than the original one
            if (!instantRotation && Mathf.Abs(deltaAngle * Time.deltaTime * rotateSpeed) < Mathf.Abs(deltaAngle))
            {
                deltaAngle = deltaAngle * Time.deltaTime * rotateSpeed;
            }

            currentAngle = (currentAngle + deltaAngle +360) % 360;
            if(animator != null)
            {
                animator.SetFloat("Angle", currentAngle);
            }

            Quaternion q = Quaternion.AngleAxis(currentAngle, Vector3.forward);
            if(transformRotation)
            {
                transform.rotation = q;
            }
            
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
