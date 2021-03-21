using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using Unity.Notifications.iOS;
using UnityEngine;


public class Note
{
    private readonly float _timeOfNote;
    private readonly float duration = 2;
    private readonly GameObject _reference;

    public Note(float time, GameObject notePrefab, Transform notesTransform)
    {
        var newNote = GameObject.Instantiate(notePrefab, new Vector3(0, 1500, -5), Quaternion.identity);
        newNote.transform.SetParent(notesTransform);
        _timeOfNote = time;
        this._reference = newNote;
    }

    private float GetY(float currTime)
    {
        return ((_timeOfNote - currTime) / duration) * (FiguresManager.SpawnPoint.y - FiguresManager.EndPoint.y);
    }

    public void UpdateY(float currTime)
    {
        Vector3 currPos = _reference.transform.position;
        Vector3 newPos = new Vector3(currPos.x, GetY(currTime), currPos.z);
        _reference.transform.position = newPos;
    }

    public bool HasPassedEndpoint()
    {
        return this._reference.transform.position.y < FiguresManager.EndPoint.y;
    }

    public Vector3 GetPosition()
    {
        return this._reference.transform.position;
    }

    public GameObject GameObject => _reference;
}