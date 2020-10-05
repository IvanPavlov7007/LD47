using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckEnemyCheck : MonoBehaviour
{
    public static StuckEnemyCheck instance;
    MeshFilter f;
    MeshCollider c;

    private void Awake()
    {
        instance = this;
        f = GetComponent<MeshFilter>();
        c = GetComponent<MeshCollider>();
    }

    public void CheckStuckEnemy(Node newNode, Node lastNode)
    {
        if (newNode.parent == lastNode && lastNode != newNode.parent)
            return;

        transform.position = newNode.position + Vector3.up;

        List<Vector3> verts = new List<Vector3>();
        List<int> triangles = new List<int>();
        Node n = newNode;

        f.mesh.Clear();

        int i = 2;

        verts.Add(n.transform.position);
        verts.Add(n.parent.transform.position);

        n = n.parent;

        

        while (n.parent != lastNode) 
        {
            n = n.parent;
            verts.Add(n.transform.position);
            triangles.Add(0);
            triangles.Add(i - 1);
            triangles.Add(i);
            i++;
        }

        Debug.Log(i);
        f.mesh.vertices = verts.ToArray();
        f.mesh.triangles = triangles.ToArray();

        
        c.sharedMesh = f.mesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        var m = other.GetComponentInParent<Mushroom>();
        if (m != null)
            Destroy(m.gameObject);
    }

}
