using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace uqac.timesick.gameplay
{
    public class NoiseDetector : MonoBehaviour
    {

        private List<Vector4> noises;

        [HideInInspector]
        public Action<Vector2> OnNoiseHeard;

        void Start()
        {
            noises = new List<Vector4>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log("Collision ! ");
            // On ajoute l'objet à portée dans la liste des objets potentiellement sélectionnable
            Noise noise = collision.gameObject.GetComponent<Noise>();
            if (noise!= null)
            {
                Vector3 noisePosition = collision.gameObject.transform.position;
                noises.Add(new Vector4(noisePosition.x,noisePosition.y, noisePosition.z,Time.time));

                OnNoiseHeard?.Invoke(new Vector2(noisePosition.x, noisePosition.y)); //Trigger events on noise heard
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {

        }

        public List<Vector4> GetHeardNoises()
        {
            List<Vector4> l = noises;
            noises = new List<Vector4>();
            return l;
        }
    }
}
