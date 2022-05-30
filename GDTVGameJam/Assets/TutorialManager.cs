using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject[] popUps;
    private int popUpIndex;
    private PlayerController player;
    bool doorUnlocked = false;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        UnlockDoor.OnUnlock += () => doorUnlocked = true;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < popUps.Length; i++)
        {
            if(i==popUpIndex)
            {
                popUps[i].SetActive(true);
            }
            else
            {
                popUps[i].SetActive(false);
            }
        }
        if(popUpIndex == 0)
        {
            if(player.Input.x != 0 || player.Input.y != 0)
            {
                popUpIndex++;
            }
        }
        else if(popUpIndex == 1)
        {
            if(!player.GhostMode)
            {
                popUpIndex++;
            }
        }
        else if(popUpIndex == 2)
        {
            if(player.StartJump)
            {
                popUpIndex++;
            }
        }
        else if(popUpIndex == 3)
        {
            if(doorUnlocked)
            {
                popUpIndex++;
            }
        }
        else if(popUpIndex == 4)
        {
            if(player.GhostMode)
            {
                popUpIndex++;
                GhostController ghost = FindObjectOfType<GhostController>();
                ghost.hasTimeLimit = false;
            }
        }
        else if(popUpIndex == 5)
        {
            if(!player.GhostMode)
            {
                popUps[popUpIndex].SetActive(false);
            }
        }
    }
}
