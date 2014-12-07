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
        List<Unit> unit_manager;
        public GameObject focus;
        public Rigidbody camera_rb;
        public Animator focus_an;
        public Texture texture;
        public int selected_unit; //Index of the unit currently selected, if none: -1
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
            unit_manager = new List<Unit>();
            unit_manager.Add( new unit.Test());

            Debug.Log(tile_manager.is_adjecent(1,2));
            Debug.Log(tile_manager.is_adjecent(0,1));
            
        }


        void Update()
        {
            //Check for left-click on all tiles
            if (Input.GetMouseButtonDown(0)|| Input.GetMouseButton(1))
            {
                for (int i = 0; i < tile_manager.count; i++)
                {
                    if (tile_manager.tiles[i].check_click(Input.mousePosition, Camera.main))
                    {
                        if (Input.GetMouseButton(0))
                        {
                            Vector3 tile_pos = tile_manager.tiles[i].position;
                            move_focus(tile_pos);
                            if (unit_manager[0].obj.transform.position == tile_pos)
                            {
                                selected_unit = 0;
                            }
                            else
                            {
                                selected_unit = -1;
                            }
                        }
                        else
                        {
                            if (tile_manager.tiles[i].check_click(Input.mousePosition, Camera.main) && selected_unit != -1)
                            {
                                Vector3 tile_pos = tile_manager.tiles[i].position;
                                move_focus(tile_pos);
                                unit_manager[selected_unit].move(tile_manager.tiles[i]);

                            }
                        }
                    }
                }
            }
        }

        private void move_focus(Vector3 pos)
        {
            focus.transform.position = pos + new Vector3(0, .005f, 0);
            focus_an.Play("focus_fade", -1, .7f);
        }

        private void add_tiles()
        {
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