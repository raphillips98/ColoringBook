using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawLineSystem : MonoBehaviour
{

 

    private Vector3 position; // Mouse click position
   

    [Header("Main Prefab")]
    public GameObject DrawPrefabs;

    [Header("Settings Color and Size")]
    public Color LineColor;
    public float StrokeWidth = 0.4f;

    [Header("Leave These Empty")]
    public EdgeCollider2D edgeCollider;  
    public GameObject currentLine;
    public LineRenderer lineRenderer;
    public TrailRenderer trailRenderer;
    public List<Vector2> touchPositions;
    private GameObject colorSelected;
    private GameObject button;
    private Button rand;
    private GameObject brushSlider;

    private bool drawWithCollider;


    private void Start()
    {
        colorSelected = GameObject.Find("CurrentColor");
        button = GameObject.FindGameObjectWithTag("random");
        rand = button.GetComponent<Button>();
        brushSlider = GameObject.Find("Slider");
    }

    // Update is called once per frame
    void Update()
    {
        
        position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = -5;
        StrokeWidth = brushSlider.GetComponent<Slider>().value;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
        {
            RayCast2D();
            rand.onClick.AddListener(RandomizeColor);

            
               
               
            drawWithCollider = false;
            createDrawLine();
            
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetMouseButton(0))
        {
            Vector2 temppos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(temppos, touchPositions[touchPositions.Count-1]) > .1f)
            {
                //case here to many
              RayCastDraw(temppos);
            }
        }
    }


    private void createDrawLine()
    {
            currentLine = Instantiate(DrawPrefabs, Vector3.zero, Quaternion.identity);
            lineRenderer = currentLine.GetComponent<LineRenderer>();
            
            touchPositions.Clear();
            UpdateLineColorAndSize();
            touchPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            touchPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            lineRenderer.SetPosition(0, touchPositions[0]);
            lineRenderer.SetPosition(1, touchPositions[1]);

           
    }

   



    private void UpdateLine(Vector2 newpos)
    {
            touchPositions.Add(newpos);
            lineRenderer.positionCount += 1;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, newpos);
    }

    private void UpdateLineColorAndSize()
    {
        lineRenderer.startColor = LineColor; //set new color
        lineRenderer.endColor = LineColor; //set new color
        lineRenderer.widthMultiplier = StrokeWidth; //set new width
    }

    // Check if mouse clicks on the color tags on the Menu to change the draw color
    private void RayCast2D()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(position, Vector2.zero);

        if (hit2D.collider == null)
            return;

        if (hit2D.collider.tag == "color")
        {
            LineColor = hit2D.collider.GetComponent<SpriteRenderer>().color;

            // changes the current color circle in the menu to the color that is selected
            colorSelected.GetComponent<SpriteRenderer>().color = LineColor;
        }

    }

    // Detect if the Mouse position is inside the turtle picture collider
    // if so, then continue drawing the line. This keeps the drawing inside the turtle
    private void RayCastDraw(Vector2 newpos)
    {
        RaycastHit2D hit2D = Physics2D.Raycast(position, Vector2.zero);

        if (hit2D.collider == null)
            return;

        if (hit2D.collider.tag == "turtle")
        {
            UpdateLine(newpos);
        }

    }

    private void RandomizeColor()
    {
        Color RandomColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        LineColor = RandomColor;
        colorSelected.GetComponent<SpriteRenderer>().color = LineColor;
    }
}
