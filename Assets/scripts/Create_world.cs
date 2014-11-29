using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.scripts;
using Assets.scripts.reference;
using Assets.scripts.tile;

namespace Assets.scripts
{
    public class Create_world : MonoBehaviour
    {
        List<Tile> tiles = new List<Tile>();
        public GameObject focus;
        public Rigidbody camera_rb;
        public Animator focus_an;
        private int original_length;
        public Texture texture;
        void Start()
        {
            MeshFilter mf = GetComponent<MeshFilter>();
            var mesh = new Mesh();
            mf.mesh = mesh;

            add_tiles();
            Vector3[] vertices = new Vector3[] { };
            vertices = get_vertices();
            

            List<int> tri = new List<int>();

            for (int i = 0; i < tiles.Count; i++)
            {
                tri = tri.Concat(tiles[i].triangles).ToList();
            }

            for (int i = 0; i < tiles.Count; i++)
            {
                tri.AddRange(tiles[i].get_connection_tris(Util.v3_to_v2(vertices, "y"), original_length));
            }
            Vector2[] uv = new Vector2[vertices.Length];
            for (int i = 0; i < original_length; i++)
            {
                int k = i % 6;
                uv[i] = World.hex_uv[k] * (float)(World.tex_scale);
                switch (k)
                {
                    case 0:
                        uv[i + original_length * 1] = World.tri_uv[1] * (float)World.tex_scale;
                        uv[i + original_length * 3] = World.rect_uv[2] * (float)World.tex_scale;
                        uv[i + original_length * 4] = World.rect_uv[3] * (float)World.tex_scale;
                        break;
                    case 1:
                        uv[i + original_length * 1] = World.tri_uv[2] * (float)World.tex_scale;
                        uv[i + original_length * 2] = World.rect_uv[0] * (float)World.tex_scale;
                        uv[i + original_length * 3] = World.rect_uv[3] * (float)World.tex_scale;
                        break;
                    case 2:
                        uv[i + original_length * 1] = World.tri_uv[2] * (float)World.tex_scale;
                        uv[i + original_length * 2] = World.rect_uv[1] * (float)World.tex_scale;
                        uv[i + original_length * 4] = World.rect_uv[0] * (float)World.tex_scale;
                        break;
                    case 3:
                        uv[i + original_length * 1] = World.tri_uv[0] * (float)World.tex_scale;
                        uv[i + original_length * 3] = World.rect_uv[0] * (float)World.tex_scale;
                        uv[i + original_length * 4] = World.rect_uv[1] * (float)World.tex_scale;
                        break;
                    case 4:
                        uv[i + original_length * 1] = World.tri_uv[0] * (float)World.tex_scale;
                        uv[i + original_length * 2] = World.rect_uv[2] * (float)World.tex_scale;
                        uv[i + original_length * 3] = World.rect_uv[1] * (float)World.tex_scale;
                        break;
                    case 5:
                        uv[i + original_length * 1] = World.tri_uv[1] * (float)World.tex_scale;
                        uv[i + original_length * 2] = World.rect_uv[3] * (float)World.tex_scale;
                        uv[i + original_length * 4] = World.rect_uv[2] * (float)World.tex_scale;
                        break;
                }
            }

            mesh.vertices = vertices;
            mesh.triangles = tri.ToArray();
            mesh.uv = uv;
            mesh.RecalculateNormals();
            mesh.Optimize();
            renderer.material.SetColor("_Color", new Color(.7f, .7f, .7f));
            renderer.material.SetTexture("_MainTex", texture);

            tiles[0].set_unit();
            tiles[0].unit.spawn();

        }

