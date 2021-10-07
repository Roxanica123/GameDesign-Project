using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Drawing : MonoBehaviour
{
    public UnityEvent onShapeDrawn = new UnityEvent();
    private Vector3[] _latestPath = null;

    private const int NoTouches = 5;
    private List<Vector3>[] _paths;
    private GameObject[] _particles;
    private ParticleSystem[] _particleSystems;
    private Camera _camera;
    
    public Vector3[] GetPoints()
    {
        return _latestPath;
    }

    private void DrawTrail(int index)
    {
        Touch touch = Input.GetTouch(index);
        Vector3 point = _camera.ScreenToWorldPoint(touch.position);
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

        for (int i = 0; i < NoTouches; i++)
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
    }

    // Update is called once per frame
    void Update()
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
}