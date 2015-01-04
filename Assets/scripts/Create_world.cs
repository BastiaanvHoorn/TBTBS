using System.Collections.Generic;
using Assets.scripts.tile;
using UnityEngine;

namespace Assets.scripts
{
    public class Create_world : MonoBehaviour
    {
        Unit_manager unit_manager = new Unit_manager();
        Tile_manager tile_manager = new Tile_manager();
        Input_manager input_manager;
        public GameObject focus;
        public Animator focus_an;
        public Texture texture;
        void Start()
        {
            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
            input_manager = new Input_manager(focus, focus_an);
            Mesh mesh = gameObject.AddComponent<MeshFilter>().mesh;
            
            add_tiles(ref tile_manager);

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

            unit_manager.add<unit.Test>(tile_manager[0]);
            unit_manager.add<unit.Test>(tile_manager[8]);

            Debug.Log("Loaded world in " + sw.ElapsedMilliseconds + " ms");
            sw.Stop();
            
        }


        void Update()
        {
            input_manager.process_input(ref unit_manager, ref tile_manager);
            unit_manager.move_units();          
        }



        private void add_tiles(ref Tile_manager tile_manager)
        {
            tile_manager.add<Grassland>(0, 0, 0);
            tile_manager.add<Grassland>(0, 0, 1);
            tile_manager.add<Stone>(0, 1, 2);
            tile_manager.add<Stone>(0, 2, 3);
            tile_manager.add<Stone>(0, 1, 4);
            tile_manager.add<Grassland>(0, 0, 5);
            tile_manager.add<Grassland>(1, 0, 0);
            tile_manager.add<Stone>(1, 1, 1);
            tile_manager.add<Stone>(1, 2, 2);
            tile_manager.add<Stone>(1, 3, 3);
            tile_manager.add<Stone>(1, 2, 4);
            tile_manager.add<Grassland>(2, 0, 0);
            tile_manager.add<Grassland>(2, 0, 1);
            tile_manager.add<Grassland>(2, 0, 2);
            tile_manager.add<Grassland>(2, 0, 3);
            tile_manager.add<Grassland>(2, 0, 4);
            tile_manager.add<Grassland>(2, 0, 5);
            tile_manager.add<Grassland>(3, 0, 0);
            tile_manager.add<Grassland>(3, -1, 1);
            //tile_manager.add<Grassland>(3, 0, 2);
            tile_manager.add<Grassland>(3, 0, 3);
            tile_manager.add<Grassland>(3, 0, 4);
            tile_manager.add<Grassland>(4, 0, 0);
            tile_manager.add<Grassland>(4, 0, 1);
            tile_manager.add<Grassland>(4, 0, 2);
            tile_manager.add<Grassland>(4, 0, 3);
            tile_manager.add<Grassland>(4, 0, 4);
            tile_manager.add<Grassland>(4, 0, 5);
            
        }
    }
}