using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace uqac.timesick.gameplay
{
    public class LevelExit : MonoBehaviour
    {
        [SerializeField]
        private MainCharacter mainCharacter;

        private SpriteRenderer spriteRenderer;
        private Collider2D circleCollider;

        private bool isEnabled;
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                spriteRenderer.enabled = isEnabled;
                circleCollider.enabled = isEnabled;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            circleCollider = GetComponent<CircleCollider2D>();
            IsEnabled = false;
            mainCharacter.MedicineStolenEvent += OnMedicineStolenEvents;
        }

        void OnMedicineStolenEvents()
        {
            IsEnabled = true;
            // Add sound and visual effects
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("End of level");
            // Animations + go to debrief scene
        }
    }
}
