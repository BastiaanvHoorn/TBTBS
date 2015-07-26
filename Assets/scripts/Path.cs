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
            int came_from;
            while (true)
            {
                Tile tile = frontier[0];
                frontier.RemoveAt(0);
                if (tile.position == goal.position)
                {
                    //tiles.Add(tile);
                    came_from = tile_manager.tiles.IndexOf(tile);
                    break;
                }
                else
                {
                    foreach (Tile _tile in tile_manager.tiles)
                    {
                        if (Tile_manager.is_in_range(_tile, tile))
                        {
                            if (_tile.is_movable(unit, tile.height))
                            {

                                if (_tile.came_from == -1)
                                {
                                    if (Tile_manager.is_in_range(_tile, tile))
                                    {

                                        //unvisited.Add(_tile);
                                        _tile.came_from = tile_manager.tiles.IndexOf(tile);
                                        frontier.Add(_tile);
                                    }
                                }
                            }

                        }
                    }
                }
            }

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
