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

        public Mesh mesh { get; set; }
        public Tile parrent_tile { get; set; }
        public GameObject obj { get; set; }
        
        public Unit(){}

        public void spawn()
        {
            MeshFilter mf = obj.AddComponent<MeshFilter>();
            MeshRenderer mr = obj.AddComponent<MeshRenderer>();
            mf.mesh = Resources.LoadAssetAtPath("Assets/meshes/test_unit.blend", typeof(Mesh)) as Mesh;
            mr.material = new Material(Shader.Find("Diffuse"));
        }

        public abstract void move(Tile target);
    }
}
