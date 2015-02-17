using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.scripts.client.tile;

namespace Assets.scripts.client
{
    public abstract class Unit
    {
        public Tile parrent_tile { get; set; }
        public GameObject obj { get; set; }
        public abstract String model_name { get; }
        public abstract string name { get; }
        public Player player { get; set; }
        public bool can_move { get; set; }
        public Unit()
        {
        }

        //Create the actual gameobject in the scene
        public void spawn()
        {
            obj = new GameObject("soldiers");

            add_unit(new Vector3(-1, 0, -1)).transform.parent = obj.transform;
            add_unit(new Vector3(.1f, 0, -.7f)).transform.parent = obj.transform;
            add_unit(new Vector3(.7f, 0, -1.5f)).transform.parent = obj.transform;
            add_unit(new Vector3(.2f, 0, .1f)).transform.parent = obj.transform;
            add_unit(new Vector3(1.4f, 0, -.1f)).transform.parent = obj.transform;
            add_unit(new Vector3(.8f, 0, .9f)).transform.parent = obj.transform;
            add_unit(new Vector3(-.4f, 0, 1.1f)).transform.parent = obj.transform;
            add_unit(new Vector3(-.9f, 0, .4f)).transform.parent = obj.transform;
        }

        private GameObject add_unit(Vector3 position)
        {
            GameObject unit = new GameObject("soldier");
            unit.AddComponent<MeshFilter>().mesh = Resources.LoadAssetAtPath<Mesh>("Assets/meshes/test_unit.3ds");
            Material material = unit.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Diffuse"));
            if(this.player == Player.Blue)
            {
                material.color = reference.Player_color.blue;
            }
            else
            {
                material.color = reference.Player_color.red;
            }
            unit.transform.position = position;
            return unit;
        }
        public virtual string to_string()
        {
            return this.GetType().ToString() + " on " + this.obj.transform.position.ToString();
        }
        /// <summary>
        /// Moves this unit to the target tile if possible
        /// </summary>
        /// <param name="target">The tile that the unit will try to move to</param>
        /// <param name="unit_manager">The unit_manager that contains all the units</param>
        /// <param name="spawn">If this move command spawns the unit. This will bypass the check if the target is actually in range</param>
        /// <returns>Returns true if succeeded, returns false if failed</returns>
        public virtual bool move(Tile target, Unit_manager unit_manager, bool spawn = false)
        {
            if (Tile_manager.is_adjecent(this.obj.transform.position, target.position) || spawn)
            {
                bool is_empty = unit_manager.is_tile_empty(target);
                if (is_empty)
                {
                    if (can_move || spawn)
                    {
                        if(!spawn)
                        {
                            can_move = false;
                        }
                        else
                        {
                            this.obj.transform.position = target.position;
                        }
                        this.parrent_tile = target;
                        return true; 
                    }
                }
            }
            return false;
        }

        public void move_towards()
        {
            Vector3 parrent_pos = parrent_tile.position;
            Vector3 this_pos = this.obj.transform.position;
            if (this_pos.y == parrent_pos.y || Util.v3_to_v2(this_pos, "y") == Util.v3_to_v2(parrent_pos, "y"))
            {
                this_pos = Vector3.MoveTowards(this_pos, parrent_pos, .1f);
            }
            else if(this_pos.y >= parrent_pos.y)
            {
                this_pos = Vector3.MoveTowards(this_pos, new Vector3(parrent_pos.x, this_pos.y, parrent_pos.z), .1f);
            }
            else
            {
                this_pos = Vector3.MoveTowards(this_pos, new Vector3(this_pos.x, parrent_pos.y, this_pos.z), .1f);
            }
            this.obj.transform.position = this_pos;
        }
        public GameObject show_range(ref Tile_manager world)
        {
            Vector3 grid_pos = parrent_tile.get_grid_pos3();
            Tile_manager range = tiles_in_range(ref world);
            range.add<Grassland>(grid_pos);


            Vector3[] vertices = range.get_vertices();
            List<int> tri = range.get_tri(vertices);
            Vector2[] uv = range.get_uv();

            GameObject obj = new GameObject();
            Mesh mesh = obj.AddComponent<MeshFilter>().mesh;
            Renderer renderer = obj.AddComponent<MeshRenderer>();

            mesh.vertices = vertices;
            mesh.triangles = tri.ToArray();
            mesh.uv = uv;
            mesh.RecalculateNormals();
            mesh.Optimize();
            renderer.material = new Material(Shader.Find("Transparent/Diffuse"));
            if (can_move)
            {
                renderer.material.SetColor("_Color", new Color(.12f, .85f, .12f, .5f));
            }
            else
            {
                renderer.material.SetColor("_Color", new Color(.85f, .08f, .08f, .5f));
            }
            obj.transform.position += new Vector3(0, .01f, 0);

            return obj;
        }

        private Tile_manager tiles_in_range(ref Tile_manager world)
        {
            Tile_manager tiles = world.get_adjecent_tiles(parrent_tile);
            return tiles;
        }
    }
}
