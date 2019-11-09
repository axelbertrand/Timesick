

namespace uqac.timesick.gameplay
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using System.Linq;
    using Sirenix.OdinInspector;

    [Serializable]
    public class DetectableSensor : SerializedMonoBehaviour
    {
        [ShowInInspector, ReadOnly]
        private List<IDetectable> sighted = new List<IDetectable>();

        [HideInInspector]
        public Action<IDetectable> OnSight = null;

        [HideInInspector]
        public Action<IDetectable> OnLoseOfSight = null;

        public List<IDetectable> Sighted {
            get => sighted;
        }

        private void Awake()
        {
            sighted = new List<IDetectable>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IDetectable detectable = collision.GetComponent<IDetectable>();
            if (detectable != null)
            {
                OnSight?.Invoke(detectable);
                sighted.Add(detectable);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {

            IDetectable detectable = collision.GetComponent<IDetectable>();
            if (detectable != null)
            {
                sighted.Remove(detectable);
                OnLoseOfSight?.Invoke(detectable);
            }
        }

        public List<T> GetSightedFromType<T>()
        {
            return sighted.OfType<T>().ToList();
        }
    }
}
