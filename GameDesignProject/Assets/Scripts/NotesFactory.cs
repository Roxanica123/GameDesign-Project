using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class NotesFactory
{
    private Transform NotesTransform { get; set; }
    private Dictionary<string, GameObject> prefabs;
    private Vector3 SpawnPoint { get; set; }
    private Vector3 EndPoint { get; set; }
    private int[] Zones = {-1 , 0, 1};

    public NotesFactory()
    {
        NotesTransform = GameObject.Find("Notes").transform;
        prefabs = new Dictionary<string, GameObject>()
        {
            {"circle", Resources.Load<GameObject>("Prefabs/DummyCircle")},
            {"check", Resources.Load<GameObject>("Prefabs/DummyCheck")}
        };
    }

    public Note GetNote(string type, float duration)
    {
        int index = Random.Range(0, Zones.Length);
        SpawnPoint = new Vector3(Zones[index] * 285, Screen.height / 2.0f, -5);
        EndPoint = new Vector3(Zones[index] * 285, -Screen.height / 2.0f, -5);
        return new Note(duration, prefabs[type], NotesTransform, SpawnPoint, EndPoint, type);
    }

    public Note GetRandomNote(float duration)
    {
        // var note = prefabs.ElementAt((int) (Random.Range(0, prefabs.Count)));
        var note = prefabs.ElementAt(1);
        int index = Random.Range(0, Zones.Length);
        SpawnPoint = new Vector3(Zones[index] * 285, Screen.height / 2.0f, -5);
        EndPoint = new Vector3(Zones[index] * 285, -Screen.height / 2.0f, -5);
        return new Note(duration, note.Value, NotesTransform, SpawnPoint, EndPoint, note.Key);
    }
}