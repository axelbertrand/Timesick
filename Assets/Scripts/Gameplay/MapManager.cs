
namespace uqac.timesick.gameplay
{
    using Cawotte.Toolbox;
    using Cawotte.Toolbox.Pathfinding;
    using Sirenix.OdinInspector;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Tilemaps;
    using System.Linq;

    public class MapManager : Singleton<MapManager>
    {

        [SerializeField]
        private MainCharacter player;

        [ShowInInspector]
        private List<Guard> guards = new List<Guard>();

        [Header("Map Parameters")]
        [SerializeField, Required, SceneObjectsOnly]
        private Tilemap[] obstacleTilemaps;

        [SerializeField, Required, SceneObjectsOnly]
        private Grid grid;

        [ShowInInspector]
        private List<PatrolPoint> patrolPoints = new List<PatrolPoint>();

        [SerializeField]
        private Map map;

        [SerializeField]
        private bool diagonalMovements = false;

        private Pathfinder pathfinder;

        public MainCharacter Player { get => player; }

        private void Awake()
        {
            map = new Map(grid, obstacleTilemaps);
            pathfinder = new Pathfinder(map, diagonalMovements);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void RegisterPlayer(MainCharacter player)
        {
            this.player = player;
        }

        public void RegisterGuard(Guard guard)
        {
            guards.Add(guard);
        }

        public void RegisterPatrolPoint(PatrolPoint patrolPoint)
        {
            patrolPoints.Add(patrolPoint);
        }

        public void notifyAlert()
        {

        }

        public PatrolPoint PickPointToPatrol(Guard guard)
        {
            //nb of closest point to choose from
            int nbClosestPoint = 5;

            List<PatrolPoint> pickPool = new List<PatrolPoint>();

            //Pick the X closest patrol points
            foreach (PatrolPoint pp in patrolPoints)
            {
                //If there's not enough point in the pool, add them
                if (pickPool.Count <= nbClosestPoint)
                {
                    pickPool.Add(pp);

                }
                //Else, replace with a new one if it's closer that the farthest one
                else
                {

                    //Order by descending distance with guard
                    pickPool = pickPool.OrderByDescending(
                        (point) => Vector2.Distance(guard.Position, point.transform.position)
                    ).ToList();

                    float distanceCurrentPoint = Vector2.Distance(guard.Position, pp.transform.position);
                    float distanceFarthestPoint = Vector2.Distance(guard.Position, pickPool[0].transform.position);

                    if (distanceCurrentPoint < distanceFarthestPoint)
                    {
                        pickPool.RemoveAt(0);
                        pickPool.Insert(0, pp);
                    }
                }
            }

            //Pick one randomly with bias toward the least visited one.
            float randomRange = 0f;
            foreach (PatrolPoint pp in pickPool)
            {
                randomRange += pp.TimeSinceLastVisit;
            }

            float randVal = UnityEngine.Random.Range(0, randomRange);

            randomRange = 0f;
            foreach (PatrolPoint pp in pickPool)
            {
                randomRange += pp.TimeSinceLastVisit;

                if (randVal <= randomRange)
                    return pp;
            }

            Debug.Log("No point picked ! return last");

            return pickPool[pickPool.Count - 1];
        }

        public TilePath GetPathFromTo(Vector2 startWorldpos, Vector2 endWorldPos)
        {
            return pathfinder.GetPath(startWorldpos, endWorldPos);
        }

        public TilePath GetPathToPlayer(Vector2 startWorldPos)
        {
            return pathfinder.GetPath(startWorldPos, player.Position);
        }
    }
}
