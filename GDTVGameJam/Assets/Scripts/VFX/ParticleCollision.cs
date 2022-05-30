using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    ParticleSystem particle;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(particle, other, collisionEvents);

        foreach(ParticleCollisionEvent collision in collisionEvents)
        {
            GameObject.Instantiate(objectToSpawn, collision.intersection, Quaternion.identity);
        } 
    }
}
