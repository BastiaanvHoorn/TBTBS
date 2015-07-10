 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.scripts;
using UnityEngine;
using Assets.scripts.reference;
using Assets.scripts.tile;

namespace Assets.scripts
{
    public class Tile_manager
    {
        #region variable declaration

        public List<Tile> tiles { get; private set; }
        public int count
        {
            get { return tiles.Count; }
        }
        public int vertices_amount;
        public Tile_manager()
        {
            tiles = new List<Tile>();
        }

        public Tile this[int index]
        {
            get
            {
                return tiles[index];
            }
        } 
        #endregion
        #region adding methods

        /// <summary>
        /// Adds a tile to the array of tiles
        /// </summary>
        /// <param name="x">The x-position of the tile in the hex-grid</param>
        /// <param name="y">The height of the tile in the hex-grid</param>
        /// <param name="z">The z-position of the tile in the hex-grid</param>
        public Tile add<Tile_type>(int x, int y, int z) where Tile_type : Tile, new()
        {
            Tile_type tile = new Tile_type();
            
            tile.init(x, y, z, count);
            tiles.Add(tile);
            return tile;
        }

        public Tile add<Tile_type>(Vector3 pos) where Tile_type : Tile, new()
        {
            return add<Tile_type>((int)pos.x, (int)pos.y, (int)pos.z);
        }

