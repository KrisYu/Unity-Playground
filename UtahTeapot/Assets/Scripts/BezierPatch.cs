using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class BezierPatch : MonoBehaviour
{

  

	[Range(1,20)]
	[SerializeField] int divs = 2;

    List<Vector3> points;
    List<int> triangles;

 
    void Awake() => GenerateMesh();


#if UNITY_EDITOR
	void Update() => GenerateMesh();
#endif

 
    public void GenerateMesh()
    {

        var mesh = new Mesh();
        mesh.name = "Teapot";
        

        points = new List<Vector3>();
        triangles = new List<int>();


        int kNumPatches = TeaPotData.kNumPatches;;

        int[][] kPatches = TeaPotData.kPatches;
        Vector3[] kVertices = TeaPotData.kVertices;  

      


        
        for (int k = 0; k < kNumPatches; k++)
        {
            Vector3[] cps = new Vector3[16];

            for (int i = 0; i < 16; i++)
            {
                int index = kPatches[k][i];
                cps[i] = kVertices[index-1];
            }


            for (int i = 0; i <= divs; i++)
            {
                for (int j = 0; j <= divs; j++)
                {
                    float u = (float)i/divs;
                    float v = (float)j/divs;
                    Vector3 point = patch(cps,u,v);

                    // Gizmos.DrawSphere(transform.position, 1);
                    points.Add(point);
                } 
            }
        }


        for (int k = 0; k < kNumPatches; k++)
        {
            for (int i = 0; i < divs; i++)
            {
                int start = k * (divs + 1) * (divs + 1);

                for (int j = 0; j < divs; j++)
                {
                    int a = start + i * (divs+1) + j;
                    int b = a + 1;
                    int c = a + (divs+1) ;
                    int d = c + 1;

                    // triangles.Add( c );
                    // triangles.Add( a );
                    // triangles.Add( b );

                    // triangles.Add( d );
                    // triangles.Add( c );
                    // triangles.Add( b );

                    triangles.Add( b );
                    triangles.Add( a );
                    triangles.Add( c );

                    triangles.Add( b );
                    triangles.Add( c );
                    triangles.Add( d );
                }
            }
            
        }
		
        
        // Apply all of this to the mesh object
		mesh.SetVertices( points );
		mesh.SetTriangles(triangles, 0);
		mesh.RecalculateNormals();

		GetComponent<MeshFilter>().mesh = mesh;


    }

    public Vector3 Bezier(Vector3[] cps, float t)
    {
        Vector3 c0 = cps[0];
        Vector3 c1 = cps[1];
        Vector3 c2 = cps[2];
        Vector3 c3 = cps[3];


        Vector3 point = (1 - t) * (1 - t) * (1 - t) * c0 
            + 3 * (1 - t) * (1 - t) * t * c1 
            + 3 * (1 - t) * t * t * c2 
            + t * t * t * c3;
        
        return point;
    }

    public Vector3 patch(Vector3[] cps, float u, float v)
    {
        Vector3[] pu = new Vector3[4];
        for (int i = 0; i < 4; i++)
        {
            Vector3[] curveP = new Vector3[4];
            curveP[0] = cps[i*4];
            curveP[1] = cps[i*4 + 1];
            curveP[2] = cps[i*4 + 2];
            curveP[3] = cps[i*4 + 3];

            pu[i] = Bezier(curveP, u);
        }
        return Bezier(pu, v);
    }



    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        for (int i = 0; i < points.Count; i++)
        {
            // Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(points[i], 0.05f);
        }

    }

    

}
