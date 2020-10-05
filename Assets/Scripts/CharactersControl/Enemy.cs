using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float maxVelocity, acceleration;
    public Rigidbody rb;

    RopeGod dog;
    Animator anim;

    NavMeshAgent agent;

    public int initHealth = 5;

    int health;

    int hitTrig = Animator.StringToHash("hit");
    int randHash = Animator.StringToHash("randomness");

    private void Awake()
    {
        health = initHealth;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        anim.SetFloat(randHash, Random.Range(0f, 30f));
        dog = RopeGod.instance;
        GameManager.instance.mushrooms.Add(this);
    }

    private void OnDestroy()
    {
        GameManager.instance.mushrooms.Remove(this);
    }

    private void Update()
    {
        agent.SetDestination(RopeGod.instance.position);
        agent.speed = maxVelocity;
    }

    public void Hit()
    {
        SoundManager.PlayHit();
        anim.SetTrigger(hitTrig);
        if(--health == 0)
        {
            GameManager.AddPoints(initHealth);
            ParticlesManager.Explode(transform.position);
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
            GameManager.LoseLife();
    }

    void FixedUpdate()
    {
        Vector3 dist = dog.rb.position - rb.position;
        Vector3 velosityProj = dist.normalized * Vector3.Dot(dist, rb.velocity) * rb.velocity.magnitude / dist.magnitude;

        //if (velosityProj.magnitude < maxVelocity)
            //rb.AddForce((dist).normalized * acceleration * rb.mass);
    }
}
