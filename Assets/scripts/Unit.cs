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
        public int height { get; set; }
        public Vector2 pos { get; set; }
        public Tile parrent_tile { get; set; }
        
        public Unit(){}

        public void spawn()
        {
            GameObject obj = new GameObject();
            MeshFilter mf = obj.AddComponent<MeshFilter>();
            MeshRenderer mr = obj.AddComponent<MeshRenderer>();
            mf.mesh = Resources.LoadAssetAtPath("Assets/meshes/test_unit.blend", typeof(Mesh)) as Mesh;
            mr.material = new Material(Shader.Find("Diffuse"));
            Debug.Log(pos);
            obj.transform.position = new Vector3(pos.x, height+.5f, pos.y);
        }
    }
}
