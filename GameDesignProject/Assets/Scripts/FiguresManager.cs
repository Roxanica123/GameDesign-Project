using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using Unity.Notifications.iOS;
using UnityEngine;

public class FiguresManager : MonoBehaviour
{
    private Transform notesTransform;
    private GameObject notePrefab;
    private List<Note> notesList;
    static Vector3 spawnPoint;
    static Vector3 endPoint;
    private float timer = 0;
    private float lastSpawnTime = 0;
    static BoxCollider2D greenCollider;

    private class Note
    {
        private float timeOfNote;
        private float duration = 2;
        public GameObject reference;

        public Note(float time, GameObject reference)
        {
            timeOfNote = time;
            this.reference = reference;
        }

        private float GetY(float currTime)
        {
            return ((timeOfNote - currTime) / duration) * (spawnPoint.y - endPoint.y);
        }

        public void UpdateY(float currTime)
        { 
            Vector3 currPos = reference.transform.position;
            Vector3 newPos = new Vector3(currPos.x, GetY(currTime), currPos.z);
            reference.transform.position = newPos;
            // Debug.Log(newPos);
        }

        public bool HasPassedEndpoint()
        {
            return this.reference.transform.position.y < endPoint.y;
        }

        public bool IsInFrontOfScoreZone()
        {
            return greenCollider.OverlapPoint(this.reference.transform.position);
        }
    }
    
    

    void Start()
    {
        spawnPoint = new Vector3(0, 1000, -5);
        endPoint = new Vector3(0, -870, -5);
        this.notesTransform = GameObject.Find("Notes").transform;
        this.notePrefab = Resources.Load<GameObject>("Prefabs/Dummy");
        this.notesList = new List<Note>();
        greenCollider = GameObject.Find("GreenZone").GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        this.timer += Time.deltaTime;
        foreach (Note note in notesList)
        {
            note.UpdateY(timer);
            if (note.IsInFrontOfScoreZone())
            {
                Debug.Log("Verde");
            }
            // if (note.HasPassedEndpoint())
            // {
            //     Destroy(note.reference);
            //     notesList.Remove(note);
            // }
        }

        if (timer - lastSpawnTime >= 2)
        {
            // Debug.Log("Aci!");
            SpawnNote(timer + 2);
            lastSpawnTime = timer;
        }
    }

    private GameObject SpawnNote(float time)
    {
        var newNote = Instantiate(this.notePrefab, new Vector3(0, 1500, -5), Quaternion.identity);
        newNote.transform.SetParent(this.notesTransform);
        notesList.Add(new Note(time, newNote));
        return newNote;
    }
}
