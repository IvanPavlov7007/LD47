using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootScript : MonoBehaviour
{

    public Rigidbody dog;
    public float initMaxDint, maxDist, anglesToRopeLength;

    void Start()
    {
        maxDist = initMaxDint;
        dist = dog.position - transform.position;
    }
    Vector3 dist;

    bool clockDirectionLeft = true;

    void FixedUpdate()
    {
        Vector3 newDist = dog.position- transform.position;

        float dif = Vector3.SignedAngle(dist, newDist, transform.up) * anglesToRopeLength;
        dif *= clockDirectionLeft? 1f : -1f;
        maxDist = Mathf.Max(1f, maxDist + dif);
        if(maxDist > initMaxDint)
        {
            maxDist = initMaxDint;
            clockDirectionLeft = !clockDirectionLeft;
        }

        dist = newDist;

        if (dist.magnitude > maxDist)
        {
            dog.position = transform.position + dist.normalized * maxDist;
            Vector3 dogForward = dog.transform.forward;
            if(Vector3.Dot(dogForward,dist) > 0.1f)
                dog.rotation *= Quaternion.AngleAxis(Vector3.SignedAngle(dogForward, Vector3.Cross(dist, Vector3.Cross(dogForward,dist)), transform.up),Vector3.up);
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, dog.position);
    }
}
