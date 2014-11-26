using UnityEngine;
using System.Collections;

public class MyCube : MonoBehaviour
{
    void Start()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        var mesh = new Mesh();
        mf.mesh = mesh;

        Vector3[] vertices = new Vector3[6];

        vertices[0] = new Vector3(-1, -Mathf.Sqrt(3), 0);
        vertices[1] = new Vector3(1, -Mathf.Sqrt(3), 0);
        vertices[2] = new Vector3(2, 0, 0);
        vertices[3] = new Vector3(1, Mathf.Sqrt(3), 0);
        vertices[4] = new Vector3(-1, Mathf.Sqrt(3), 0);
        vertices[5] = new Vector3(-2, 0, 0);
        mesh.vertices = vertices;

        int[] tri = new int[12];

        tri[0] = 0;
        tri[1] = 2;
        tri[2] = 1;

        tri[3] = 0;
        tri[4] = 2;
        tri[5] = 3;

        tri[3] = 0;
        tri[4] = 3;
        tri[5] = 5;

        tri[3] = 3;
        tri[4] = 5;
        tri[5] = 4;

        mesh.triangles = tri;

        Vector3[] normals = new Vector3[6];

        normals[0] = -Vector3.forward;
        normals[1] = -Vector3.forward;
        normals[2] = -Vector3.forward;
        normals[3] = -Vector3.forward;
        normals[3] = -Vector3.forward;
        normals[3] = -Vector3.forward;

        mesh.normals = normals;

        Vector2[] uv = new Vector2[6];

        uv[0] = new Vector3(0, -1, 0);
        uv[1] = new Vector3(1, -1, 0);
        uv[2] = new Vector3(2, 0, 0);
        uv[3] = new Vector3(1, 1, 0);
        uv[4] = new Vector3(-1, 1, 0);
        uv[5] = new Vector3(-1, 0, 0);

        mesh.uv = uv;
        mesh.Optimize();
        renderer.material.color = Color.gray;
    }
}