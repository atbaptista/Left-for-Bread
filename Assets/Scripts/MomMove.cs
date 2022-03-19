using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomMove : MonoBehaviour
{
    public Sprite LookRightSprite;
    public Sprite LookLeftSprite;
    public float Speed = 5f;
    private bool _isMoving = false;
    private bool _isMovingBack = false;
    private Vector3 _initialPosition;

    private void Start()
    {
        _initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isMoving)
        {
            this.GetComponent<SpriteRenderer>().sprite = LookRightSprite;
            transform.position += new Vector3(Time.deltaTime * Speed, 0, 0);
            if (transform.position.x >= 18)
            {
                _isMoving = false;
            }
        }
        if (_isMovingBack)
        {
            this.GetComponent<SpriteRenderer>().sprite = LookLeftSprite;
            transform.position -= new Vector3(Time.deltaTime * Speed, 0, 0);
            if (transform.position.x <= _initialPosition.x)
            {
                _isMovingBack = false;
                GameObject.Find("SecretEndingDialogue").GetComponent<DialogueTrigger>().TriggerDialogue();
            }
        }
    }

    public void StartMoving()
    {
        _isMoving = true;
    }

    public void StartMovingBack()
    {
        _isMovingBack = true;
    }
}
