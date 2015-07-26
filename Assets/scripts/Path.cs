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
        public Path(Tile start, Tile goal, Tile_manager tile_manager)
        {
            tile_manager.reset_came_from();
            tiles = new List<Tile>();
            List<Tile> frontier = new List<Tile>();
            frontier.Add(start);
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
                    List<Tile> unvisited = new List<Tile>();
                    foreach(Tile _tile in tile_manager.tiles)
                    {
                        if(_tile.came_from == -1)
                        {
                            unvisited.Add(_tile);
                        }
                    }
                    List<Tile> adjecent = get_adjecent(unvisited, tile);
                    foreach(Tile adj_tile in adjecent)
                    {
                        adj_tile.came_from = tile_manager.tiles.IndexOf(tile);
                        frontier.Add(adj_tile);
                    }

                }
            }
            
            while(true)
            {
                if(tile_manager.tiles[came_from] != start)
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

            //List<Tile> unvisited = tile_manager.tiles;
            //List<Tile> frontier = get_adjecent(unvisited, start);
            //foreach (Tile tile in frontier)
            //{
            //    if (tile.position == goal.position)
            //    {
            //        tiles.Add(tile);
            //        return;
            //    }
            //}
        }

        private List<Tile> get_adjecent(List<Tile> tiles, Tile center)
        {
            List<Tile> adjecent = tiles.FindAll(tile => 
                tile.position_axial == center.position_axial + new Vector2(1, 0) || 
                tile.position_axial == center.position_axial + new Vector2(0, 1) ||
                tile.position_axial == center.position_axial + new Vector2(-1, 0) ||
                tile.position_axial == center.position_axial + new Vector2(0, -1) ||
                tile.position_axial == center.position_axial + new Vector2(1, -1) ||
                tile.position_axial == center.position_axial + new Vector2(-1, 1));
            return adjecent;
        }
        public Path()
        {
            tiles = new List<Tile>();
        }
    }
}
