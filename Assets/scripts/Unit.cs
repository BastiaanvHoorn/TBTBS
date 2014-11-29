using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
namespace Assets.scripts
{
    public class Unit
    {
        public Mesh mesh { get; set; }
        int height;
        public Unit(int _height)
        {
            _height = height;
        }

        public void spawn()
        {
            GameObject obj = new GameObject();
            MeshFilter mf = obj.AddComponent<MeshFilter>();
            MeshRenderer mr = obj.AddComponent<MeshRenderer>();
            mf.mesh = Resources.LoadAssetAtPath("Assets/meshes/test_unit.blend", typeof(Mesh)) as Mesh;
            mr.material = new Material(Shader.Find("Diffuse"));
            obj.transform.position = new Vector3(0, 1, 0);
        }
    }
}
