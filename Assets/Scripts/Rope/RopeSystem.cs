using System.Collections.Generic;
using UnityEngine;
public class RopeSystem : MonoBehaviour
{
    public static RopeSystem instnace = null;

    public float RopeLength;
    public List<Node> nodes;


    private void Awake()
    {
        if (instnace != null)
            Destroy(this);
        else
            instnace = this;
    }
}
