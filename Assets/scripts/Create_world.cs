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
        Tile_manager tile_manager = new Tile_manager();
        public GameObject focus;
        public Rigidbody camera_rb;
        public Animator focus_an;
        public Texture texture;
        void Start()
        {
            Mesh mesh = gameObject.AddComponent<MeshFilter>().mesh;
            

            add_tiles();
            Vector3[] vertices = tile_manager.get_vertices();
            List<int> tri = tile_manager.get_tri(vertices);
            Vector2[] uv = tile_manager.get_uv();
            
            mesh.vertices = vertices;
            mesh.triangles = tri.ToArray();
            mesh.uv = uv;
            mesh.RecalculateNormals();
            mesh.Optimize();
            renderer.material.SetColor("_Color", new Color(.7f, .7f, .7f));
            renderer.material.SetTexture("_MainTex", texture);

            Unit test_unit = new unit.Test();
        }


        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                for (int i = 0; i < tile_manager.count; i++)
                {
                    if (tile_manager.tiles[i].check_click(Input.mousePosition, Camera.main))
                    {
                        focus.transform.position = tile_manager.tiles[i].position + new Vector3(0, .005f, 0);
                        focus_an.Play("focus_fade", -1, .7f);
                    }
                        
                }
            }
        }

        private void add_tiles()
        {
            tile_manager.add_tile<Test>(0, 0, 0);
            tile_manager.add_tile<Test>(0, 0, 0);
            tile_manager.add_tile<Test>(0, 0, 1);
            tile_manager.add_tile<Test>(0, 1, 2);
            tile_manager.add_tile<Test>(0, 2, 3);
            tile_manager.add_tile<Test>(0, 1, 4);
            tile_manager.add_tile<Test>(0, 0, 5);
            tile_manager.add_tile<Test>(1, 0, 0);
            tile_manager.add_tile<Test>(1, 1, 1);
            tile_manager.add_tile<Test>(1, 2, 2);
            tile_manager.add_tile<Test>(1, 3, 3);
            tile_manager.add_tile<Test>(1, 2, 4);
            tile_manager.add_tile<Test>(2, 0, 0);
            tile_manager.add_tile<Test>(2, 0, 1);
            tile_manager.add_tile<Test>(2, 0, 2);
            tile_manager.add_tile<Test>(2, 0, 3);
            tile_manager.add_tile<Test>(2, 0, 4);
            tile_manager.add_tile<Test>(2, 0, 5);
            tile_manager.add_tile<Test>(3, 0, 0);
            tile_manager.add_tile<Test>(3, 0, 1);
            tile_manager.add_tile<Test>(3, 0, 2);
            tile_manager.add_tile<Test>(3, 0, 3);
            tile_manager.add_tile<Test>(3, 0, 4);
            tile_manager.add_tile<Test>(4, 0, 0);
            tile_manager.add_tile<Test>(4, 0, 1);
            tile_manager.add_tile<Test>(4, 0, 2);
            tile_manager.add_tile<Test>(4, 0, 3);
            tile_manager.add_tile<Test>(4, 0, 4);
            tile_manager.add_tile<Test>(4, 0, 5);
            
        }
    }
}