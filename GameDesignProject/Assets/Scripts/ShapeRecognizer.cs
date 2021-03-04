using System;
using System.Collections.Generic;
using UnityEngine;
using WobbrockLib;
using RecognizerAlgo = Recognizer.Dollar.Recognizer;
public class ShapeRecognizer : MonoBehaviour
{
    private DrawPath _drawPath;
    private RecognizerAlgo _recognizer = new RecognizerAlgo();
    
    
    // Start is called before the first frame update
    void Start()
    {
        this._drawPath = GameObject.FindWithTag("Playarea").GetComponent<DrawPath>();
        LoadGestures();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnShapeDrawn()
    {
        Vector3[] points = this._drawPath.GetPoints();
        var result = _recognizer.Recognize(Vector3ToTimePointF(points), true);
        Debug.Log($"Shape: {result.Name} with accuracy {result.Score}");
    }

    private List<TimePointF> Vector3ToTimePointF(Vector3[] points)
    {
        List<TimePointF> shape = new List<TimePointF>();
        foreach (Vector3 point in points)
        {
            shape.Add(new TimePointF(point.x, point.y, 0));
        }
        return shape;
    }

    private void LoadGestures()
    {
        Debug.Log(_recognizer.LoadGesture("Gestures/triangle1"));
        Debug.Log(_recognizer.LoadGesture("Gestures/triangle2"));
        Debug.Log(_recognizer.LoadGesture("Gestures/square1"));
        Debug.Log(_recognizer.LoadGesture("Gestures/sqaure2"));
    }
}
