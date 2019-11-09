
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

        private Rigidbody2D rb;

        //Trigger on position change, (PreviousPos, NewPos)
        protected Action<Vector2, Vector2> OnPositionChange = null;

        //region Properties

        public Vector2 Position
        {
            get => transform.position;
            set
            {
                OnPositionChange?.Invoke(rb.position, value);
                rb.MovePosition(value);
            }
        }

        //endregion


        //region MonoBehaviour Loop

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

            OnPositionChange += TryRotateTowardNewPosition; //lambda function

        }

        //endregion


        protected void MoveToward(Vector2 worldPos)
        {

            Vector2 direction = (worldPos - rb.position).normalized;

            Vector2 deltaMovement = speed * Time.deltaTime * direction;

            //rb.velocity = deltaMovement;
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

        public void OnSight()
        {
            throw new NotImplementedException();
        }

        public void OnLoseOfSight()
        {
            throw new NotImplementedException();
        }
    }
}
