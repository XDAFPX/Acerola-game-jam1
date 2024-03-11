using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform LookTarget;
    public bool faceRight;
    private void FixedUpdate()
    {
        if(faceRight)
            transform.right = (transform.position-LookTarget.position);
        else transform.right = (LookTarget.position-transform.position);
    }
}
