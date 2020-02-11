using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{

    private Mesh mesh;
    // Start is called before the first frame update
    [SerializeField]
    private GameObject eye;
    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Update()
    {


        float fov = 60f;
        Vector3 origin = eye.GetComponent<Transform>().position;
        int rayCount = 50;
        float angle = 30f;
        float angleIncrease = fov / rayCount;
        float viewDistance = 20f;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i < rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit raycastHit;
            bool intersected = Physics.Raycast(origin, AngleToVec3(angle), out raycastHit, viewDistance);

            if (intersected)
            {
                // Hit
                vertex = raycastHit.point;
            }
            else
            {
                // No hit
                vertex = origin + AngleToVec3(angle) * viewDistance;
            }
            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }
            vertexIndex++;
            angle -= angleIncrease;
        }



        // Assign vertices to mesh
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
     }
 

    public Vector3 AngleToVec3(float angle) {
        float angleRad = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

}
