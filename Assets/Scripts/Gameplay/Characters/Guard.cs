
namespace uqac.timesick.gameplay
{
    using Cawotte.Toolbox.Pathfinding;
    using Cawotte.Toolbox.AI;
    using Sirenix.OdinInspector;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Guard : Character
    {
        private Sensor sightSensor = null;
        private NoiseDetector hearingSensor = null;

        public Sensor SightSensor { get => sightSensor; }

        [SerializeField]
        private TilePath pathToFollow = null;

        [SerializeField]
        private StateMachine<Guard> stateMachine;

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

        protected override void Awake()
        {
            base.Awake();

            sightSensor = GetComponentInChildren<Sensor>();
            sightSensor.Eye = transform;

            hearingSensor = GetComponentInChildren<NoiseDetector>();
            //Init state machine
            stateMachine = new StateMachine<Guard>(new StateIdle(), this);

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            stateMachine.Update();

            HandleMovements();

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

            if (stateMachine != null)
            {

                Gizmos.color = stateMachine.CurrentState.stateColor;
                Gizmos.DrawSphere(Position, 0.2f);
            }
        }
    }
}
