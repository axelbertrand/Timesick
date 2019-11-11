

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

        [SerializeField]
        private bool seeThroughWall = false;

        [HideInInspector]
        public Action<IDetectable> OnSight = null; //events to invoke on Sight of new detectable

        [HideInInspector]
        public Action<IDetectable> OnLoseOfSight = null; //events to invoke on lose of sight

        private Transform eye = null;

        public List<IDetectable> Sighted {
            get => sighted;
        }

        public Transform Eye { get => eye; set => eye = value; }

        private void Awake()
        {
            sighted = new List<IDetectable>();
        }

        public List<T> GetSightedFromType<T>()
        {
            return sighted.OfType<T>().ToList();
        }

        private bool HasLineOfSightOn(IDetectable detectable)
        {
            GameObject go = detectable.gameObject;
            Vector2 direction = detectable.Position - (Vector2)eye.position;

            ContactFilter2D filter = new ContactFilter2D();
            List<RaycastHit2D> hits = new List<RaycastHit2D>();

            Physics2D.Raycast(eye.position, direction.normalized, filter, hits);

            //A wall has been hit!

            foreach (RaycastHit2D hit in hits)
            {

                //Is a wall hit first ?
                if (LayerMask.LayerToName(hit.collider.gameObject.layer).Equals("Wall"))
                {
                    return false;
                }
                //Is the player hit first ?
                else if (hit.collider.gameObject.Equals(detectable.gameObject))
                {
                    return true;
                }

            }

            return true;
        }

        private void AddToSighted(IDetectable detectable)
        {

            //Add it to the sighted list, and invoke events.
            OnSight?.Invoke(detectable);
            sighted.Add(detectable);
        }

        private void RemoveFromSight(IDetectable detectable)
        {
            //Remove it from the sighted list, and invoke events it it was in the list.
            if (sighted.Remove(detectable))
            {
                OnLoseOfSight?.Invoke(detectable);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IDetectable detectable = collision.GetComponent<IDetectable>();

            //If a detectable entered the line of sight
            if (detectable != null)
            {
                if (seeThroughWall || 
                    (!seeThroughWall && HasLineOfSightOn(detectable)))
                {
                    AddToSighted(detectable);
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (seeThroughWall)
                return;

            IDetectable detectable = collision.GetComponent<IDetectable>();

            //If it's not yet sighted

            if (detectable != null) {

                if (!sighted.Contains(detectable))
                {
                    //Check if it's in the line of sight, if yes add it
                    if (HasLineOfSightOn(detectable))
                    {
                        AddToSighted(detectable);
                    }
                }
                else
                {
                    //Check if it was lost from sight, if yes remove it
                    if (!HasLineOfSightOn(detectable))
                    {
                        RemoveFromSight(detectable);
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {

            IDetectable detectable = collision.GetComponent<IDetectable>();

            //If a detectable exited the line of sight
            if (detectable != null)
            {
                //Remove it from the sighted list, and invoke events it it was in the list.
                RemoveFromSight(detectable);
            }
        }

    }
}
