﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float maxVelocity, acceleration;
    public Rigidbody rb;

    RopeGod dog;

    NavMeshAgent agent;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        dog = RopeGod.instance;
    }

    private void Update()
    {
        agent.SetDestination(RopeGod.instance.position);
    }

    void FixedUpdate()
    {
        Vector3 dist = dog.rb.position - rb.position;
        Vector3 velosityProj = dist.normalized * Vector3.Dot(dist, rb.velocity) * rb.velocity.magnitude / dist.magnitude;

        //if (velosityProj.magnitude < maxVelocity)
            //rb.AddForce((dist).normalized * acceleration * rb.mass);
    }
}
