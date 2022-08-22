using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Heavily inspired by https://www.youtube.com/watch?v=_Ij24zRI9J0&ab_channel=DitzelGames. ~ SK
public class Waves : MonoBehaviour
{
    //Public Properties
    public int dimensions = 10;
    public float UVScale = 2f;
    public Octave[] octaves;

    //Mesh
    protected MeshFilter meshFilter;
    protected Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        //Mesh Setup
        mesh = new Mesh();
        mesh.name = gameObject.name;

        mesh.vertices = GenerateVerts();
        mesh.triangles = GenerateTries();
        mesh.uv = GenerateUVs();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    public float GetHeight(Vector3 position)
    {
        // Scale factor and position in local space. ~ SK
        var scale = new Vector3(1 / transform.lossyScale.x, 0, 1 / transform.lossyScale.z);
        var localPos = Vector3.Scale((position - transform.position), scale);

        // Get edge points. ~ SK
        var p1 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Floor(localPos.z));
        var p2 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Ceil(localPos.z));
        var p3 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Floor(localPos.z));
        var p4 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Ceil(localPos.z));

        // Clamp if the position is outside the plane. ~ SK
        p1.x = Mathf.Clamp(p1.x, 0, dimensions);
        p1.z = Mathf.Clamp(p1.z, 0, dimensions);
        p2.x = Mathf.Clamp(p2.x, 0, dimensions);
        p2.z = Mathf.Clamp(p2.z, 0, dimensions);
        p3.x = Mathf.Clamp(p3.x, 0, dimensions);
        p3.z = Mathf.Clamp(p3.z, 0, dimensions);
        p4.x = Mathf.Clamp(p4.x, 0, dimensions);
        p4.z = Mathf.Clamp(p4.z, 0, dimensions);

        // Get the max distance to one of the edges and take that to compute max - dist. ~ SK
        var max = Mathf.Max(Vector3.Distance(p1, localPos), Vector3.Distance(p2, localPos), Vector3.Distance(p3, localPos), Vector3.Distance(p4, localPos) + Mathf.Epsilon);
        var dist = (max - Vector3.Distance(p1, localPos))
                 + (max - Vector3.Distance(p2, localPos))
                 + (max - Vector3.Distance(p3, localPos))
                 + (max - Vector3.Distance(p4, localPos) + Mathf.Epsilon);
        // Weighted sum. ~ SK
        var height = mesh.vertices[Index(p1.x, p1.z)].y * (max - Vector3.Distance(p1, localPos))
                   + mesh.vertices[Index(p2.x, p2.z)].y * (max - Vector3.Distance(p2, localPos))
                   + mesh.vertices[Index(p3.x, p3.z)].y * (max - Vector3.Distance(p3, localPos))
                   + mesh.vertices[Index(p4.x, p4.z)].y * (max - Vector3.Distance(p4, localPos));

        // Scale. ~ SK
        return height * transform.lossyScale.y / dist;

    }

    private Vector3[] GenerateVerts()
    {
        var verts = new Vector3[(dimensions + 1) * (dimensions + 1)];

        // Equaly distributed verts. ~ SK
        for (int x = 0; x <= dimensions; x++)
        {
            for (int z = 0; z <= dimensions; z++)
            {
                verts[Index(x, z)] = new Vector3(x, 0, z);
            }
        }

        return verts;
    }

    private int[] GenerateTries()
    {
        var tries = new int[mesh.vertices.Length * 6];// 6 because 2 triangles (in a square) have 6 vertices in total. ~ SK 

        for (int x = 0; x < dimensions; x++)
        {
            for (int z = 0; z < dimensions; z++)
            {
                // Verts of first triangle (in a square). ~ SK
                tries[Index(x, z) * 6 + 0] = Index(x, z);
                tries[Index(x, z) * 6 + 1] = Index(x + 1, z + 1);
                tries[Index(x, z) * 6 + 2] = Index(x + 1, z);

                // Verts of second triangle (in a square). ~ SK
                tries[Index(x, z) * 6 + 3] = Index(x, z);
                tries[Index(x, z) * 6 + 4] = Index(x, z + 1);
                tries[Index(x, z) * 6 + 5] = Index(x + 1, z + 1);
            }
        }

        return tries;
    }

    private Vector2[] GenerateUVs()
    {
        var uvs = new Vector2[mesh.vertices.Length];

        // Always set one uv over n tiles then flip the uv and set it again. ~ SK
        for (int x = 0; x <= dimensions; x++)
        {
            for (int z = 0; z <= dimensions; z++)
            {
                var vec = new Vector2((x / UVScale) % 2, (z / UVScale) % 2);
                //"vec.x <= 1 ? vec.x : 2 - vec.x" means "if vec.x == 0 then use vec.x otherwise use 2-vec.x" ~ SK
                uvs[Index(x, z)] = new Vector2(vec.x <= 1 ? vec.x : 2 - vec.x, vec.y <= 1 ? vec.y : 2 - vec.y);
            }
        }

        return uvs;
    }

    private int Index(int x, int z)
    {
        // Examples:
        // x=0, z=0 => 0 ... x=0, z=9 => 9 ... x=1, z=0 => 12 (dimensions+1)+1 ~ SK

        return x * (dimensions + 1) + z;
    }

    private int Index(float x, float z)// For when trying to pass in floats to Index(). ~ SK
    {
        return Index((int)x, (int)z);
    }

    // Update is called once per frame
    void Update()
    {
        var verts = mesh.vertices;// Get vertices and store in verts. ~ SK

        for (int x = 0; x <= dimensions; x++)
        {
            for (int z = 0; z <= dimensions; z++)
            {
                var y = 0f;
                for (int o = 0; o < octaves.Length; o++)
                {
                    if (octaves[o].alternate)
                    {
                        var perl = Mathf.PerlinNoise((x * octaves[o].scale.x) / dimensions, (z * octaves[o].scale.y) / dimensions) * Mathf.PI * 2f;// Uses y (not z) because speed and scale are Vector2. ~ SK
                        y += Mathf.Cos(perl + octaves[o].speed.magnitude * Time.time) * octaves[o].height;
                    }
                    else
                    {
                        var perl = Mathf.PerlinNoise((x * octaves[o].scale.x + Time.time * octaves[o].speed.x) / dimensions, (z * octaves[o].scale.y + Time.time * octaves[o].speed.y) / dimensions) - 0.5f;// -0.5f to lower the base of the waves. ~ SK
                        y += perl * octaves[o].height;
                    }
                }

                verts[Index(x, z)] = new Vector3(x, y, z);
            }
        }
        mesh.vertices = verts;// Put vertices back (after having been manipulated). ~ SK
        mesh.RecalculateNormals();
    }

    [Serializable]
    public struct Octave
    {
        public Vector2 speed;
        public Vector2 scale;
        public float height;
        public bool alternate;
    }
}