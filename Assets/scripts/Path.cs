using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Path
    {
        public List<Tile> tiles { get; private set; }
        public Tile start { get; set; }
        public Tile goal { get; set; }
        public bool completed = false;
        public Tile next { get { return tiles[0]; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_start">Cube position of the starting tile</param>
        /// <param name="_goal">Cube position of the goal tile</param>
        /// <param name="tile_manager"></param>
        /// <returns></returns>
        public Path(Vector3 _start, Vector3 _goal, Tile_manager tile_manager)
        {
            tiles = new List<Tile>();
            Tile start = tile_manager.get_tile_by_pos(_start);
            tiles.Add(tile_manager.get_tile_by_pos(_goal, "cube"));

        }
        public Path()
        {
            tiles = new List<Tile>();
        }
    }
}
