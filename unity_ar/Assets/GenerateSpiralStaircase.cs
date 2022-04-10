using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateSpiralStaircase : MonoBehaviour
{

    private class MeshGenData
    {
        public List<int> tris;
        public List<Vector3> vertices;
        public List<Vector3> normals;
        public MeshGenData()
        {
            tris = new List<int>();
            vertices = new List<Vector3>();
            normals = new List<Vector3>();
        }
        public Mesh ToMesh()
        {
            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.normals = normals.ToArray();
            mesh.triangles = tris.ToArray();
            return mesh;
        }
    }

    public float stairInnerRadius = 1;

    public float stairOuterRadius = 2;
    
    [Range(1, 100)]
    public float totalHeight;
    public float stairHeight = 0.2f;

    public int stairsPerRotation = 12;
    private float degreesPerStair;

    public int radialTriangles = 36;

    public int dirMult = 1;

    public GameObject innerCylinderObject;
    public GameObject stairsObject;

    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    public void Start()
    {
        GenerateMesh();
    }

    public void OnValidate()
    {
        GenerateMesh();
    }

    private void GenerateMesh()
    {
        MeshFilter meshFilter = innerCylinderObject.GetComponent<MeshFilter>();
        MeshGenData cylinderData = new MeshGenData();
        GenerateArcZFaceOut(cylinderData, Vector3.zero, radialTriangles, 0, 360.0f, stairInnerRadius, totalHeight);
        GenerateArcXY(cylinderData, new Vector3(0, totalHeight, 0), radialTriangles, 0.0f, 360.0f, 0, stairInnerRadius, Vector3.up);
        meshFilter.mesh = cylinderData.ToMesh();

        meshFilter = stairsObject.GetComponent<MeshFilter>();
        MeshGenData stairsData = new MeshGenData();
        float stairAngle = 360.0f/stairsPerRotation;
        for (int i = 0; i < totalHeight/stairHeight; i++)
        {
            GenerateArcXY(stairsData, new Vector3(0, i * stairHeight, 0), radialTriangles/stairsPerRotation, stairAngle*i*dirMult, stairAngle*dirMult, stairInnerRadius, stairOuterRadius, Vector3.up);
        }
        meshFilter.mesh = stairsData.ToMesh();
    }

    private void GenerateInnerCylinder()
    {
        MeshFilter meshFilter = innerCylinderObject.GetComponent<MeshFilter>();
        Vector3[] vertices = new Vector3[radialTriangles];
        for (int i = 0; i < radialTriangles; i++)
        {
            // Bottom

            // Wall
            // Top
        }
    }

    private void GenerateArcZFaceOut(MeshGenData gen, Vector3 shift, int triangleCount, float startAngle, float angle, float radius, float height)
    {
        int sv = gen.vertices.Count;
        for (int i = 0; i < triangleCount + 1; i++)
        {
            Vector2 offset = DegreeToVector2(startAngle + i * angle/triangleCount);
            offset *= radius;
            gen.vertices.Add(shift + new Vector3(offset.x, 0, offset.y));
            gen.normals.Add(offset.normalized);

            gen.vertices.Add(shift + new Vector3(offset.x, height, offset.y));
            gen.normals.Add(offset.normalized);
        }
        for (int i = sv; i < sv+triangleCount*2; i++)
        {
            gen.tris.Add(i+1);
            gen.tris.Add(i);
            gen.tris.Add(i+2);

            gen.tris.Add(i+1);
            gen.tris.Add(i+2);
            gen.tris.Add(i);
        } 
    }

    private void GenerateArcZFaceSide(MeshGenData gen, int triangleCount, float innerRadius, float outerRadius)
    {

    }
    private void GenerateArcXY(MeshGenData gen, Vector3 shift, int triangleCount, float startAngle, float angle, float innerRadius, float outerRadius, Vector3 normal)
    {
        int sv = gen.vertices.Count;
        if (innerRadius == 0.0f)
        {
            gen.vertices.Add(shift);
            gen.normals.Add(normal);
            for (int i = 0; i < triangleCount + 1; i++)
            {
                Vector2 offset = DegreeToVector2(startAngle + i * angle/triangleCount);
                offset *= outerRadius;
                gen.vertices.Add(shift + new Vector3(offset.x, 0, offset.y));
                gen.normals.Add(normal);
            }
            for (int i = sv; i < sv+triangleCount; i++)
            {
                gen.tris.Add(sv);
                gen.tris.Add(i+2);
                gen.tris.Add(i+1);
            }
        } else
        {
            for (int i = 0; i < triangleCount + 1; i++)
            {
                Vector2 offset = DegreeToVector2(startAngle + i * angle/triangleCount);
                Vector2 innerOffset = offset * innerRadius;
                gen.vertices.Add(shift + new Vector3(innerOffset.x, 0, innerOffset.y));
                gen.normals.Add(normal);
            }
            for (int i = 0; i < triangleCount + 1; i++)
            {
                Vector2 offset = DegreeToVector2(startAngle + i * angle/triangleCount);
                Vector2 outerOffset = offset * outerRadius;
                gen.vertices.Add(shift + new Vector3(outerOffset.x, 0, outerOffset.y));
                gen.normals.Add(normal);
            }
            for (int i = sv; i < sv+triangleCount; i++)
            {
                gen.tris.Add(i);
                gen.tris.Add(triangleCount+i+2);
                gen.tris.Add(triangleCount+i+1);
                
                gen.tris.Add(i);
                gen.tris.Add(i+1);
                gen.tris.Add(triangleCount+i+2);
            } 
        }
    }
}
