using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mushroom : MonoBehaviour
{
    RopeGod dog;
    Animator anim;
    Transform animBody;
    NavMeshAgent agent;

    public float timeToJump = 3f;

    public bool jumped = false;

    float currentJTime = 0f;

    int hitTrig = Animator.StringToHash("hit");
    int randHash = Animator.StringToHash("randomness");
    int jumpTrig = Animator.StringToHash("jump");


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        dog = RopeGod.instance;
    }
    void Update()
    {
        if (currentJTime < timeToJump)
        {
            currentJTime += Time.deltaTime;
            if (currentJTime >= timeToJump)
            {
                anim.SetTrigger(jumpTrig);
                jumped = true;
            }
        }
        else if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            Vector3 fromDogToThis = transform.position - dog.position;
            agent.SetDestination(transform.position + fromDogToThis);
        }
    }
}
