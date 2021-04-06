using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTrack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioClip track1 = Resources.Load<AudioClip>("Sound/soundtrack");
        AudioSource audioSourse = gameObject.GetComponent<AudioSource>();
        audioSourse.clip = track1;
        audioSourse.PlayDelayed(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