        Vector3[] get_vertices()
        {
            Vector3[] vertices = new Vector3[]{};

            for (int i = 0; i < tiles.Count; i++)
            {
                vertices = vertices.Concat(tiles[i].vertices).ToArray();
            }

            original_length = vertices.Length;
            //Second set of vertices is used for connection triangles
            for (int i = 0; i < tiles.Count; i++)
            {
                vertices = vertices.Concat(tiles[i].vertices).ToArray();
            }
            //Upper left and lower right connection triangles
            for (int i = 0; i < tiles.Count; i++)
            {
                vertices = vertices.Concat(tiles[i].vertices).ToArray();
            }
            //Top and bottom connection rectangles
            for (int i = 0; i < tiles.Count; i++)
            {
                vertices = vertices.Concat(tiles[i].vertices).ToArray();
            }
            //Upper right and lower left connection rectangles
            for (int i = 0; i < tiles.Count; i++)
            {
                vertices = vertices.Concat(tiles[i].vertices).ToArray();
            }

            return vertices;

        }
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                for (int i = 0; i < tiles.Count; i++)
                {
                    if (tiles[i].check_click(Input.mousePosition, Camera.main))
                    {
                        focus.transform.position = tiles[i].position + new Vector3(0, .005f, 0);
                        focus_an.Play("focus_fade", -1, .7f);
                    }
                        
                }
            }

            move_camera();
        }
        private void move_camera()
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                camera_rb.velocity = new Vector3(0, 0, 10);
            }
            else if(Input.GetKey(KeyCode.DownArrow))
            {
                camera_rb.velocity = new Vector3(0, 0, -10);
            }
            else
            {
                camera_rb.velocity = new Vector3(camera_rb.velocity.x, camera_rb.velocity.y, 0); ;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                camera_rb.velocity = new Vector3(-10, 0, 0);
            }
            else if(Input.GetKey(KeyCode.RightArrow))
            {
                camera_rb.velocity = new Vector3(10, 0, 0);
            }
            else
            {
                camera_rb.velocity = new Vector3(0, camera_rb.velocity.y, camera_rb.velocity.z);
            }

        }

        private void add_tiles()
        {
            tiles.Add(new Test(0, 0, 0, tiles.Count));
            tiles.Add(new Test(0, 0, 1, tiles.Count));
            tiles.Add(new Test(0, 1, 2, tiles.Count));
            tiles.Add(new Test(0, 2, 3, tiles.Count));
            tiles.Add(new Test(0, 1, 4, tiles.Count));
            tiles.Add(new Test(0, 0, 5, tiles.Count));
            tiles.Add(new Test(1, 0, 0, tiles.Count));
            tiles.Add(new Test(1, 1, 1, tiles.Count));
            tiles.Add(new Test(1, 2, 2, tiles.Count));
            tiles.Add(new Test(1, 3, 3, tiles.Count));
            tiles.Add(new Test(1, 2, 4, tiles.Count));
            tiles.Add(new Test(2, 0, 0, tiles.Count));
            tiles.Add(new Test(2, 0, 1, tiles.Count));
            tiles.Add(new Test(2, 0, 2, tiles.Count));
            tiles.Add(new Test(2, 0, 3, tiles.Count));
            tiles.Add(new Test(2, 0, 4, tiles.Count));
            tiles.Add(new Test(2, 0, 5, tiles.Count));
            tiles.Add(new Test(3, 0, 0, tiles.Count));
            tiles.Add(new Test(3, 0, 1, tiles.Count));
            tiles.Add(new Test(3, 0, 2, tiles.Count));
            tiles.Add(new Test(3, 0, 3, tiles.Count));
            tiles.Add(new Test(3, 0, 4, tiles.Count));
            tiles.Add(new Test(4, 0, 0, tiles.Count));
            tiles.Add(new Test(4, 0, 1, tiles.Count));
            tiles.Add(new Test(4, 0, 2, tiles.Count));
            tiles.Add(new Test(4, 0, 3, tiles.Count));
            tiles.Add(new Test(4, 0, 4, tiles.Count));
            tiles.Add(new Test(4, 0, 5, tiles.Count));
            
        }
    }
}