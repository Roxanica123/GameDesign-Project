using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        float timeElapsed = 0.0f;
    
        while (timeElapsed < duration)
        {
            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;
            transform.localPosition = new Vector3(x, y, originalPos.z);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    
        transform.localPosition = originalPos;
    }
    
    public IEnumerator Thump(float duration, float magnitude)
    {
        Camera camera = Camera.main;
        float originalSize = camera.orthographicSize;
        float timeElapsed = 0.0f;

        while (timeElapsed < duration)
        {
            float interpolationRatio = timeElapsed / duration;
            float newSize = originalSize + magnitude * interpolationRatio;
            camera.orthographicSize = newSize;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        
        timeElapsed = 0.0f;
        while (timeElapsed < duration)
        {
            float interpolationRatio = 1 - (timeElapsed / duration);
            float newSize = originalSize + magnitude * interpolationRatio;
            camera.orthographicSize = newSize;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        camera.orthographicSize = originalSize;
    }
}
