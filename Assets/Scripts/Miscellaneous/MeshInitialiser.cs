using UnityEngine;
using System.Collections;

public class MeshInitialiser : MonoBehaviour
{
    public Vector3[] newVertices;
    public Vector2[] newUV;
    public int[] newTriangles;
    void Start() {
        Mesh mesh = new Mesh();


        /*mesh = GetComponent<MeshFilter>().mesh;
        foreach (Vector3 v in mesh.vertices)
        Debug.Log("vert="+v);
        foreach (Vector2 u in mesh.uv)
            Debug.Log("uv=" + u);
        foreach (int t in mesh.triangles)
            Debug.Log("tri=" + t);*/

        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = newTriangles;
    }
}