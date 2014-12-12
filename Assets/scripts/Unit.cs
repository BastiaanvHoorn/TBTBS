using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
namespace Assets.scripts
{
    public abstract class Unit
    {
        public Tile parrent_tile { get; set; }
        public GameObject obj { get; set; }
        public abstract String model_name { get; }
        
        public Unit()
        {
            spawn();
        }

        //Create the actual gameobject in the scene
        private void spawn()
        {
            Debug.Log("created new cube");
            obj = new GameObject();
            MeshFilter mf = obj.AddComponent<MeshFilter>();
            MeshRenderer mr = obj.AddComponent<MeshRenderer>();
            mf.mesh = Resources.LoadAssetAtPath("Assets/meshes/" + model_name , typeof(Mesh)) as Mesh;
            mr.material = new Material(Shader.Find("Diffuse"));
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
        public virtual bool move(Tile target, Unit_manager unit_manager)
        {
            if (Tile_manager.is_adjecent(this.obj.transform.position, target.position))
            {
                if(unit_manager.is_tile_empty(target))
                {
                    this.parrent_tile = target;
                    this.obj.transform.position = target.position;
                    return true;
                }
            }
            return false;
        }
    }
}
