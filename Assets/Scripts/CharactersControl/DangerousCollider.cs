using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerousCollider : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
            GameManager.LoseLife();
    }
}
