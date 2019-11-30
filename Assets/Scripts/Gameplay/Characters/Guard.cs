
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
        private float shootLineOfSightMultiplier = 1.5f;

        [Title("Shooting")]
        [SerializeField]
        private float rateOfFire = 2f;
        private float timerBulletFire = 0f;

        [Title("Pathfinding")]
        [SerializeField]
        private TilePath pathToFollow = null;


        //ACTION
        public Action<PatrolPoint> OnPatrolVisit = null;


        [Title("AI")]
        //STATE MACHINE
        [SerializeField]
        private StateMachine<Guard> stateMachine;

        //SENSOR
        public Sensor SightSensor { get => sightSensor; }
        public NoiseDetector HearingSensor { get => hearingSensor; }




        private const float minDistForNextPath = 0.2f;

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

        #endregion
        protected override void Awake()
        {
            base.Awake();

            sightSensor = GetComponentInChildren<Sensor>();
            sightSensor.Eye = transform;

            hearingSensor = GetComponentInChildren<NoiseDetector>();

        }

        // Start is called before the first frame update
        void Start()
        {
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
                    MoveToward(current.CenterWorld,false);
                }
                else
                {
                    //We reached a tile, we go for the next one
                    if (pathToFollow.IteratorSimplePath.HasNext())
                    {
                        current = pathToFollow.IteratorSimplePath.Next();
                        MoveToward(current.CenterWorld,false);
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
         
        /// <summary>
        /// If false, set line of sight to default size, else to extended size.
        /// Extended size is only used for SHOOTING to avoid lose of sight too fast.
        /// </summary>
        /// <param name="isExtended"></param>
        public void SetLineOfSightAsExtended(bool isExtended)
        {
            if (!isExtended)
            {
                sightSensor.MultiplySizeSight(1f / shootLineOfSightMultiplier);
            }
            else
            {
                sightSensor.MultiplySizeSight(shootLineOfSightMultiplier);
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
