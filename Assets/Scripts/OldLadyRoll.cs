using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldLadyRoll : MonoBehaviour
{
    private bool _isRolling = false;
    public float Speed = 5;
    public float RotateSpeed = 0.5f;
    

    // Update is called once per frame
    void Update()
    {
        if (_isRolling)
        {
            transform.position += new Vector3(-1 * Time.deltaTime * Speed, 0, 0);
            transform.Rotate(new Vector3(0, 0, -1 * RotateSpeed));
        }
    }

    public void StartRolling()
    {
        _isRolling = true;
    }
}
