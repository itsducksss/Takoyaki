using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyMotion : MonoBehaviour
{
    public Transform targetLimb;
    public bool mirror;
    //public bool inverse;
    ConfigurableJoint cj;
    Quaternion startRot;

    void Start()
    {
        cj = GetComponent<ConfigurableJoint>();
        startRot  =transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!inverse)
        //{ 
        //    cj.targetRotation = targetLimb.localRotation * startRot;
        //} 
        //else
        //{ 
        //    cj.targetRotation = Quaternion.Inverse(targetLimb.localRotation) * startRot; 
        //}

        if (!mirror)
        {
            cj.targetRotation = targetLimb.rotation;
            //cj.target.Transform = targetLimb.Transform;
        }
        else
        {
            cj.targetRotation = Quaternion.Inverse(targetLimb.rotation);
        }
    }
}
