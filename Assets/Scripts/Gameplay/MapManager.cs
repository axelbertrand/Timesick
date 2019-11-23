
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
            bool printDebug = false;

            Dictionary<PatrolPoint, float> pickPool = new Dictionary<PatrolPoint, float>();

            Dictionary<PatrolPoint, float> valuePool = new Dictionary<PatrolPoint, float>();

            float cumulatedRandomCoeff = 0f;

            foreach (PatrolPoint pp in patrolPoints)
            {
                float coeffValue = pp.TimeSinceLastVisit *
                    (1 / (2f * Vector2.Distance(guard.Position, pp.transform.position)));
                cumulatedRandomCoeff += coeffValue;

                pickPool.Add(pp, cumulatedRandomCoeff);

                if (printDebug)
                {
                    valuePool.Add(pp, coeffValue);
                }
            }

            float randomVal = UnityEngine.Random.Range(0, cumulatedRandomCoeff);

            //PRINT
            if (printDebug)
            {
                Debug.Log("Chosen value : " + randomVal.ToString("n2"));
                foreach (KeyValuePair<PatrolPoint, float> pick in pickPool.OrderBy(key => key.Value))
                {
                    Debug.Log(pick.Key.gameObject.name +  " coeff : " + valuePool[pick.Key].ToString("n2") + ", Proba : " + (valuePool[pick.Key] / cumulatedRandomCoeff).ToString("n2"), pick.Key);
                }

            }
            //We iterate through the list, and return the first point with cumulated above the picked value.
            foreach (KeyValuePair<PatrolPoint, float> pick in pickPool.OrderBy(key => key.Value))
            {
                if (randomVal <= pick.Value)
                {
                    //Debug.Log("Chosen patrol : " + valuePool[pick.Key], pick.Key);
                    return pick.Key;
                }
            }

            Debug.Log("No point picked ! return last");

            return null;
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
