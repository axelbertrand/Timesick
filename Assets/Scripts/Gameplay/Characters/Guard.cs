
namespace uqac.timesick.gameplay
{
    using Cawotte.Toolbox.Pathfinding;
    using Cawotte.Toolbox.AI;
    using Sirenix.OdinInspector;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    public class Guard : Character
    {
        #region Members and Properties
        private Sensor sightSensor = null;
        private NoiseDetector hearingSensor = null;

        [SerializeField, Required, AssetsOnly]
        private GameObject bulletPrefab;

        [Title("Sensor")]
        [SerializeField]
        private float reducedLineOfSight = 0.7f;

        [Title("Shooting")]
        [SerializeField]
        private float rateOfFire = 2f;
        private float timerBulletFire = 0f;

        [Title("Pathfinding")]
        [SerializeField]
        private TilePath pathToFollow = null;

        private int maxSizeLastPatrolVisited = 5;
        private Queue<PatrolPoint> lastPatrolsVisited = new Queue<PatrolPoint>();

        //ACTION
        public Action<PatrolPoint> OnPatrolVisit = null;


        [Title("AI")]
        //STATE MACHINE
        [SerializeField]
        private StateMachine<Guard> stateMachine;

        [SerializeField]
        private bool isAlerted = false;

        [SerializeField]
        private FadingPopup questionMarkPopup;
        [SerializeField]
        private FadingPopup exclamationMarkPopup;


        //SENSOR
        public Sensor SightSensor { get => sightSensor; }
        public NoiseDetector HearingSensor { get => hearingSensor; }


        private const float minDistForNextPath = 0.2f;

        public bool IsAlerted {
            get => isAlerted;
            set
            {
                isAlerted = value;

                if (isAlerted)
                {
                    currentSpeed = sprintingSpeed;
                }
                else
                {
                    currentSpeed = walkingSpeed;
                }
            }
        }

        #region Audio properties
        [Header("Audio properties")]
        [SerializeField]
        private string chaseSoundName="";
        [SerializeField]
        private string lostSoundName="";

        public string ChaseSoundName
        {
            get => chaseSoundName;
            protected set
            {
                chaseSoundName = value;
            }
        }

        public string LostSoundName
        {
            get => lostSoundName;
            protected set
            {
                lostSoundName = value;
            }
        }


        #endregion

        public StateMachine<Guard> StateMachine { get => stateMachine; }
        public Queue<PatrolPoint> LastPatrolsVisited { get => lastPatrolsVisited; }

        #endregion
        protected override void Awake()
        {
            base.Awake();

            sightSensor = GetComponentInChildren<Sensor>();
            sightSensor.Eye = transform;

            hearingSensor = GetComponentInChildren<NoiseDetector>();

            OnPatrolVisit += AddPatrolToVisited;

        }

        // Start is called before the first frame update
        void Start()
        {

            exclamationMarkPopup.Init();
            questionMarkPopup.Init();

            MapManager.Instance.RegisterGuard(this);


            //Init state machine
            stateMachine = new StateMachine<Guard>(
                new StatePatrol(MapManager.Instance.PickPointToPatrol(this)), this);
        }

        // Update is called once per frame
        void Update()
        {
            stateMachine.Update();

            HandleMovements();

            /*
            List<Vector4> noises = hearingSensor.GetHeardNoises();
            if(noises.Count > 0)
            {
                GetComponent<SpriteRenderer>().color = Color.red;
            } */
            sightSensor.UpdateRotation(currentAngle);

            if (timerBulletFire > 0f)
            {
                timerBulletFire -= Time.deltaTime;
            }
        }

        public void StopMovement()
        {
            pathToFollow = null;
            isMoving = false;
        }

        public void SetWalkingSpeed()
        {

        }

        public void GoTo(Vector2 worldPos)
        {
            pathToFollow = MapManager.Instance.GetPathFromTo(Position, worldPos);
        }

        public void GoToPlayerPosition()
        {
            pathToFollow = MapManager.Instance.GetPathToPlayer(Position);
        }

        private void HandleMovements()
        {

            bool hasPath = (pathToFollow != null && !pathToFollow.IsEmpty);

            //Follow path if there's one.
            if (hasPath)
            {
                //Follow path if there's one.


                TileNode current = pathToFollow.IteratorSimplePath.Current;

                //If the guard is not close enough to the next path point, move towards it.
                if ( Vector2.Distance(Position, current.CenterWorld) > minDistForNextPath)
                {
                    MoveToward(current.CenterWorld);
                }
                else
                {
                    //We reached a tile, we go for the next one
                    if (pathToFollow.IteratorSimplePath.HasNext())
                    {
                        current = pathToFollow.IteratorSimplePath.Next();
                        MoveToward(current.CenterWorld);
                    }
                    //If there' no next one, we stahp
                    else
                    {
                        pathToFollow = null;
                        hasPath = false;
                    }
                }
            }

            isMoving = hasPath;
        }

        private void AddPatrolToVisited(PatrolPoint pp)
        {
            lastPatrolsVisited.Enqueue(pp);
            if (lastPatrolsVisited.Count > maxSizeLastPatrolVisited)
            {
                lastPatrolsVisited.Dequeue();
            }
        }
         
        /// <summary>
        /// If false, set line of sight to default size, else to extended size.
        /// Extended size is only used for SHOOTING to avoid lose of sight too fast.
        /// </summary>
        /// <param name="isExtended"></param>
        public void SetLineOfSightAsReduced(bool isExtended)
        {
            if (!isExtended)
            {
                sightSensor.MultiplySizeSight(1f / reducedLineOfSight);
            }
            else
            {
                sightSensor.MultiplySizeSight(reducedLineOfSight);
            }
        }

        public void TryShootAt(Vector2 pos)
        {
            if (timerBulletFire <= 0f)
            {
                ShootAt(pos);
            }
        }

        private void ShootAt(Vector2 pos)
        {
            Bullet bullet = GameObject.Instantiate(bulletPrefab, Position, Quaternion.identity).GetComponent<Bullet>();

            bullet.Direction = pos - Position;

            bullet.gameObject.SetActive(true);

            timerBulletFire = 1 / rateOfFire;
        }

        public void ShowExclamationPopup()
        {
            questionMarkPopup.Hide();
            exclamationMarkPopup.Show();
        }


        public void ShowQuestionPopup()
        {
            exclamationMarkPopup.Hide();
            questionMarkPopup.Show();
        }

        private void OnDrawGizmos()
        {

            //draw current state
            if (stateMachine != null)
            {

                Gizmos.color = stateMachine.CurrentState.stateColor;
                Gizmos.DrawSphere(Position, 0.2f);

                /*
                if (stateMachine.CurrentState is StatePatrol)
                {
                    Gizmos.DrawLine(Position, ((StatePatrol)stateMachine.CurrentState).PatrolPoint.transform.position);

                } */
            }

            if (pathToFollow != null && pathToFollow.IteratorSimplePath != null)
            {
                int startIndex = Mathf.Max(0, pathToFollow.IteratorSimplePath.IndexCurrent - 1);
                for (int i = startIndex; i < pathToFollow.SimplifiedPath.Size - 1; i++)
                {
                    Gizmos.DrawLine(pathToFollow.SimplifiedPath[i].CenterWorld, pathToFollow.SimplifiedPath[i + 1].CenterWorld);
                }

            }
        }
    }
}
