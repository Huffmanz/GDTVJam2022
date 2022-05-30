using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostObject : MonoBehaviour
{
    [SerializeField] PlayerController player;
    
    private bool ghostMode;
    private SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!player.GhostMode)
        {
            sr.enabled=false;
        }
        else
        {
            sr.enabled = true;
        }
        
    }
}
