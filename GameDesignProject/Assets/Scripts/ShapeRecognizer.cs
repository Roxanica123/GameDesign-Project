using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using RecognizerAlgo = Recognizer.DollarRecognizer;
using Point = Recognizer.Point;

public class ShapeRecognizer : MonoBehaviour
{
    private DrawPath _drawPath;
    private RecognizerAlgo _recognizer;
    [SerializeField] private double tapTreshold = 50;
    [SerializeField] private double accuracyTreshold = 0.7;
    public UnityEvent onFigure = new UnityEvent();

    private string _figureDrawn = "";
    [SerializeField] private TextMeshProUGUI guessText;

    // Start is called before the first frame update
    void Start()
    {
        this._recognizer = new RecognizerAlgo();
        this._drawPath = GameObject.FindWithTag("Playarea").GetComponent<DrawPath>();
    }

    private bool isTap(Vector3[] points)
    {
        double sum = 0;
        Vector3 runningPoint = points.First();
        foreach (Vector3 point in points)
        {
            sum += Vector3.Distance(runningPoint, point);
            runningPoint = point;
            if (sum >= tapTreshold)
                return false;
        }
        return true;
    }

    public void OnShapeDrawn()
    {
        Vector3[] points = this._drawPath.GetPoints();

        if (isTap(points))
        {
            _figureDrawn = "tap";
            guessText.SetText(_figureDrawn);
            onFigure.Invoke();
            return;
        }
            
        var result = _recognizer.Recognize(Vector3ToTimePointF(points));
        guessText.SetText($"{result.Name} - {Math.Round(result.Score * 100)}%");
        if (result.Score >= accuracyTreshold)
        {
            _figureDrawn = result.Name;
            onFigure.Invoke();
        }
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

    public string Shape => _figureDrawn;
}