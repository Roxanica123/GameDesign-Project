using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DrawPath : MonoBehaviour
{
    public UnityEvent onShapeDrawn = new UnityEvent();
    private Vector3[] _latestPath = null;

    [SerializeField] private int noTouches = 5;
    [SerializeField] private bool mouseMode;
    private List<Vector3>[] _paths;
    private GameObject[] _particles;
    private ParticleSystem[] _particleSystems;
    private Camera _camera;

    private bool _isDrawing = false;

    public Vector3[] GetPoints()
    {
        return _latestPath;
    }

    private void DrawTrail(int index)
    {
        Vector3 point = _camera.ScreenToWorldPoint(
            !mouseMode ? (Vector2) Input.GetTouch(index).position : (Vector2) Input.mousePosition
        );
        _paths[index].Add(point);
        _particles[index].transform.position = point;
        _particleSystems[index].time = 0;
    }

    private void EndTrail(int index)
    {
        this._latestPath = _paths[index].ToArray();
        _paths[index].Clear();
        onShapeDrawn.Invoke();
    }


    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        GameObject masterParticle = GameObject.FindWithTag("Trail");

        List<List<Vector3>> pathsList = new List<List<Vector3>>();
        List<GameObject> particlesList = new List<GameObject>();
        List<ParticleSystem> particleSystemsList = new List<ParticleSystem>();

        if (mouseMode)
            noTouches = 1;

        for (int i = 0; i < noTouches; i++)
        {
            pathsList.Add(new List<Vector3>());

            GameObject particle = Instantiate(masterParticle);
            particlesList.Add(particle);
            particleSystemsList.Add(particle.GetComponent<ParticleSystem>());
        }

        _paths = pathsList.ToArray();
        _particles = particlesList.ToArray();
        _particleSystems = particleSystemsList.ToArray();
        masterParticle.SetActive(false);
        Debug.Log("[DrawManager] Loaded! Using " + (mouseMode ? "mouse" : "touches"));
    }

    // Update is called once per frame
    void Update()
    {
        if (mouseMode)
            UpdateMouse();
        else
            UpdateTouch();
    }


    void UpdateTouch()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            switch (Input.GetTouch(i).phase)
            {
                case TouchPhase.Began:
                    _particleSystems[i].Pause();
                    DrawTrail(i);
                    _particleSystems[i].Play();
                    break;
                case TouchPhase.Moved:
                    DrawTrail(i);
                    break;
                case TouchPhase.Ended:
                    EndTrail(i);
                    break;
                case TouchPhase.Canceled:
                    EndTrail(i);
                    break;
            }
        }
    }

    IEnumerator DrawMouseTrail()
    {
        _particleSystems[0].Stop();
        DrawTrail(0);
        _particleSystems[0].Play();
        _isDrawing = true;
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        _isDrawing = false;
        EndTrail(0);
    }

    void UpdateMouse()
    {
        if (Input.GetMouseButtonDown(0))
            StartCoroutine(DrawMouseTrail());
        else if (_isDrawing)
            DrawTrail(0);
            
    }
}