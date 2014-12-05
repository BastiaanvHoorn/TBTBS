using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.scripts;
using UnityEngine;
using Assets.scripts.reference;

namespace Assets.scripts
{
    class Tile_manager
    {
        public List<Tile> tiles { get; private set; }
        public int count {
            get{return tiles.Count;}  
        }
        public int vertices_amount;
        public Tile_manager()
        {
            tiles = new List<Tile>();
        }
        /// <summary>
        /// Adds a tile to the array of tiles
        /// </summary>
        /// <param name="x">The x-position of the tile in the hex-grid</param>
        /// <param name="y">The height of the tile in the hex-grid</param>
        /// <param name="z">The z-position of the tile in the hex-grid</param>
        public void add_tile<Tile_type>(int x, int y, int z) where Tile_type:Tile , new()
        {
            Tile_type _tile = new Tile_type();
            Vector3 _position;
            if (x % 2 == 1)
            {
                _position = new Vector3(x * reference.World.horizontal_space, y, z * reference.World.vertical_space + reference.World.vertical_offset);
            }
            else
            {
                _position = new Vector3(x * reference.World.horizontal_space, y, z * reference.World.vertical_space);
            }
            _tile.init(_position, count);
            tiles.Add(_tile);
        }

        public Vector3[] get_vertices()
        {
            Vector3[] vertices = new Vector3[] { };

            for (int i = 0; i < count; i++)
            {
                vertices = vertices.Concat(tiles[i].vertices).ToArray();
            }

            vertices_amount = vertices.Length;
            for (int loop = 1; loop <= 4; loop++)
            {
                //Second set of vertices is used for connection triangles
                //Third set of vertices upper left and lower right connection triangles
                //Fourth set of vertices top and bottom connection rectangles
                //Fifth set of vertices upper right and lower left connection rectangles
                for (int i = 0; i < count; i++)
                {
                    vertices = vertices.Concat(tiles[i].vertices).ToArray();
                }
            }


            return vertices;

        }
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
        public Vector2[] get_uv()
        {
            Vector2[] uv = new Vector2[vertices_amount*5];
            for (int i = 0; i < vertices_amount; i++)
            {
                int k = i % 6;
                uv[i] = World.hex_uv[k] * (float)(World.tex_scale);
                switch (k)
                {
                    case 0:
                        uv[i + vertices_amount * 1] = World.tri_uv[1] * (float)World.tex_scale;
                        uv[i + vertices_amount * 3] = World.rect_uv[2] * (float)World.tex_scale;
                        uv[i + vertices_amount * 4] = World.rect_uv[3] * (float)World.tex_scale;
                        break;
                    case 1:
                        uv[i + vertices_amount * 1] = World.tri_uv[2] * (float)World.tex_scale;
                        uv[i + vertices_amount * 2] = World.rect_uv[0] * (float)World.tex_scale;
                        uv[i + vertices_amount * 3] = World.rect_uv[3] * (float)World.tex_scale;
                        break;
                    case 2:
                        uv[i + vertices_amount * 1] = World.tri_uv[2] * (float)World.tex_scale;
                        uv[i + vertices_amount * 2] = World.rect_uv[1] * (float)World.tex_scale;
                        uv[i + vertices_amount * 4] = World.rect_uv[0] * (float)World.tex_scale;
                        break;
                    case 3:
                        uv[i + vertices_amount * 1] = World.tri_uv[0] * (float)World.tex_scale;
                        uv[i + vertices_amount * 3] = World.rect_uv[0] * (float)World.tex_scale;
                        uv[i + vertices_amount * 4] = World.rect_uv[1] * (float)World.tex_scale;
                        break;
                    case 4:
                        uv[i + vertices_amount * 1] = World.tri_uv[0] * (float)World.tex_scale;
                        uv[i + vertices_amount * 2] = World.rect_uv[2] * (float)World.tex_scale;
                        uv[i + vertices_amount * 3] = World.rect_uv[1] * (float)World.tex_scale;
                        break;
                    case 5:
                        uv[i + vertices_amount * 1] = World.tri_uv[1] * (float)World.tex_scale;
                        uv[i + vertices_amount * 2] = World.rect_uv[3] * (float)World.tex_scale;
                        uv[i + vertices_amount * 4] = World.rect_uv[2] * (float)World.tex_scale;
                        break;
                }
            }
            return uv;
        }

    }
}
