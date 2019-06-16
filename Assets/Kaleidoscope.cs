using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;

public class Kaleidoscope : MonoBehaviour
{
    public int SliceCount = 8;
    public float SliceLength = 1f;

    public float SliceTextureAngle = 45f;
    public float ScrollSpeed = 1f;
    public float RotationSpeed = 1f;

    private MeshRenderer R;

    private int AngleId = 0;
    private int ScrollId = 0;

    public string Url = "";

    public Vector2 TexTiling;
    public Vector2 TexOffset;

    public Material MatDefault;

    IEnumerator DownloadImage(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            R.material = MatDefault;
            R.material.mainTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        R = GetComponent<MeshRenderer>();
        AngleId = Shader.PropertyToID("_Angle");
        ScrollId = Shader.PropertyToID("_Scroll");

        SliceCount = URLParameters.GetSearchParameters().GetInt("slicecount", SliceCount);
        ScrollSpeed = (float)URLParameters.GetSearchParameters().GetDouble("scrollspeed", ScrollSpeed);
        RotationSpeed = (float)URLParameters.GetSearchParameters().GetDouble("rotationspeed", RotationSpeed);
        TexTiling.x = (float)URLParameters.GetSearchParameters().GetDouble("tilingx", TexTiling.x);
        TexTiling.y = (float)URLParameters.GetSearchParameters().GetDouble("tilingy", TexTiling.y);
        TexOffset.x = (float)URLParameters.GetSearchParameters().GetDouble("offsetx", TexOffset.x);
        TexOffset.y = (float)URLParameters.GetSearchParameters().GetDouble("offsety", TexOffset.y);

        if (Url != "" || URLParameters.GetSearchParameters().TryGetValue("url",out Url) )
        {
            StartCoroutine(DownloadImage(UnityWebRequest.UnEscapeURL(Url)));
        }
        else
        {
            R.material = MatDefault;
        }

        if (SliceCount % 2 != 0)
            ++SliceCount;       // need even slice count for uvs to match.

        SliceTextureAngle = 2 * Mathf.PI / SliceCount;

        GetComponent<MeshFilter>().sharedMesh = GenerateMesh();

        MatDefault.SetTextureOffset("_MainTex", TexOffset);
        MatDefault.SetTextureScale("_MainTex", TexTiling);
    }

    // Update is called once per frame
    void Update()
    {
        R.material.SetFloat(AngleId, Time.time * RotationSpeed);
        R.material.SetFloat(ScrollId, Time.time * ScrollSpeed);
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

        Vector2[] TC = new Vector2[2] { new Vector2(0f, 1f), new Vector2(Mathf.Sin(SliceTextureAngle), Mathf.Cos(SliceTextureAngle))  };

        for(int i=0;i<SliceCount; i++)
        {
            V[2*i + 1] = new Vector3(Mathf.Sin(a) * SliceLength, Mathf.Cos(a) * SliceLength, 0f);
            T[2*i + 1] = TC[i%2];

            a += da;

            V[2*i + 2] = new Vector3(Mathf.Sin(a) * SliceLength, Mathf.Cos(a) * SliceLength, 0f);
            T[2*i + 2] = TC[(i+1)%2];
        }

        for( int s=0;s<SliceCount;s++)
        {
            I[3*s] = 0;
            I[3*s+1] = 2*s+1;
            I[3*s+2] = 2*s+2;
        }
        
        Mesh M = new Mesh
        {
            vertices = V,
            uv = T,
            triangles = I
        };
        return M;
    }

    /*
    void ProcessUrl()
    {
        int pm = Application.absoluteURL.IndexOf("?");
        if (pm != -1)
        {
            sceneName = Application.absoluteURL.Split("?"[0])[1];
        }
    }
    */
}
