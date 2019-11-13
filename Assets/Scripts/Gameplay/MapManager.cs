
namespace uqac.timesick.gameplay
{
    using Cawotte.Toolbox;
    using Cawotte.Toolbox.Pathfinding;
    using Sirenix.OdinInspector;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    public class MapManager : Singleton<MapManager>
    {

        [SerializeField, Required, SceneObjectsOnly]
        private MainCharacter player;

        [Header("Map Parameters")]
        [SerializeField, Required, SceneObjectsOnly]
        private Tilemap[] obstacleTilemaps;

        [SerializeField, Required, SceneObjectsOnly]
        private Grid grid;


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
