using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{

    AudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    
    public void FootStep()
    {
        Debug.Log("footstep");
        audioManager.Play("footstep");
    }
}
