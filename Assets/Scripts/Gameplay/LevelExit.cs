using UnityEngine;
using UnityEngine.SceneManagement;

namespace uqac.timesick.gameplay
{
    public class LevelExit : MonoBehaviour
    {
        [SerializeField]
        private MainCharacter mainCharacter;

        [SerializeField]
        private float feedbackCircleRange = 15;

        [SerializeField]
        private float feedbackCircleSpeed = 15;

        [SerializeField]
        private float feedbackCircleThickness = 2;

        [SerializeField]
        private string sceneToLoad = "Debriefing";

        private SpriteRenderer spriteRenderer;
        private Collider2D circleCollider;
        private Transform feedbackTransform;
        private Transform feedbackMaskTransform;

        private float timeSincefeedbackActivation = 0;

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

        private bool isFeedbackActive;

        // Start is called before the first frame update
        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            circleCollider = GetComponent<CircleCollider2D>();
            feedbackTransform = GetComponentsInChildren<Transform>()[1];
            feedbackMaskTransform = GetComponentsInChildren<Transform>()[2];
            IsEnabled = false;
            mainCharacter.MedicineStolenEvent += OnMedicineStolenEvents;
        }

        private void Update()
        {
            if(isFeedbackActive)
            {
                UpdateFeedback();
            }
        }

        void OnMedicineStolenEvents()
        {
            IsEnabled = true;
            isFeedbackActive = true;

            // Add sound effects
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("End of level");

            // TODO: Animations

            SceneManager.LoadSceneAsync(sceneToLoad);
        }

        private void UpdateFeedback()
        {
            timeSincefeedbackActivation += Time.deltaTime;
            float feedbackCircleSize = feedbackCircleSpeed * timeSincefeedbackActivation;
            feedbackTransform.localScale = new Vector3(feedbackCircleSize, feedbackCircleSize, 1);
            feedbackMaskTransform.localScale = new Vector3(feedbackCircleSize - feedbackCircleThickness, feedbackCircleSize - feedbackCircleThickness, 1);
            if (feedbackCircleSize > feedbackCircleRange)
            {
                feedbackTransform.localScale = new Vector3();
                isFeedbackActive = false;
            }
        }
    }
}
