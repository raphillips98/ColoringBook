using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityStandardAssets.CrossPlatformInput;

public class Paintfill : MonoBehaviour
{

    private Color currentColor = Color.white;
    private Vector3 position; // Mouse click position
    private GameObject button;
    private Button rand;
    private Button reset;
    private GameObject colorSelected;



    void Start()
    {
        // Initialize Random button
        button = GameObject.FindGameObjectWithTag("random");
        rand = button.GetComponent<Button>();
        button = GameObject.FindGameObjectWithTag("reset");
        reset = button.GetComponent<Button>();

        colorSelected = GameObject.Find("CurrentColor");
    }

    // Update is called once per frame
    void Update()
    {
        position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = -5;

        if (Input.GetMouseButtonDown(0))
        {
            RayCast2D();
            rand.onClick.AddListener(RandomizeColor);
            reset.onClick.AddListener(ResetColor);
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
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

            // changes the current color circle in the menu to the color that is selected
            colorSelected.GetComponent<SpriteRenderer>().color = currentColor;
            

        }

        // passes the Polygon Collider of clicked image part into CollderToMesh method
        else if (hit2D.collider.tag == "image")
        {
            PolygonCollider2D pc2 = hit2D.collider.GetComponent<PolygonCollider2D>();

            //hit2D.collider.GetComponent<MeshRenderer>().material.color = currentColor;

            ColliderToMesh(pc2);
            //Changes color of image part Mesh to the currentColor selected
            hit2D.collider.GetComponent<MeshRenderer>().material.color = currentColor;

        }

        else if (hit2D.collider.tag == "spots")
        {
            PolygonCollider2D pc2 = hit2D.collider.GetComponent<PolygonCollider2D>();

            //hit2D.collider.GetComponent<MeshRenderer>().material.color = currentColor;

            // fills the spots with the current color selected
            FillSpots(currentColor);


        }




    }

    // Creates array of all objects with the tag spots and fills them in with the current color
    private void FillSpots(Color spotColor)
    {
        
        GameObject[] spots;
        PolygonCollider2D pc;

        spots = GameObject.FindGameObjectsWithTag("spots");

        for(int i = 0; i < spots.Length; i++)
        {
            if (spots[i].GetComponent<Mesh>() == null)
            {
                pc = spots[i].GetComponent<PolygonCollider2D>();

                // creates a mesh renderer in the gameobject passed in
                ColliderToMesh(pc);
            }

            spots[i].GetComponent<MeshRenderer>().material.color = spotColor;
            
        }
    }

    private void RandomizeColor()
    {
        // get a one time random color to fill all of the spots as the same color
        Color spotsRandomColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        Color randomColor = Color.white;
        GameObject[] imageParts;
        GameObject[] spots;
        PolygonCollider2D pc;

        imageParts = GameObject.FindGameObjectsWithTag("image");
        spots = GameObject.FindGameObjectsWithTag("spots");

        for(int i = 0; i <  imageParts.Length; i++)
        {
            if (imageParts[i].GetComponent<Mesh>() == null)
            {
                // Gets collider from the ImagePart object [i] from the array
                pc = imageParts[i].GetComponent<PolygonCollider2D>();
                // passes to colliderToMesh to create a mesh renderer in order to color it in
                ColliderToMesh(pc);
            }
            
            
            randomColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

            imageParts[i].GetComponent<MeshRenderer>().material.color = randomColor;
            

            

        }

        // randomize spots array
        for(int i = 0; i < spots.Length; i++)
        {
            if(spots[i].GetComponent<Mesh>() == null)
            {
                pc = spots[i].GetComponent<PolygonCollider2D>();
                ColliderToMesh(pc);
            }

            spots[i].GetComponent<MeshRenderer>().material.color = spotsRandomColor;
        }

        
    }

    private void ResetColor()
    {   
        GameObject[] imageParts;
        GameObject[] spots;
        PolygonCollider2D pc;

        imageParts = GameObject.FindGameObjectsWithTag("image");
        spots = GameObject.FindGameObjectsWithTag("spots");

        //color all parts of turtle as white
        for (int i = 0; i < imageParts.Length; i++)
        {
            if (imageParts[i].GetComponent<Mesh>() == null)
            {
                // Gets collider from the ImagePart object [i] from the array
                pc = imageParts[i].GetComponent<PolygonCollider2D>();
                // passes to colliderToMesh to create a mesh renderer in order to color it in
                ColliderToMesh(pc);
            }

            imageParts[i].GetComponent<MeshRenderer>().material.color = Color.white;

        }

        // fill in the spots as white
        for (int i = 0; i < spots.Length; i++)
        {
            if (spots[i].GetComponent<Mesh>() == null)
            {
                pc = spots[i].GetComponent<PolygonCollider2D>();
                ColliderToMesh(pc);
            }

            spots[i].GetComponent<MeshRenderer>().material.color = Color.white;
        }
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