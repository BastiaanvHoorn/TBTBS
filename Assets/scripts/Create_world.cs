using System.Collections.Generic;
using Assets.scripts.tile;
using UnityEngine;

namespace Assets.scripts
{
    public class Create_world : MonoBehaviour
    {
        Unit_manager blue = new Unit_manager();
        Unit_manager red = new Unit_manager();
        Tile_manager tile_manager = new Tile_manager();
        Input_manager input_manager;
        public GameObject focus;
        public Animator focus_an;
        public Texture texture;
        enum player { Blue, Red};
        private player current_player = player.Blue;
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

            blue.color = Color.blue;
            red.color = Color.red;
            blue.add<unit.Test>(tile_manager[0]);
            red.add<unit.Test>(tile_manager[8]);

            Debug.Log("Loaded world in " + sw.ElapsedMilliseconds + " ms");
            sw.Stop();
            
        }


        void Update()
        {
            if(current_player == player.Blue)
            {
                input_manager.process_input(ref blue, ref tile_manager);
            }
            else
            {
                input_manager.process_input(ref red, ref tile_manager);
            }
            blue.move_units();
            red.move_units();          
        }

        public void switch_player(GameObject button)
        {

            if (current_player == player.Blue)
            {
                current_player = player.Red;
                button.GetComponent<UnityEngine.UI.Image>().color = Color.red;
                Debug.Log("switched to red");
            }
            else
            {
                current_player = player.Blue;
                button.GetComponent<UnityEngine.UI.Image>().color = Color.blue;
                Debug.Log("switched to blue");
            }
        }

        public void end_turn()
        {

        }

        private void add_tiles(ref Tile_manager tile_manager)
        {
            tile_manager.add<Grassland>(0, 0, 0);
            tile_manager.add<Grassland>(0, 0, 1);
            tile_manager.add<Grassland>(0, 1, 2);
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
            tile_manager.add<Grassland>(3, 0, 1);
            tile_manager.add<Grassland>(3, 0, 2);
            tile_manager.add<Grassland>(3, 0, 3);
            tile_manager.add<Grassland>(3, 0, 4);

            tile_manager.add<Grassland>(4, 0, 0);
            tile_manager.add<Grassland>(4, 0, 1);
            tile_manager.add<Stone>(4, 0, 2);
            tile_manager.add<Stone>(4, 0, 3);
            tile_manager.add<Grassland>(4, 0, 4);
            tile_manager.add<Grassland>(4, 0, 5);

            tile_manager.add<Grassland>(5, 0, 0);
            tile_manager.add<Grassland>(5, 0, 1);
            tile_manager.add<Desert>(5, 0, 2);
            tile_manager.add<Grassland>(5, 0, 3);
            tile_manager.add<Grassland>(5, 0, 4);

            tile_manager.add<Grassland>(6, 0, 0);
            tile_manager.add<Grassland>(6, 0, 1);
            tile_manager.add<Grassland>(6, 0, 2);
            tile_manager.add<Grassland>(6, 0, 3);
            tile_manager.add<Grassland>(6, 0, 4);
            tile_manager.add<Grassland>(6, 0, 5);

        }
    }
}