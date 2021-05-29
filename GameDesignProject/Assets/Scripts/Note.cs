using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


public class Note
{
    private float _timeOfNote;
    private int _direction = 1;
    private float _error = (float) 0.05;
    public float Duration { get; private set; }
    private readonly GameObject _reference;
    private Vector3 SpawnPoint { get; }
    private Vector3 EndPoint { get; }
    private Queue<float> _movingTimestamps = new Queue<float>();
    public string Type { get; private set; }
    public bool Hit { get; set; }
    public float Speed { get; private set; }

    public Note(float time, GameObject notePrefab, Transform notesTransform, Vector3 spawnPoint, Vector3 endPoint,
        string type, float duration, float speed)
    {
        var newNote = Object.Instantiate(notePrefab, new Vector3(spawnPoint.x, 1500, -5), Quaternion.identity);
        newNote.transform.SetParent(notesTransform);
        _timeOfNote = time;
        _reference = newNote;
        SpawnPoint = spawnPoint;
        EndPoint = endPoint;
        Duration = duration;
        Type = type;
        Speed = speed;
        Hit = false;
    }

    private float GetY(float currTime)
    {
        return ((_timeOfNote - currTime) / (Speed + (float) 0.25 * Duration)) * (SpawnPoint.y - EndPoint.y);
    }

    public void UpdateY(float currTime)
    {
        Vector3 currPos = _reference.transform.position;

        if (this._movingTimestamps.Count > 0 && Math.Abs(currTime - this._movingTimestamps.Peek()) <= this._error)
        {
            currPos.x += 30 * _direction;
            _direction *= -1;
            this._movingTimestamps.Dequeue();
        }
        else
        {
            if (this._movingTimestamps.Count > 0 && this._movingTimestamps.Peek() < currTime)
            {
                this._movingTimestamps.Dequeue();
            }
        }

        Vector3 newPos = new Vector3(currPos.x, GetY(currTime), currPos.z);
        _reference.transform.position = newPos;
    }

    public bool HasPassedEndpoint()
    {
        return this._reference.transform.position.y < EndPoint.y;
    }

    public Vector3 GetPosition()
    {
        return this._reference.transform.position;
    }

    public void SetTimeOfNote(float time, Queue<float> movingTimestamps)
    {
        _movingTimestamps = movingTimestamps;
        _timeOfNote = time;
    }

    public GameObject GameObject => _reference;
}