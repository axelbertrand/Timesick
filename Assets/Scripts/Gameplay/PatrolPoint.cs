
namespace uqac.timesick.gameplay
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PatrolPoint : MonoBehaviour
    {

        private float timeSinceLastVisit = 1f;

        public float TimeSinceLastVisit { get => timeSinceLastVisit; }

        private void Start()
        {
            MapManager.Instance.RegisterPatrolPoint(this);
        }

        private void Update()
        {
            timeSinceLastVisit += Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Guard guard = collision.GetComponent<Guard>();
            if (guard != null)
            {
                timeSinceLastVisit = 0f;
                guard.OnPatrolVisit?.Invoke(this);
            }
        }

        private void OnDrawGizmos()
        {
            UnityEditor.Handles.Label(transform.position, timeSinceLastVisit.ToString("0."));
        }
    }
}
