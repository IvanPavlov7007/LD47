using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node parent, child = null;


    Vector3 nPos;
    public Vector3 position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public virtual void SetParent(Node parent)
    {
        this.parent = parent;
    }

    public virtual void SetChild(Node child)
    {
        this.child = child;
    }

    //Vector3 dist;

    //public bool createdNode = false;

    //public void RecalculateMaxDistRecursively()
    //{
    //    if(child != null)
    //    {

    //        child.RecalculateMaxDistRecursively();
    //    }
    //}

    //public virtual void FixedUpdate()
    //{
    //    if (childRB == null || childRB.isKinematic)
    //        return;

        

    //    Vector3 newDist = childRB.position - transform.position;
    //    float dif = Vector3.SignedAngle(dist, newDist, transform.up) * anglesToRopeLength;

    //    RaycastHit hit;
    //    Ray r = new Ray(transform.position, newDist);
    //    if(Physics.Raycast(r,out hit, newDist.magnitude))
    //    {
    //        Node n = hit.transform.GetComponent<Node>();
    //        if (n.tag == "Enemy")
    //        {
    //            Destroy(n.gameObject);
    //            ParticlesManager.Explode(n.transform.position);
    //        }
    //        else if (n != child && !n.isDog)
    //        {
    //            if (n.child != null)
    //            {
    //                n = CreateNewNode(n);
    //            }

    //            n.ConnectWithTarget(child);
    //            n.clockDirectionLeft = dif < 0;

    //            return;
    //        }
    //    }

        
    //    dif *= clockDirectionLeft ? 1f : -1f;
    //    maxDist = Mathf.Max(0.4f, maxDist + dif);
    //    if (maxDist > initMaxDist)
    //    {
    //        maxDist = initMaxDist;
    //        float exitAngle = Vector3.SignedAngle(Vector3.forward, dist, transform.up);

    //        //TODO: Fix sometimes can't get out of tree
    //        if (!isRoot)// ((exitAngle > entranceAngle && !clockDirectionLeft) || (exitAngle <= entranceAngle && clockDirectionLeft))
    //        {
    //            DisconnectFromRope();
    //            if(createdNode)
    //            {
    //                Destroy(rope.gameObject);
    //                Destroy(this);
    //            }
    //            else
    //                rope.gameObject.SetActive(false);
    //            return;
    //        }
    //        clockDirectionLeft = !clockDirectionLeft;
    //    }

    //    dist = newDist;
    //    rope.sagAmplitude = 1f - dist.magnitude / maxDist;

    //    if (dist.magnitude > maxDist)
    //    {
    //        childRB.position = transform.position + dist.normalized * maxDist;
    //        Vector3 childForward = childRB.transform.forward;
    //        if (Vector3.Dot(childForward, dist) > 0.1f)
    //            childRB.rotation *= Quaternion.AngleAxis(Vector3.SignedAngle(childForward, Vector3.Cross(dist, Vector3.Cross(childForward, dist)), transform.up), Vector3.up);
    //    }
    //}
}
