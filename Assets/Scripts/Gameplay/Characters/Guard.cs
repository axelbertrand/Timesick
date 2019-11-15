
namespace uqac.timesick.gameplay
{
    using Sirenix.OdinInspector;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Guard : Character
    {
        private DetectableSensor sightSensor = null;
        private NoiseDetector hearingSensor = null;

        protected override void Awake()
        {
            base.Awake();

            sightSensor = GetComponentInChildren<DetectableSensor>();
            sightSensor.Eye = transform;

            hearingSensor = GetComponentInChildren<NoiseDetector>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            List<MainCharacter> mcs = sightSensor.GetSightedFromType<MainCharacter>();
            if (mcs.Count > 0)
            {
                RotateToward(mcs[0].Position);
            }
            List<Vector4> noises = hearingSensor.GetHeardNoises();
            if(noises.Count > 0)
            {
                GetComponent<SpriteRenderer>().color = Color.red;
            }
        }

        private void OnDrawGizmos()
        {
            if (sightSensor != null)
            {
                foreach (IDetectable detectable in sightSensor.Sighted)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(transform.position, ((MonoBehaviour)detectable).transform.position);
                }

            }
        }
    }
}
