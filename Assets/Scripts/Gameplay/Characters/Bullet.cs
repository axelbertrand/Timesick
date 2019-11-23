namespace uqac.timesick.gameplay
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Sirenix.OdinInspector;

    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float velocity = 1f;
        [SerializeField] private int damage = 1;
        [SerializeField] private float offsetAngle = 0f;
        [SerializeField] [ReadOnly] private float maxLifetime = 2f;


        private Rigidbody2D rb;
        private Vector2 direction = Vector2.zero;
        private float timerLifetime = 0f;

        public Vector2 Direction { get => direction; set => direction = value; }


        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

        }

        private void Start()
        {
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            MoveToward(direction);

            if (timerLifetime >= maxLifetime)
            {
                Destroy(gameObject);
                return;
            }

            timerLifetime += Time.fixedDeltaTime;
        }

        public void SetVelocityAndDamage(float velocity, float damage)
        {
            this.damage = (int)damage;
            this.velocity = velocity;
        }

        private void MoveToward(Vector2 direction)
        {
            Vector3 movement = direction * Time.fixedDeltaTime * velocity;

            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            angle += offsetAngle;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = q;

            rb.MovePosition(transform.position + movement);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            MainCharacter character = collision.GetComponent<MainCharacter>();

            if (character != null)
            {
                character.DealDamage(damage);
                Destroy(gameObject);
            }

            if (LayerMask.LayerToName(collision.gameObject.layer).Equals("Wall"))
            {
                Destroy(gameObject);
            }
        }

    }
}
