using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using UnityEngine;


public class Note
{
    private readonly float _timeOfNote;
    private readonly float duration = 2;
    private readonly GameObject _reference;
    private Vector3 SpawnPoint { get; }
    private Vector3 EndPoint { get; }
    public string Type { get; private set; }
    public bool Hit { get; set; }

    public Note(float time, GameObject notePrefab, Transform notesTransform, Vector3 spawnPoint, Vector3 endPoint, string type)
    {
        var newNote = Object.Instantiate(notePrefab, new Vector3(0, 1500, -5), Quaternion.identity);
        newNote.transform.SetParent(notesTransform);
        _timeOfNote = time;
        _reference = newNote;
        SpawnPoint = spawnPoint;
        EndPoint = endPoint; 
        Type = type;
        Hit = false;

    }

    private float GetY(float currTime)
    {
        return ((_timeOfNote - currTime) / duration) * (SpawnPoint.y - EndPoint.y);
    }

    public void UpdateY(float currTime)
    {
        Vector3 currPos = _reference.transform.position;
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

    public GameObject GameObject => _reference;
}