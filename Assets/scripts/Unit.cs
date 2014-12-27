using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Assets.scripts
{
    public abstract class Unit
    {
        public Tile parrent_tile { get; set; }
        public GameObject obj { get; set; }
        public abstract String model_name { get; }
        public abstract string name { get; }

        public Unit()
        {
            spawn();
        }

        //Create the actual gameobject in the scene
        private void spawn()
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
            unit.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Diffuse"));
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
    }
}
