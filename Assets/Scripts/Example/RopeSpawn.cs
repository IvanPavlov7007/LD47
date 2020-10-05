using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSpawn : MonoBehaviour
{
    public GameObject partPrefab, parentObject;
    public Rigidbody boundedBody;
    public float partDistance = 0.21f;
    public int length;
    public bool snapFirst, snapLast, spawn;

    private void Update()
    {
        if(spawn)
        {
            Spawn();
            spawn = false;
        }
    }

    public void Spawn()
    {
        int count = (int)(length / partDistance);

        for(int i = 0; i < count;i++)
        {
            GameObject temp;
            temp = Instantiate(partPrefab, new Vector3(transform.position.x, transform.position.y + partDistance * (i + 1), transform.position.z), Quaternion.identity, parentObject.transform);
            temp.transform.eulerAngles = new Vector3(180, 0, 0);

            temp.name = parentObject.transform.childCount.ToString();

            if (i == 0)
            {
                //Destroy(temp.GetComponent<CharacterJoint>());
                temp.GetComponent<CharacterJoint>().connectedBody = boundedBody;
                if (snapFirst)
                    temp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
            else
                temp.GetComponent<CharacterJoint>().connectedBody = parentObject.transform.Find((parentObject.transform.childCount - 1).ToString()).GetComponent<Rigidbody>();
        }
        if(snapLast)
            parentObject.transform.Find((parentObject.transform.childCount).ToString()).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
}
