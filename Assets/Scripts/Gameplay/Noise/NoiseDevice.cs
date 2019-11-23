﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cawotte.Toolbox.Audio;

namespace uqac.timesick.gameplay
{

    public class NoiseDevice : MonoBehaviour, Noise
    {
        public float timeBeforeExplosion;
        public float explosionRange;
        public float explosionSpeed;
        public Sprite explosionSprite;
        public Sprite idleSprite;

        private float explosionTimer=0;
        private float timeSinceExplosion=0;
        private bool isExploding=false;
        void Start()
        {
            GetComponent<SpriteRenderer>().sprite = idleSprite;
            GetComponent<CircleCollider2D>().enabled = false;

        }

        // Update is called once per frame
        void Update()
        {
            
            if(isExploding)
            {
                Exploding();
            }
            else
            {
                WaitToExplode();
            }


        }

        private void WaitToExplode()
        {
            explosionTimer += Time.deltaTime;
            if(explosionTimer > timeBeforeExplosion)
            {
                Explode();
            }
        }

        private void Exploding()
        {
            timeSinceExplosion += Time.deltaTime;
            float sizeExplosion = explosionSpeed * timeSinceExplosion;
            transform.localScale = new Vector3(sizeExplosion, sizeExplosion, 1);
            if(sizeExplosion > explosionRange)
            {
                Exploded();
            }
        }

        private void Explode()
        {
            isExploding = true;
            timeSinceExplosion = explosionTimer - timeBeforeExplosion;
            GetComponent<SpriteRenderer>().sprite = explosionSprite;
            GetComponent<CircleCollider2D>().enabled = true;
            //TODO
            AudioManager.Instance.PlaySound("Grenade");
        }
        private void Exploded()
        {
            Destroy(gameObject, 0.0f);
        }


    }
}