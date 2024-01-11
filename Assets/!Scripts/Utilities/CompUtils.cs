using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shir0.Math;

public static class CompUtils
{
    public static Coroutine Pulse<T>(this T pulseObj, float minDelta, float maxDelta, float pulseSpeed = 2f, float duration = -1f) where T : Component
    {
        Transform t = pulseObj.transform;
        IEnumerator pulseCoroutine()
        {
            // pulse for given duration.
            if (duration > 0)
            {
                float elapsedDuration = 0;
                float currentScaleRatio = t.localScale.x;

                while (elapsedDuration < duration)
                {
                    elapsedDuration += Time.deltaTime;
                    
                }
            }
            // run indefinitely or until stopped externally.
            else
            {

            }
            yield return new WaitForEndOfFrame();
        }

        return null;
    }
}
/*
 
totalDuration = duration * pi;

 
 */