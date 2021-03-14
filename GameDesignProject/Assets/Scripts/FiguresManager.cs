using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.iOS;
using UnityEngine;

public class FiguresManager : MonoBehaviour
{
    private Transform notesTransform;

    private GameObject notePrefab;

    private class Note
    {
        private float timeOfNote;
        private GameObject reference;

        public Note(float time)
        {
            timeOfNote = time;
        }

        public void Update()
        {
            
        }
    }
    
    

    void Start()
    {
        this.notesTransform = GameObject.Find("Notes").transform;
        this.notePrefab = Resources.Load<GameObject>("Prefabs/Dummy");
    }

    private GameObject SpawnNote(Vector3 position)
    {
        var newNote = Instantiate(this.notePrefab, position, Quaternion.identity);
        newNote.transform.SetParent(this.notesTransform);
        return newNote;
    }
}
