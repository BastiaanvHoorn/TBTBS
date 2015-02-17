using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.scripts;

namespace Assets.scripts.client
{
    /// <summary>
    /// Playfield is built up from tiles
    /// </summary>
    public abstract class Tile
    {
        #region variable declaration
        /// <summary>
        /// Location of the center of the tile in the playfield
        /// </summary>
        public Vector3 position { get; set; }
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

        public Tile() { }
        public void init(Vector3 _position, int _index)
        {
            position = _position;
            index = _index;

            add_vertices(_position);
            add_triangles(_index);
        }

        public bool check_click(Vector2 mouse_pos, Camera camera)
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
            if ((mouse_pos.y > right_bot[0] * mouse_pos.x + right_bot[1]) &&
               (mouse_pos.y < left_top[0] * mouse_pos.x + left_top[1]) &&
               (mouse_pos.y < right_top[0] * mouse_pos.x + right_top[1]) &&
               (mouse_pos.y > left_bot[0] * mouse_pos.x + left_bot[1]) &&
               (mouse_pos.y < _4.y) && (mouse_pos.y > _1.y))
            {
                on_click();
                return true;
            }
            return false;
        }

        protected abstract void on_click();

        /// <summary>
        /// Returns a  vector2 of the position of this tile relative too all other tiles
        /// </summary>
        /// <returns></returns>
        public Vector2 get_grid_pos2(bool rounded = false)
        {
            float x = (float)System.Math.Round(position.x / reference.World.horizontal_space);
            float y = (position.z / reference.World.vertical_space);
            if (rounded)
            {
                y = (float)System.Math.Floor(y);
            }
            return new Vector2(x, y);
        }

        public Vector3 get_grid_pos3(bool rounded = false)
        {
            float x = (float)System.Math.Round(position.x / reference.World.horizontal_space);
            float y = position.y;
            float z = (position.z / reference.World.vertical_space);
            if (rounded)
            {
                z = (float)System.Math.Floor(z);
            }
            return new Vector3(x, y, z);
        }

        #region render stuff
        /// <summary>
        /// Generate the vertices for this tile.
        /// Vertices are not numbered and need to be appended to all vertices of preceding tiles
        /// </summary>
        /// <param name="_position">Position of this tile in the scene</param>
        private void add_vertices(Vector3 _position)
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
                vertices[i] += _position;
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
            if(bottom)
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
            if(bottom && left)
            {
                tris.AddRange(add_triangle(
                    Util.index_of(_vertices2d, vertices2d[0]),
                    Util.index_of(_vertices2d, (vertices2d[0] + left_bottom)),
                    Util.index_of(_vertices2d, (vertices2d[0] + bottom_left)),
                    tile_count,
                    "tri"
                    ));
            }
            if(left)
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
            if(type == "tri")
            {
                index = tile_count;
            }
            else if(type == "right_rect")
            {
                index = tile_count * 2;
            }
            else if(type == "bot_rect")
            {
                index = tile_count * 3;
            }
            else if(type == "left_rect")
            {
                index = tile_count * 4;
            }
            else
            {
                Debug.LogError("something went wrong");
            }
            return new int[] { vertex1 + index, vertex2 + index, vertex3 + index};
        }
        #endregion
    }
}
