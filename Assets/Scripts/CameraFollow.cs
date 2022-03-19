using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject FollowTarget;
    public float Speed = 5;


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newXpos = new Vector3(FollowTarget.transform.position.x, this.transform.position.y, this.transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newXpos, Time.deltaTime * Speed);

    }
}
