using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts
{
    /// <summary>
    /// Playfield is built up from tiles
    /// </summary>
    public abstract class Tile
    {
        #region variable declaration
        /// <summary>
        /// Location of the center of the tile in world coordinates
        /// </summary>
        public Vector3 position { get; private set; }
        /// <summary>
        /// Cube coordinates of this tile
        /// </summary>
        public Vector3 position_cube { get; private set; }
        /// <summary>
        /// Axial coordinates of this tile
        /// </summary>
        public Vector2 position_axial { get; private set; }
        /// <summary>
        /// Offset coordinates of this tile
        /// </summary>
        public Vector2 position_offset { get; private set; }
        /// <summary>
        /// The elevation of this tile
        /// </summary>
        public float height { get; private set; }
        /// <summary>
        /// All vertices for this tile (total of 6)
        /// </summary>
        public List<Vector3> vertices { get; set; }
        /// <summary>
        /// All triangles for this tile (4 triangels, total of 12 ints)
        /// </summary>
        public List<int> triangles { get; set; }
        /// <summary>
        /// Index of this tile, used to number vertices.
        /// </summary>
        public int index { get; set; }
        /// <summary>
        /// Returns the bottom left pixel of the textures of this tile
        /// </summary>
        public abstract Vector2 tex_location { get; }
        public abstract int tex_prio { get; }
        #endregion
        #region initializing

        /// <summary>
        /// Creates a tile at the given coordinates
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">height</param>
        /// <param name="z"><b>offset</b> z</param>
        /// <param name="_index"></param>
        public void init(int x, float _height, int z, int _index)
        {
            height = _height;
            set_positions(x, z);

            index = _index;

            add_vertices();
            add_triangles(_index);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void set_positions(int x, int y)
        {
            position_offset = new Vector3(x, y);
            float col;
            float row;
            col = x * reference.World.horizontal_space;

            if (x % 2 == 1)
            {
                row = y * reference.World.vertical_space + reference.World.vertical_offset;
            }
            else
            {
                row = y * reference.World.vertical_space;
            }
            position = new Vector3(col, height, row);

            int _x = x;
            int _y;

            if (x % 2 == 1)
            {
                _y = y - ((x - 1) / 2);
            }
            else
            {
                _y = y - (x / 2);
            }

            int _z = -_x - _y;
            position_axial = new Vector2(_x, _y);
            position_cube = new Vector3(_x, _y, _z);

        }
        #endregion
        /// <summary>
        /// Checks if it is possible to attack this tile
        /// </summary>
        /// <param name="unit_manager"></param>
        /// <param name="_player">The unit that wants to attack</param>
        /// <returns></returns>
        public Unit is_attackable(Unit_manager unit_manager, Unit attacker)
        {
            Unit unit = null;
            foreach (Unit defender in unit_manager.units)
            {
                if(defender.player != attacker.player)
                {
                    if(defender.occupiying_tile.position_axial == attacker.next_tile.position_axial)
                    {
                        unit = defender;
                    }
                }
            }
            //Unit unit = unit_manager.units.Find(defender => attacker.occupiying_tile.position_axial == attacker.next_tile.position_axial && attacker.player != defender.player);
            return unit;
        }
        public bool is_movable(Unit_manager unit_manager, Unit _unit)
        {

            if ((_unit.next_tile.height - height) > 2)//unit_manager.get_unit_by_next_tile(this) != null
            {
                string s = string.Format("Can't move {0} to {1}", _unit.to_string(), this.position_axial);
                Debug.Log(s);
                return false;

            }
            return true;
        }

        /// <summary>
        /// Returns true if the given pixel is part of this tile
        /// </summary>
        /// <param name="pixel"></param>
        /// <param name="camera"></param>
        /// <returns></returns>
        public bool is_pixel_of_tile(Vector2 pixel, Camera camera)
        {
            // Convert all corners of this tile to 2d coordinates on the screen
            Vector2 _0 = Util.v3_to_v2(camera.WorldToScreenPoint(position + Util.v2_to_v3(reference.World.vertex0, "y")), "z");
            Vector2 _1 = Util.v3_to_v2(camera.WorldToScreenPoint(position + Util.v2_to_v3(reference.World.vertex1, "y")), "z");
            Vector2 _2 = Util.v3_to_v2(camera.WorldToScreenPoint(position + Util.v2_to_v3(reference.World.vertex2, "y")), "z");
            Vector2 _3 = Util.v3_to_v2(camera.WorldToScreenPoint(position + Util.v2_to_v3(reference.World.vertex3, "y")), "z");
            Vector2 _4 = Util.v3_to_v2(camera.WorldToScreenPoint(position + Util.v2_to_v3(reference.World.vertex4, "y")), "z");
            Vector2 _5 = Util.v3_to_v2(camera.WorldToScreenPoint(position + Util.v2_to_v3(reference.World.vertex5, "y")), "z");

            // All skewed lines that surround this tile
            float[] right_bot = Util.get_line(_1, _2);
            float[] right_top = Util.get_line(_2, _3);
            float[] left_top = Util.get_line(_5, _4);
            float[] left_bot = Util.get_line(_0, _5);

            // Check if the mouse is between those lines
            if ((pixel.y > right_bot[0] * pixel.x + right_bot[1]) &&
               (pixel.y < left_top[0] * pixel.x + left_top[1]) &&
               (pixel.y < right_top[0] * pixel.x + right_top[1]) &&
               (pixel.y > left_bot[0] * pixel.x + left_bot[1]) &&
               (pixel.y < _4.y) && (pixel.y > _1.y))
            {
                on_click();
                return true;
            }
            return false;
        }

        protected abstract void on_click();

        #region render stuff
        /// <summary>
        /// Generate the vertices for this tile.
        /// Vertices are not numbered and need to be appended to all vertices of preceding tiles
        /// </summary>
        /// <param name="_position">Position of this tile in the scene</param>
        private void add_vertices()
        {
            vertices = new List<Vector3>{
                Util.v2_to_v3(reference.World.vertex0, "y"),
                Util.v2_to_v3(reference.World.vertex1, "y"),
                Util.v2_to_v3(reference.World.vertex2, "y"),
                Util.v2_to_v3(reference.World.vertex3, "y"),
                Util.v2_to_v3(reference.World.vertex4, "y"),
                Util.v2_to_v3(reference.World.vertex5, "y"),
            };
            for (int i = 0; i < 6; i++)
            {
                vertices[i] += position;
            }

        }
        /// <summary>
        /// Generate the triangles for this tile.
        /// Tile consists of 4 triangles
        /// </summary>
        /// <param name="_index">Index of the tile relative to all tiles</param>
        private void add_triangles(int _index)
        {
            triangles = new List<int> {
                0,2,1,
                0,3,2,
                0,5,3,
                3,5,4,
            };

            for (int i = 0; i < 12; i++)
            {
                triangles[i] += _index * 6;

            }
        }
        /// <summary>
        /// Returns the an int[] containing triangles below this tile to fill up the space between the next 3 tiles.
        /// Triangles given in 3 consequent ints. Triangles facing up
        /// </summary>
        /// <param name="_vertices">All vertices of the playfield</param>
        /// <param name="tile_count">The amount of tiles on the playfield</param>
        public int[] get_connection_tris(Vector2[] _vertices2d, int tile_count)
        {
            /// The following vectors represent the bottom half of the vertices of the adjecent hexagons
            Vector2 top_right = new Vector2(3 + reference.Math.cos_30, reference.Math.sqrt_3 - .5f); //The upper vertex of the right-side hexagon 
            Vector2 bottom_right = new Vector2(2 + reference.Math.cos_30, -.5f); //The lower vertex of the right-side hexagon
            Vector2 right_bottom = new Vector2(2, -1); //The right vertex of the bottom hexagon
            Vector2 left_bottom = new Vector2(0, -1); //The left vertex of the bottom hexagon
            Vector2 bottom_left = new Vector2(-reference.Math.cos_30, -.5f); //The bottom vertex of the left hexagon
            Vector2 top_left = new Vector2(-(reference.Math.cos_30 + 1), reference.Math.sqrt_3 - .5f); //The top vertex of the left hexagon

            Vector2[] vertices2d = Util.v3_to_v2(vertices.ToArray(), "y");// And do the same for the positions of the corners of this tile

            bool right = false;
            bool bottom = false;
            bool left = false;

            for (int i = 0; i < vertices.Count; i++)
            {
                vertices2d[i] = Util.v3_to_v2(vertices[i], "y");
            }

            List<int> tris = new List<int>();

            if (Util.index_of(_vertices2d, (vertices2d[0] + top_right)) != -1) // Right side
            {
                right = true;
            }

            if (Util.index_of(_vertices2d, (vertices2d[0] + right_bottom)) != -1) // Bottom side
            {
                bottom = true;
            }

            if (Util.index_of(_vertices2d, (vertices2d[0] + bottom_left)) != -1) // Left side
            {
                left = true;
            }

            if (right)
            {
                tris.AddRange(add_triangle(
                    Util.index_of(_vertices2d, vertices2d[1]),
                    Util.index_of(_vertices2d, vertices2d[2]),
                    Util.index_of(_vertices2d, (vertices2d[0] + top_right)),
                    tile_count,
                    "right_rect"
                    ));
                tris.AddRange(add_triangle(
                    Util.index_of(_vertices2d, vertices2d[1]),
                    Util.index_of(_vertices2d, (vertices2d[0] + top_right)),
                    Util.index_of(_vertices2d, (vertices2d[0] + bottom_right)),
                    tile_count,
                    "right_rect"
                    ));
            }
            if (right && bottom)
            {
                tris.AddRange(add_triangle(
                    Util.index_of(_vertices2d, vertices2d[1]),
                    Util.index_of(_vertices2d, (vertices2d[0] + bottom_right)),
                    Util.index_of(_vertices2d, (vertices2d[0] + right_bottom)),
                    tile_count,
                    "tri"
                    ));
            }
            if (bottom)
            {
                tris.AddRange(add_triangle(
                    Util.index_of(_vertices2d, vertices2d[0]),
                    Util.index_of(_vertices2d, vertices2d[1]),
                    Util.index_of(_vertices2d, (vertices2d[0] + right_bottom)),
                    tile_count,
                    "bot_rect"
                    ));
                tris.AddRange(add_triangle(
                    Util.index_of(_vertices2d, vertices2d[0]),
                    Util.index_of(_vertices2d, (vertices2d[0] + right_bottom)),
                    Util.index_of(_vertices2d, (vertices2d[0] + left_bottom)),
                    tile_count,
                    "bot_rect"
                    ));

            }
            if (bottom && left)
            {
                tris.AddRange(add_triangle(
                    Util.index_of(_vertices2d, vertices2d[0]),
                    Util.index_of(_vertices2d, (vertices2d[0] + left_bottom)),
                    Util.index_of(_vertices2d, (vertices2d[0] + bottom_left)),
                    tile_count,
                    "tri"
                    ));
            }
            if (left)
            {
                tris.AddRange(add_triangle(
                    Util.index_of(_vertices2d, vertices2d[5]),
                    Util.index_of(_vertices2d, vertices2d[0]),
                    Util.index_of(_vertices2d, (vertices2d[0] + bottom_left)),
                    tile_count,
                    "left_rect"
                    ));
                tris.AddRange(add_triangle(
                    Util.index_of(_vertices2d, vertices2d[5]),
                    Util.index_of(_vertices2d, (vertices2d[0] + bottom_left)),
                    Util.index_of(_vertices2d, (vertices2d[0] + top_left)),
                    tile_count,
                    "left_rect"
                    ));
            }


            return tris.ToArray();
        }
        private int[] add_triangle(int vertex1, int vertex2, int vertex3, int tile_count, string type)
        {
            //Debug.Log("added triangle: " + vertex1 + ", " + vertex2 + ", " + vertex3);
            int index = 0;
            if (type == "tri")
            {
                index = tile_count;
            }
            else if (type == "right_rect")
            {
                index = tile_count * 2;
            }
            else if (type == "bot_rect")
            {
                index = tile_count * 3;
            }
            else if (type == "left_rect")
            {
                index = tile_count * 4;
            }
            else
            {
                Debug.LogError("something went wrong");
            }
            return new int[] { vertex1 + index, vertex2 + index, vertex3 + index };
        }
        #endregion
    }
}
