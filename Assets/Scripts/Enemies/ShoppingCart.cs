using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingCart : MonoBehaviour, EnemyHealth
{
    private enum States {
        DIE, TOEND, TOSTART
    }

    [Header("Movement")]
    public Transform StartPos;
    public Transform EndPos;
    public float Speed;
    public float DistOffset;

    [Header("Health")]
    [SerializeField] private float _health;

    [Space]
    public BoxCollider2D HumanCol;
    public BoxCollider2D CartCol;

    private Rigidbody2D _rb;
    private States _state;
    private float _pointDir;

    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _state = States.TOEND;
        _pointDir = 1;
    }

    void Update() {
        ChangeState();
    }

    private void ChangeState() {
        //check health and change state to die if <0
        if (_health <= 0) {
            _state = States.DIE;
        }

        switch (_state) {
            case States.DIE:
                Die();
                break;
            case States.TOEND:
                ToEndPos();
                break;
            case States.TOSTART:
                ToStartPos();
                break;
        }
    }

    private void Die() {
        //die code
        Destroy(this.gameObject);
    }

    private void ToEndPos() {
        Debug.DrawLine(transform.position, EndPos.position, Color.red);

        //if near end pos go to startpos
        float distance = Vector2.Distance(transform.position, EndPos.position);
        if (distance <= DistOffset) {
            _state = States.TOSTART;
            return;
        }

        //shopping cart is to the left of the endpos
        if (transform.position.x - EndPos.position.x < 0) {
            //look right
            ChangeLookDir(1);
            //move right 
            _rb.velocity = new Vector2(Speed, _rb.velocity.y);
        } else //shopping cart is on the right of endpos
        {
            //look left
            ChangeLookDir(-1);
            //move left
            _rb.velocity = new Vector2(-Speed, _rb.velocity.y);
        }
    }

    private void ToStartPos() {
        Debug.DrawLine(transform.position, StartPos.position, Color.red);

        //if near start pos go to startpos
        float distance = Vector2.Distance(transform.position, StartPos.position);
        if (distance <= DistOffset) {
            _state = States.TOEND;
            return;
        }

        //go to start pos
        //shopping cart is to the left of the startpos
        if (transform.position.x - StartPos.position.x < 0) {
            //look right
            ChangeLookDir(1);
            //move right 
            _rb.velocity = new Vector2(Speed, _rb.velocity.y);
        } else //shopping cart is on the right of startpos
        {
            //look left
            ChangeLookDir(-1);
            //move left
            _rb.velocity = new Vector2(-Speed, _rb.velocity.y);
        }
    }

    public void TakeDamage(float dmg) {
        _health -= dmg;
    }

    /// <summary>
    /// method assumes that the default look direction of the sprite is pointing right
    /// </summary>
    /// <param name="dir">if >0 moving right, <0 moving left</param>
    private void ChangeLookDir(float moveDir) {
        if (moveDir < 0 && _pointDir > 0) {
            //moving left and looking right ^
            transform.Rotate(new Vector2(0, 180));
            _pointDir = -1;
        } else if (moveDir > 0 && _pointDir < 0) {
            //moving right and looking left ^
            transform.Rotate(new Vector2(0, 180));
            _pointDir = 1;
        }
    }
}
