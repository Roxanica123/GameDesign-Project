using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class DrawPath : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private List<Vector3> _path = new List<Vector3>();
    private Boolean _isDrawing = false;
    private Camera _camera;

    [FormerlySerializedAs("OnShapeDrawn")] 
    public UnityEvent onShapeDrawn = new UnityEvent();

    public Vector3[] GetPoints()
    {
        return this._path.ToArray();
    }


    // Start is called before the first frame update
    void Start()
    {
        this._lineRenderer = transform.GetComponent<LineRenderer>();
        this._camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_isDrawing)
            StartDrawing();
        else if (Input.GetMouseButtonUp(0))
            StopDrawing();

        if (_isDrawing)
            Draw();
    }

    void StartDrawing()
    {
        this._isDrawing = true;
        this._path.Clear();
    }

    void StopDrawing()
    {
        this._isDrawing = false;
        this._lineRenderer.positionCount = 0;
        onShapeDrawn.Invoke();
    }

    void Draw()
    {
        Vector2 point = _camera.ScreenToWorldPoint(Input.mousePosition);
        this._path.Add(transform.TransformPoint(point));
        this._lineRenderer.positionCount = _path.Count;
        this._lineRenderer.SetPositions(this._path.ToArray());
    }
}