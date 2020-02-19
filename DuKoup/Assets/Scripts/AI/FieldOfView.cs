using UnityEngine;

public class FieldOfView : MonoBehaviour
{

    private Mesh mesh;
    //private Vector3 origin;
    private float fov; 
    private int rayCount = 50;
    /// <summary>
    /// The starting angle for the looking direction
    /// </summary>
    [SerializeField]
    private float angle = 210f;
    /// <summary>
    /// The view distance for the mesh and raycasting 
    /// </summary>
    [SerializeField]
    [Tooltip("How far the vision section extends to")] private float viewDistance = 20f;
    private float defaultViewDistance = 20f;
    /// <summary>
    /// Origin of the field of view, set as default to be the 
    /// position of the game object "eye" we have attached to 
    /// this script
    /// </summary>
    //[SerializeField]
    private Vector3 origin;
    private Vector3 rotationOrientation = Vector3.zero;
    private float angleIncrease;

    /// <summary>
    /// When true the mesh gets drawn. Set in activate and turned off in
    /// deactivate
    /// </summary>
    [SerializeField]
    private bool isActive = true;

    private bool hittingPlayer = false;
    

    [SerializeField]
    private GameObject eye;

    /// <summary>
    /// Initialize a mesh object, the angle increase for drawing the mesh 
    /// later in Update and binds the mesh to the MeshFilter's mesh
    /// </summary>
    private void Start()
    {
        SetFOV(60f);
        SetPosition(eye.GetComponent<Transform>().position);
        angleIncrease = fov / rayCount;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    /// <summary>
    /// Updates the FOV point of origin (position) and draws the FOV mesh only
    /// is the is active tag is true
    /// </summary>
    private void Update()
    {
        hittingPlayer = false;
        if (!this.isActive) { Deactivate(); }
        else { Activate(); }
        SetPosition(eye.GetComponent<Transform>().position);
        SetViewDirection(eye.GetComponent<Transform>().rotation.eulerAngles);
        DrawFOV(origin);
    }

    /// <summary>
    /// Makes a Vector3 from an angle in degrees by only setting x and y and 
    /// leaving z=0 due to the nature of our 2.5D game
    /// </summary>
    /// <param name="angle"></param>
    /// <returns> </returns>
    public static Vector3 AngleToVec3(float angle) {
        float angleRad = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public static float Vec3ToAngle(Vector3 vec3)
    {
        Vector3 direction = vec3.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0)
            angle += 360;
        return angle;
    }

    public void SetFOV(float fovAngle)
    {
        fov = fovAngle; // In deg
    }

    public void SetPosition(Vector3 eyePosition)
    {
        origin = eyePosition;
    }

    public void SetViewDirection(Vector3 viewDirection)
    {
        angle = eye.transform.rotation.eulerAngles.z - fov/2f;
    }

    /// <summary>
    /// Activates the FOV mesh with a default view distance 
    /// </summary>
    public void Activate()
    {
        this.isActive = true;
        this.viewDistance = this.defaultViewDistance;
    }

    /// <summary>
    /// Deactivates mesh 
    /// </summary>
    public void Deactivate()
    {
        this.isActive = false;
        this.viewDistance = 0;

        // Draw the mesh to a point
        SetPosition(eye.GetComponent<Transform>().position);
        SetViewDirection(eye.GetComponent<Transform>().rotation.eulerAngles);
        DrawFOV(origin);
    }

    /// <summary>
    /// Draws a mesh for the field of view of the AI enemy by first creating 
    /// vertices aranged as a slice of a circle specified by an angle. Each 
    /// triangle in the mesh is only drawn until the intersection with a 
    /// collider which is calculated using Raycast.
    /// 
    /// uses values stored in:
    ///     fov, 
    ///     rayCount, 
    ///     angle, 
    ///     angleIncrease 
    ///     and viewDistance
    ///     
    /// </summary>
    private void DrawFOV(Vector3 origin)
    {
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
            Debug.Log("Raycast: ");
            Debug.Log(intersected.ToString());
            if (intersected)
            {
                // Hit
                CheckForPlayerHit(raycastHit.collider.gameObject);
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
                triangles[triangleIndex + 2] = vertexIndex;  // Invert +2 and +1 from line above if camera is in +z

                triangleIndex += 3;
            }
            vertexIndex++;
            angle -= angleIncrease;
        }



        // Assign vertices to mesh
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        angle = 210f;
    }

    private void CheckForPlayerHit(GameObject maybePlayer)
    {
        if (maybePlayer.CompareTag("Player"))
        {
            hittingPlayer = true;
        }
    }

    public bool HittingPlayer()
    {
        return hittingPlayer;
    }

}
