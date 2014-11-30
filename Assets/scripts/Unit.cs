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

        private void spawn()
        {
            Debug.Log("created new cube");
            obj = new GameObject();
            MeshFilter mf = obj.AddComponent<MeshFilter>();
            MeshRenderer mr = obj.AddComponent<MeshRenderer>();
            mf.mesh = Resources.LoadAssetAtPath("Assets/meshes/" + model_name , typeof(Mesh)) as Mesh;
            mr.material = new Material(Shader.Find("Diffuse"));
        }

        public abstract void move(Tile target);
    }
}
