using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerEnemy : MonoBehaviour, EnemyHealth {
    private enum States {
        DIE, CHASE, IDLE
    }

    [Header("RayCast Layers")]
    public LayerMask JumpableLayers;

    [Header("Movement")]
    public Transform Player;
    public float Speed;
    public float DistToPlayerOffset;
    public float JumpHeight;
    public float JumpOffset;

    [Header("Health & Attacking")]
    [SerializeField] private float _health;
    public float AggroRange;
    public float Damage = 1;

    private Rigidbody2D _rb;
    private States _state;
    private GameObject _hitObj;
    private GameObject _hitObjUp;
    private bool _isGrounded;
    private float _distance;
    private float _disToPlayer;

    private bool playerOnRight;
    private bool playerOnLeft;
    private bool playerAbove;
    private bool playerBelow;
    private bool onPlatform;
    private bool underPlatform;

    public float JumpRate;
    private float _waitTill = -999f;

    private float _pointDir;

    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _state = States.IDLE;
        _isGrounded = true;
        _pointDir = 1;
    }

    void Update() {
        RayCast();
        ChangeState();
    }

    private void ChangeState() {
        //check health and change state to die if <0
        if (_health <= 0) {
            _state = States.DIE;
        }

        _disToPlayer = (transform.position - Player.position).magnitude;

        switch (_state) {
            case States.DIE:
                Die();
                break;
            case States.IDLE:
                Idle();
                break;
            case States.CHASE:
                Chase();
                break;
        }
    }

/*    private void FixedUpdate() {
        if (_state == States.CHASE) {
            Chase();
        }
    }*/

    private void Idle() {
        if (_disToPlayer < AggroRange) {
            _state = States.CHASE;
        }
    }

    private void Chase() {
        playerOnRight = transform.position.x - Player.position.x < 0;
        playerOnLeft = transform.position.x - Player.position.x > 0;
        playerAbove = transform.position.y - Player.position.y < 0;
        playerBelow = transform.position.y - Player.position.y > 0;
        onPlatform = _hitObj.CompareTag("Platform");
        underPlatform = _hitObjUp.CompareTag("Platform") && playerAbove;

        //player.y is above enemy
        if (playerAbove) {
            if (Time.time > _waitTill) 
            {
                _waitTill = Time.time + JumpRate;
                Jump();
            }
        } //player below enemy
        else if (playerBelow) {
            if (onPlatform) {

            }
        }

        //player is on right of enemy
        if (transform.position.x - Player.position.x < 0) {
            //move right
            _rb.velocity = new Vector2(Speed, _rb.velocity.y);
            ChangeLookDir(1);
        } else if (transform.position.x - Player.position.x > 0) { //player is on left of enemy
            //move left
            _rb.velocity = new Vector2(-Speed, _rb.velocity.y);
            ChangeLookDir(-1);
        }
    }

    private void Jump() {
        if (_isGrounded) {
            _isGrounded = false;
            _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Sqrt(JumpHeight * -2 * -9.8f));
        }
    }

    private void Die() {
        //die code
        Destroy(this.gameObject);
    }

    public void TakeDamage(float dmg) {
        _health -= dmg;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            collision.gameObject.GetComponent<PlayerManager>().TakeDamage(Damage);
        }
    }

    private void ChangeLookDir(float moveDir)
    {
        if (moveDir < 0 && _pointDir > 0)
        {
            //moving left and looking right ^
            transform.Rotate(new Vector2(0, 180));
            _pointDir = -1;
        }
        else if (moveDir > 0 && _pointDir < 0)
        {
            //moving right and looking left ^
            transform.Rotate(new Vector2(0, 180));
            _pointDir = 1;
        }
    }

    private void RayCast() {
        //raycast down
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, JumpableLayers);
        //raycast up
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, JumpableLayers);

        //if not over anything
        if (hit.transform == null) {
            return;
        }

        //raycast up code
        if (hitUp.transform != null) {
            _hitObjUp = hit.transform.gameObject;
            Debug.DrawLine(transform.position, hitUp.point, Color.red);
        }

        //debug
        Debug.DrawLine(transform.position, hit.point, Color.red);

        //get object and position underneath watermelon
        _hitObj = hit.transform.gameObject;

        //distance to object underneath
        _distance = Mathf.Abs(hit.point.y - transform.position.y);

        //ground check
        if (_distance < JumpOffset && _rb.velocity.y < 0) {
            _isGrounded = true;

            //move object to floor
            _rb.velocity = new Vector2(_rb.velocity.x, -2f);
        }
    }

    void OnDrawGizmosSelected() {
        //draw aggro range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AggroRange);
    }


}