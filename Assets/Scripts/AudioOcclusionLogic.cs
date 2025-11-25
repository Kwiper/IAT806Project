using UnityEngine;
using UnityEngine.Audio;

public class AudioOcclusionLogic : MonoBehaviour
{
    RayCastLogic rayCastLogic;

    AudioSource audioSource; // Get audio source component
    AudioLowPassFilter lowPassFilter; // Get low pass filter component

    float maxCutoff;

    float occlusionLevel; // Main occlusion variable

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lowPassFilter = GetComponent<AudioLowPassFilter>();
        rayCastLogic = FindFirstObjectByType<RayCastLogic>();

        maxCutoff = lowPassFilter.cutoffFrequency;
    }

    private void FixedUpdate()
    {
        SetOcclusionLevel();
        SetLowPass();
    }

    void SetOcclusionLevel() // Sets the amount of occlusion based on the amount of rays colliding with an object
    {
        occlusionLevel = 1 - (rayCastLogic.RayCastHitCounter / rayCastLogic.RayCastAmount); // The more rays that are hit, the lower the multiplier for occlusion
        //Debug.Log(rayCastLogic.RayCastHitCounter / rayCastLogic.RayCastAmount);
    }


    void SetLowPass()
    {
        lowPassFilter.cutoffFrequency = maxCutoff * occlusionLevel;
    }
}