        public void add(IEnumerable<Tile> _tiles)
        {
            tiles.AddRange(_tiles);
        }
        #endregion
        /// <summary>
        /// Returns the index of the tile at the given grid position.
        /// If no index is found, the vector will be rounded down.
        /// If no index is found after that, -1 will be returned.
        /// </summary>
        /// <param name="pos">The grid position of the requested tile </param>
        /// <returns></returns>
        public int get_index_by_axial_pos(Vector2 pos)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].position_axial == pos)
                {
                    return i;
                }
            }
            int x = (int)System.Math.Floor(pos.x);
            int y = (int)System.Math.Floor(pos.y);
            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].position_axial == new Vector2(x, y))
                {
                    return i;
                }
            }
            return -1;
        }
        #region momement and range methods
        /// <summary>
        /// Returns true if the two specified tiles are within a certain manhattan range of eachother.
        /// </summary>
        /// <param name="tile_1"></param>
        /// <param name="tile_2"></param>
        static public bool is_in_range(Tile tile_1, Tile tile_2, int range = 1)
        {
            int dx = (int)System.Math.Abs(tile_1.position_cube.x - tile_2.position_cube.x);
            int dy = (int)System.Math.Abs(tile_1.position_cube.y - tile_2.position_cube.y);
            int dz = (int)System.Math.Abs(tile_1.position_cube.z - tile_2.position_cube.z);
            return dx + dy + dz <= range * 2;
        } 
        /// <summary>
        /// Returns all tiles in a certain manhattan range around the specified tile.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public List<Tile> get_tiles_in_range(Tile center, int range = 1)
        {
            List<Tile> in_range_tiles = new List<Tile>();
            Tile_manager manager = new Tile_manager();           
            foreach(Tile tile in tiles)
            {
                if(tile.position_cube == center.position_cube)
                {
                    continue;
                }
                if (is_in_range(center, tile, range))
                {
                    in_range_tiles.Add(manager.add<Test>(Util.v2_to_v3(tile.position_offset, "y", tile.height)));
                    
                }
                
            }
            return in_range_tiles;
        }
        #endregion
        #region render
        //Render stuff

        /// <summary>
        /// Gets all vertices of all registered tiles 5 times;
        /// First set of vertices is used for the tile itself
        /// Second set of vertices is used for connection triangles
        /// Third set of vertices upper left and lower right connection rectangles
        /// Fourth set of vertices top and bottom connection rectangles
        /// Fifth set of vertices upper right and lower left connection rectangles
        /// </summary>
        public Vector3[] get_vertices()
        {
            Vector3[] vertices = new Vector3[] { };

            //Concatenate the vertices of all tiles into one array
            for (int i = 0; i < count; i++)
            {
                vertices = vertices.Concat(tiles[i].vertices).ToArray();
            }

            vertices_amount = vertices.Length;
            //And do it a couple times more
            for (int loop = 1; loop <= 4; loop++)
            {
                for (int i = 0; i < count; i++)
                {
                    vertices = vertices.Concat(tiles[i].vertices).ToArray();
                }
            }


            return vertices;

        }

        /// <summary>
        /// Gets all triangles resembling the playfield
        /// </summary>
        /// <param name="vertices">
        /// All the vertices returned by get_vertices,
        /// I intentionally did not reuse the get_vertices as passing around the array is a lot less resource intensive then re-running that function.
        /// </param>
        public List<int> get_tri(Vector3[] vertices)
        {
            List<int> tri = new List<int>();
            for (int i = 0; i < count; i++)
            {
                tri = tri.Concat(tiles[i].triangles).ToList();
            }

            for (int i = 0; i < count; i++)
            {
                tri.AddRange(tiles[i].get_connection_tris(Util.v3_to_v2(vertices, "y"), vertices_amount));
            }
            return tri;
        }


        #region uv
        /// <summary>
        /// Gets all uv coordinates for the mesh
        /// </summary>
        public Vector2[] get_uv()
        {
            Vector2[] uv = new Vector2[vertices_amount * 5];
            //Loops through one set of vertices
            for (int i = 0; i < vertices_amount; i++)
            {
                int k = i % 6;
                //Creates the uv for the tile itself
                uv[i] = World.get_hex_uv(k);

                //creates the uv for the adjecent triangles and squares
                switch (k)
                {
                    case 0://bottom-left
                        uv[i + vertices_amount * 1] = World.get_tri_uv(1);
                        uv[i + vertices_amount * 3] = World.get_rect_uv(2);
                        uv[i + vertices_amount * 4] = World.get_rect_uv(3);
                        break;
                    case 1://bottom-right                                
                        uv[i + vertices_amount * 1] = World.get_tri_uv(2);
                        uv[i + vertices_amount * 2] = World.get_rect_uv(0);
                        uv[i + vertices_amount * 3] = World.get_rect_uv(3);
                        break;
                    case 2://mid-right                                   
                        uv[i + vertices_amount * 1] = World.get_tri_uv(2);
                        uv[i + vertices_amount * 2] = World.get_rect_uv(1);
                        uv[i + vertices_amount * 4] = World.get_rect_uv(0);
                        break;
                    case 3://top-right                                   
                        uv[i + vertices_amount * 1] = World.get_tri_uv(0);
                        uv[i + vertices_amount * 3] = World.get_rect_uv(0);
                        uv[i + vertices_amount * 4] = World.get_rect_uv(1);
                        break;
                    case 4://top-left                                    
                        uv[i + vertices_amount * 1] = World.get_tri_uv(0);
                        uv[i + vertices_amount * 2] = World.get_rect_uv(2);
                        uv[i + vertices_amount * 3] = World.get_rect_uv(1);
                        break;
                    case 5://mid-left                                    
                        uv[i + vertices_amount * 1] = World.get_tri_uv(1);
                        uv[i + vertices_amount * 2] = World.get_rect_uv(3);
                        uv[i + vertices_amount * 4] = World.get_rect_uv(2);
                        break;
                }
                //The tile
                uv[i + vertices_amount * 0] += tiles[i / 6].tex_location * (float)World.tex_scale;

                Vector2 this_pos = tiles[i / 6].position_axial;

                int index = i / 6;
                int bot = this.get_index_by_axial_pos(this_pos + new Vector2(0, -1));
                int bot_right = this.get_index_by_axial_pos(this_pos + new Vector2(1, -1));
                int top_right = this.get_index_by_axial_pos(this_pos + new Vector2(1, 0));
                int top = this.get_index_by_axial_pos(this_pos + new Vector2(0, 1));
                int top_left = this.get_index_by_axial_pos(this_pos + new Vector2(-1, 1));
                int bot_left = this.get_index_by_axial_pos(this_pos + new Vector2(-1, 0));

                if (k == 0 || k == 1)
                    uv[i + vertices_amount * 3] += get_rect_tex_location(index, bot);
                else if (k == 3 || k == 4)
                    uv[i + vertices_amount * 3] += get_rect_tex_location(index, top);

                if (k == 1 || k == 2)
                    uv[i + vertices_amount * 2] += get_rect_tex_location(index, bot_right);
                else if (k == 4 || k == 5)
                    uv[i + vertices_amount * 2] += get_rect_tex_location(index, top_left);

                if (k == 2 || k == 3)
                    uv[i + vertices_amount * 4] += get_rect_tex_location(index, top_right);
                else if (k == 5 || k == 0)
                    uv[i + vertices_amount * 4] += get_rect_tex_location(index, bot_left);

                if (k == 0)
                {
                    uv[i + vertices_amount * 1] += get_tri_tex_location(index, bot, bot_left);
                }
                if (k == 1)
                {
                    uv[i + vertices_amount * 1] += get_tri_tex_location(index, bot, bot_right);
                }
                else if (k == 2)
                {
                    uv[i + vertices_amount * 1] += get_tri_tex_location(index, top_right, bot_right);
                }
                else if (k == 3)
                {
                    uv[i + vertices_amount * 1] += get_tri_tex_location(index, top, top_right);
                }
                else if (k == 4)
                {
                    uv[i + vertices_amount * 1] += get_tri_tex_location(index, top_left, top);
                }
                else if (k == 5)
                {
                    uv[i + vertices_amount * 1] += get_tri_tex_location(index, top_left, bot_left);
                }
            }
            return uv;
        }

        private Vector2 get_rect_tex_location(int index, int dir)
        {
            if (dir != -1)
            {
                if (tiles[index].tex_prio > tiles[dir].tex_prio)
                {
                    return tiles[index].tex_location * (float)World.tex_scale;
                }
                else
                {
                    return tiles[dir].tex_location * (float)World.tex_scale;
                }
            }
            return new Vector2();
        }
        private Vector2 get_tri_tex_location(int index, int dir1, int dir2)
        {
            if (dir1 != -1)
            {
                if (dir2 != -1)
                {
                    if (tiles[index].tex_prio >= tiles[dir1].tex_prio && tiles[index].tex_prio >= tiles[dir2].tex_prio)
                    {
                        return tiles[index].tex_location * (float)World.tex_scale;
                    }

                    else if (tiles[dir2].tex_prio >= tiles[index].tex_prio && tiles[dir2].tex_prio >= tiles[dir1].tex_prio)
                    {
                        return tiles[dir2].tex_location * (float)World.tex_scale;
                    }

                    else if (tiles[dir1].tex_prio >= tiles[index].tex_prio && tiles[dir1].tex_prio >= tiles[dir2].tex_prio)
                    {
                        return tiles[dir1].tex_location * (float)World.tex_scale;
                    }
                }
            }
            return new Vector2();
        }
        #endregion 
        #endregion
    }
}
