using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cawotte.Toolbox.Audio;

namespace uqac.timesick.gameplay
{

    [System.Serializable]
    public class NoiseTrap : MonoBehaviour
    {
        [SerializeField]
        private float timeBeforeExplosion;

        [SerializeField]
        private GameObject noiseWavePrefab;

        private float explosionTimer  = 0f;

        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

            WaitToExplode();


        }

        private void WaitToExplode()
        {
            explosionTimer += Time.deltaTime;
            if(explosionTimer > timeBeforeExplosion)
            {
                GameObject noise = Instantiate(noiseWavePrefab, transform.position, Quaternion.identity);
                noise.SetActive(true);

                Destroy(gameObject);
            }
        }



    }
}