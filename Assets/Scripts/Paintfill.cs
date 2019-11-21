using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityStandardAssets.CrossPlatformInput;

public class Paintfill : MonoBehaviour
{

    private Color currentColor = Color.white;
    private Vector3 position; // Mouse click position
    private GameObject Randomize;
    private Button rand;



    void Start()
    {
        // Initialize Random button
        Randomize = GameObject.FindGameObjectWithTag("random");
        rand = Randomize.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = -5;

        if (Input.GetMouseButtonDown(0))
        {
            RayCast2D();
        }



    }

    private void RayCast2D()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(position, Vector2.zero);

        if (hit2D.collider == null)
            return;

        if (hit2D.collider.tag == "color")
        {
            currentColor = hit2D.collider.GetComponent<SpriteRenderer>().color;
            Debug.Log(currentColor);

        }

        // passes the Polygon Collider of clicked image part into CollderToMesh method
        else if (hit2D.collider.tag == "image")
        {
            PolygonCollider2D pc2 = hit2D.collider.GetComponent<PolygonCollider2D>();

            //hit2D.collider.GetComponent<MeshRenderer>().material.color = currentColor;

            ColliderToMesh(pc2);

            hit2D.collider.GetComponent<MeshRenderer>().material.color = currentColor;

        }


        rand.onClick.AddListener(RandomizeColor);

    }

    private void RandomizeColor()
    {
        // this needs to be changed probably a FindObjectsOfType????

        currentColor = Random.ColorHSV();
        Debug.Log(currentColor);
        GetComponent<MeshRenderer>().material.color = currentColor;
    }

    private void ColliderToMesh(PolygonCollider2D pc2)
    {
        //Render thing
        int pointCount = 0;
        pointCount = pc2.GetTotalPointCount();
        MeshFilter mf = pc2.GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        Vector2[] points = pc2.points;
        Vector3[] vertices = new Vector3[pointCount];
        Vector2[] uv = new Vector2[pointCount];
        for (int j = 0; j < pointCount; j++)
        {
            Vector2 actual = points[j];
            vertices[j] = new Vector3(actual.x, actual.y, 0);
            uv[j] = actual;
        }
        Triangulator tr = new Triangulator(points);
        int[] triangles = tr.Triangulate();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mf.mesh = mesh;
        //Render thing
    }

}