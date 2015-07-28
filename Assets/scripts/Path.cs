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
        public Path(Tile start, Tile goal, Tile_manager tile_manager, Unit unit)
        {
            tile_manager.reset_came_from();
            tiles = new List<Tile>();
            List<Tile> frontier = new List<Tile>();
            frontier.Add(start);
            start.came_from = tile_manager.tiles.IndexOf(start);
            start.shortest_path = 0;
            int came_from;
            while (frontier.Count > 0)
            {
                Tile tile = frontier[0];
                frontier.RemoveAt(0);
                foreach (Tile _tile in tile_manager.tiles)
                {
                    if (Tile_manager.is_in_range(_tile, tile))
                    {
                        if (_tile.is_movable(unit, tile.height))
                        {
                            float move_cost = tile.shortest_path + tile.move_cost / 2 + _tile.move_cost / 2 + System.Math.Abs(_tile.height - tile.height) / 2;
                            if (_tile.shortest_path > move_cost)
                            {
                                _tile.came_from = tile_manager.tiles.IndexOf(tile);
                                _tile.shortest_path = move_cost;
                                frontier.Add(_tile);
                            }
                        }

                    }
                }
            }
            came_from = tile_manager.tiles.IndexOf(goal);
            while (true)
            {
                if (tile_manager.tiles[came_from] != start)
                {
                    tiles.Add(tile_manager.tiles[came_from]);
                    came_from = tile_manager.tiles[came_from].came_from;
                }
                else
                {
                    break;
                }
            }
            tiles.Reverse();
        }
        public Path()
        {
            tiles = new List<Tile>();
        }
    }
}
