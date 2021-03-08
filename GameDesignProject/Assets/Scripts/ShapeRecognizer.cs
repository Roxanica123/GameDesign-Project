using System.Collections.Generic;
using UnityEngine;
using RecognizerAlgo = Recognizer.DollarRecognizer;
using Point = Recognizer.Point;
public class ShapeRecognizer : MonoBehaviour
{
    private DrawPath _drawPath;
    private RecognizerAlgo _recognizer = new RecognizerAlgo();
    
    
    // Start is called before the first frame update
    void Start()
    {
        this._drawPath = GameObject.FindWithTag("Playarea").GetComponent<DrawPath>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnShapeDrawn()
    {
        Vector3[] points = this._drawPath.GetPoints();
        var result = _recognizer.Recognize(Vector3ToTimePointF(points));
        Debug.Log($"Shape: {result.Name} with accuracy {result.Score}");
    }

    private List<Point> Vector3ToTimePointF(Vector3[] points)
    {
        List<Point> shape = new List<Point>();
        foreach (Vector3 point in points)
        {
            shape.Add(new Point(point.x, point.y));
        }
        return shape;
    }
}
