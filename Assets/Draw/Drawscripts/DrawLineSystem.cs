using System.Collections.Generic;
using UnityEngine;

public class DrawLineSystem : MonoBehaviour
{

    public enum drawStyle
    {
        DrawWithCollider,
        DrawWithOutCollider
    }

    public drawStyle DrawStyle;

    [Header("Main Prefap")]
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

    private bool drawWithCollider;




    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
        {
           

            switch (DrawStyle)
            {
                case drawStyle.DrawWithCollider:
                    drawWithCollider = true;
                    createDrawLine();
                    break;
                case drawStyle.DrawWithOutCollider:
                    drawWithCollider = false;
                    createDrawLine();
                    break;
            }
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetMouseButton(0))
        {
            Vector2 temppos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(temppos, touchPositions[touchPositions.Count-1]) > .1f)
            {
                //case here to many
              UpdateLine(temppos);
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

            if(drawWithCollider)
            {
                edgeCollider = currentLine.AddComponent<EdgeCollider2D>();
                edgeCollider.points = touchPositions.ToArray();
             }
           
    }

   



    private void UpdateLine(Vector2 newpos)
    {
            touchPositions.Add(newpos);
            lineRenderer.positionCount += 1;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, newpos);

            if (drawWithCollider)
            edgeCollider.points = touchPositions.ToArray();

    }

    private void UpdateLineColorAndSize()
    {
        lineRenderer.startColor = LineColor; //set new color
        lineRenderer.endColor = LineColor; //set new color
        lineRenderer.widthMultiplier = StrokeWidth; //set new width
    }
}
