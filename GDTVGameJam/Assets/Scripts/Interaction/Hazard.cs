using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<BaseController>(out BaseController character))
        {
            if(character.player && !character.player.GhostMode)
            {
                character.player.EnterGhostMode();
                Health health = character.GetComponent<Health>();
                if(health)
                {
                    health.Die(gameObject.transform.position);
                }
            }
        }
    }
}
