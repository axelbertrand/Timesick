
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
        private Sensor sensor = null;

        public Sensor Sensor { get => sensor; }

        [SerializeField]
        private TilePath pathToFollow = null;

        [SerializeField]
        private StateMachine<Guard> stateMachine;

        private const float minDistForNextPath = 0.2f;

        protected override void Awake()
        {
            base.Awake();

            sensor = GetComponentInChildren<Sensor>();
            sensor.Eye = transform;

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

            List<MainCharacter> mcs = sensor.GetSightedFromType<MainCharacter>();
            if (mcs.Count > 0)
            {
                RotateToward(mcs[0].Position);
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

            if (stateMachine != null)
            {

                Gizmos.color = stateMachine.CurrentState.stateColor;
                Gizmos.DrawSphere(Position, 0.2f);
            }
        }
    }
}
