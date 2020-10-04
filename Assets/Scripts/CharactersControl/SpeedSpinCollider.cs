using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedSpinCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var enemy = collision.transform.GetComponent<Enemy>();
        if (enemy != null)
        {
            //enemy.rb.velocity = Vector3.zero;
            //Vector3 centerDir = enemy.rb.position - hit.point;
            //Vector3 perp = Vector3.Cross(rayDir, Vector3.up);
            //perp = Vector3.Dot(perp, centerDir) < 0 ? perp * -1f : perp;
            //perp.Normalize();
            //enemy.rb.velocity = Vector3.zero;
            //enemy.rb.position += perp * 2f;
            //enemy.rb.AddForce(perp * 3f * RopeGod.instance.maxVelocity * enemy.rb.mass, ForceMode.Impulse);
            enemy.rb.AddForce((enemy.transform.position - transform.position) * 3f * RopeGod.instance.maxVelocity * enemy.rb.mass, ForceMode.Impulse);
            enemy.Hit();
        }
    }
}
