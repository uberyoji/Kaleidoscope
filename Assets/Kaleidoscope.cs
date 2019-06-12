using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kaleidoscope : MonoBehaviour
{
    public int SliceCount = 8;
    public float SliceLength = 1f;

    public float SliceTextureAngle = 45f;
    
    // Start is called before the first frame update
    void Start()
    {
        SliceTextureAngle = 2 * Mathf.PI / SliceCount;

        GetComponent<MeshFilter>().sharedMesh = GenerateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Mesh GenerateMesh()
    {
        int VertexCount = SliceCount * 2 + 1;  // vertex per teeth
        Vector3[] V = new Vector3[VertexCount];
        Vector2[] T = new Vector2[VertexCount];
        int[] I = new int[3 * SliceCount];


        V[0] = new Vector3(0f, 0f, 0f);
        T[0] = new Vector3(0f, 0f);

        float a = 0f;
        float da = 2 * Mathf.PI / SliceCount;

        for(int i=0;i<SliceCount; i++)
        {
            V[i + 1] = new Vector3(Mathf.Cos(a) * SliceLength, Mathf.Sin(a) * SliceLength, 0f);
            T[i + 1] = new Vector2(0f, 1f);

            a += da;

            V[i + 2] = new Vector3(Mathf.Cos(a) * SliceLength, Mathf.Sin(a) * SliceLength, 0f);
            T[i + 2] = new Vector2(Mathf.Cos(SliceTextureAngle), Mathf.Sin(SliceTextureAngle));
        }

        for( int s=0;s<SliceCount;s++)
        {
            I[3*s] = 0;
            I[3*s+1] = 2*s;
            I[3*s+2] = 2*s+1;
        }
        
        Mesh M = new Mesh
        {
            vertices = V,
            uv = T,
            triangles = I
        };
        return M;
    }
}
