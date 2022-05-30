using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BaseController))]
public class Health : MonoBehaviour
{
    [SerializeField] GameObject deathParticles;
    [SerializeField] List<GameObject> splatter = new List<GameObject>();
    public bool isDead;
    private Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Die(Vector2 deathLocation)
    {
        FindObjectOfType<AudioManager>().Play("PlayerDeath");
        isDead = true;
        GameObject.Instantiate(deathParticles, transform.position, Quaternion.identity);
        if(splatter.Count > 0)
        {
            GameObject.Instantiate(splatter[Random.Range(0, splatter.Count-1)], gameObject.transform.position, Quaternion.identity);
        }
        GameObject.Destroy(gameObject);
        if(animator)
        {
            animator.SetBool("Dead", isDead);
        }
    }
    
}
