
namespace uqac.timesick.gameplay
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PatrolPoint : MonoBehaviour
    {

        private static Color pink = new Color(1f, 0.66f, 0f);

        private float timeSinceLastVisit = 1f;

        public float TimeSinceLastVisit { get => timeSinceLastVisit; }

        private void Awake()
        {
            //Hide sprite in game
            GetComponent<SpriteRenderer>().enabled = false;
        }
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

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            //show patrol point timer on "gizmo"
            UnityEditor.Handles.Label(transform.position, timeSinceLastVisit.ToString("0."));

            //show patrol point on gizmos
            Gizmos.color = pink;
            Gizmos.DrawWireSphere(transform.position, 0.25f);
        }
        #endif
    }
}
