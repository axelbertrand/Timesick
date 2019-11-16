using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace uqac.timesick.gameplay
{
    public class Footstep : MonoBehaviour, Noise
    {

        public float noiseRange;
        public float noiseSpeed;
        public Sprite noiseSprite;

        private float timer = 0;
        private CircleCollider2D collider;
        void Start()
        {
            collider = GetComponent<CircleCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;
            float sizeNoise = noiseSpeed * timer;
            transform.localScale = new Vector3(sizeNoise, sizeNoise, 1);

            if (sizeNoise > noiseRange)
            {
                Die();
            }
        }

        private void Die()
        {
            Destroy(gameObject, 0.0f);
        }
    }
}