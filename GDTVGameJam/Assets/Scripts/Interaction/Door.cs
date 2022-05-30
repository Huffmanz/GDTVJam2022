using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] SpriteRenderer DoorTop;
    [SerializeField] SpriteRenderer DoorBottom;

    [SerializeField] Sprite LockedTop;
    [SerializeField] Sprite LockedBottom;
    [SerializeField] Sprite UnlockedTop;
    [SerializeField] Sprite UnlockedBottom;

    bool locked;

    void Start()
    {
        DoorTop.sprite = LockedTop;
        DoorBottom.sprite = LockedBottom;
        locked = true;
        UnlockDoor.OnUnlock += Unlock;
    }

    void OnDisable()
    {
        UnlockDoor.OnUnlock -= Unlock;
    }

    void Unlock()
    {       
        locked = false;
        DoorTop.sprite = UnlockedTop;
        DoorBottom.sprite = UnlockedBottom;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!locked)
        {
            LevelManager.instance.NextLevel();
        }
    }

}
