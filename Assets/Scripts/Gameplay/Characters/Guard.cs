﻿
namespace uqac.timesick.gameplay
{
    using Sirenix.OdinInspector;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Guard : Character
    {
        private DetectableSensor sensor = null;

        protected override void Awake()
        {
            base.Awake();

            sensor = GetComponentInChildren<DetectableSensor>();

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            List<MainCharacter> mcs = sensor.GetSightedFromType<MainCharacter>();
            if (mcs.Count > 0)
            {
                RotateToward(mcs[0].Position);
            }
        }

        private void OnDrawGizmos()
        {
            if (sensor != null)
            {
                foreach (IDetectable detectable in sensor.Sighted)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(transform.position, ((MonoBehaviour)detectable).transform.position);
                }

            }
        }
    }
}