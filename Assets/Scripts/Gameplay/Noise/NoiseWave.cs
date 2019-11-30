
namespace uqac.timesick.gameplay
{
    using Cawotte.Toolbox.Audio;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;


    public class NoiseWave : MonoBehaviour, Noise
    {

        [SerializeField]
        private float explosionRange = 4f;
        [SerializeField]
        private float explosionSpeed = 1f;


        private float explosionTimer = 0f;
        private float timeSinceExplosion = 0f;

        private SpriteRenderer sr;
        // Start is called before the first frame update
        void Start()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            Exploding();
        }



        private void Exploding()
        {
            timeSinceExplosion += Time.deltaTime;
            float sizeExplosion = explosionSpeed * timeSinceExplosion;
            transform.localScale = new Vector3(sizeExplosion, sizeExplosion, 1);
            if (sizeExplosion > explosionRange)
            {
                Destroy(gameObject);
            }
        }

        private void Explode()
        {
            timeSinceExplosion = explosionTimer;
            AudioManager.Instance.PlaySound("Grenade");
        }
    }
}
