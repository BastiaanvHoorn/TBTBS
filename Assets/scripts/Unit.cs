using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.scripts.tile;

namespace Assets.scripts
{
    public abstract class Unit
    {
        public Tile parrent_tile { get; set; }
        public GameObject obj { get; set; }
        public abstract String model_name { get; }
        public abstract string name { get; }
        public Player player { get; set; }
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
        /// <returns>Returns true if succeeded, returns false if failed</returns>
        public virtual bool move(Tile target, Unit_manager unit_manager, bool spawn)
        {
            if (Tile_manager.is_adjecent(this.obj.transform.position, target.position) || spawn)
            {
                if (unit_manager.is_tile_empty(target))
                {
                    this.parrent_tile = target;
                    if (spawn)
                    {
                        this.obj.transform.position = target.position;
                    }
                    return true;
                }
            }
            return false;
        }
        public virtual bool move(Tile target, Unit_manager unit_manager)
        {
            return move(target, unit_manager, false);
        }
        public void move_towards()
        {
            if (this.obj.transform.position.y == parrent_tile.position.y|| Util.v3_to_v2(this.obj.transform.position, "y") == Util.v3_to_v2(parrent_tile.position, "y"))
            {
                this.obj.transform.position = Vector3.MoveTowards(this.obj.transform.position, parrent_tile.position, .1f);
            }
            else if(this.obj.transform.position.y >= parrent_tile.position.y)
            {
                this.obj.transform.position = Vector3.MoveTowards(this.obj.transform.position, new Vector3(parrent_tile.position.x, this.obj.transform.position.y, parrent_tile.position.z), .1f);
            }
            else
            {
                this.obj.transform.position = Vector3.MoveTowards(this.obj.transform.position, new Vector3(this.obj.transform.position.x, parrent_tile.position.y, this.obj.transform.position.z), .1f);
            }
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
            renderer.material.SetColor("_Color", new Color(.12f, .85f, .12f, .5f));
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
