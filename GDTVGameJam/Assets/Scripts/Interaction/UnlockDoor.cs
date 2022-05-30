using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    [SerializeField] Sprite pressedImage;
    public delegate void Unlock();
    public static event Unlock OnUnlock;

    SpriteRenderer _sr;

    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.TryGetComponent<CharacterController2D>(out CharacterController2D controller))
        {
            if(!controller.player.GhostMode)
            {
                _sr.sprite = pressedImage;
                if(OnUnlock != null)
                {
                    OnUnlock();
                }
            }
        }
    }
}
