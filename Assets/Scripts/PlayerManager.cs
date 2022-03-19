using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    #region 
    [Header("RayCast")]
    public LayerMask RayCastLayers;
    public Transform RayCastLeft;
    public Transform RayCastRight;
    public float LROffset = 0.15f;

    private GameObject _hitObj;
    private GameObject _hitObjL;
    private GameObject _hitObjR;
    private Vector2 _hitPos;
    private float _distance = 0f;
    private float _distanceL = 0f;
    private float _distanceR = 0f;

    [Header("Movement")]
    public bool InCutscene = false;
    public KeyCode InputJump;
    public float Speed;
    public float JumpHeight;
    [Range(0.0f, 2f)]
    public float JumpOffset;

    [Header("GroundCheck")]
    public Transform GroundCheck;
    public float GroundCheckRadius;

    private bool _isGrounded = false;

    [Header("HP & DMG")]
    [SerializeField] private float _health;
    [SerializeField] private float _iFrames;
    [SerializeField] private float _jumpDmg;
    public Sprite HealthIcon;
    public Sprite HealthIconEmpty;
    private GameObject[] _healthIcons;
    private float _iFramesTimer = 0.0f;
    private bool _inIFrames = false;

    [Header("Sound")]
    public AudioSource PlayerSound;
    public AudioClip[] Sounds;

    [Header("Sprites")]
    public Sprite[] Sprites;
    public int SprIndex;

    private SpriteRenderer _sprRend;

    private Rigidbody2D _rb;
    private PlayerAttack _attackComponent;

    #endregion

    private void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _attackComponent = GetComponent<PlayerAttack>();
        _healthIcons = GameObject.FindGameObjectsWithTag("Health");
        _sprRend = GetComponent<SpriteRenderer>();

        _sprRend.sprite = Sprites[0];
    }

    void Update() {
        if (InCutscene) return;

        CheckHealth();
        RayCast();
        Move();

    }

    private void RayCast() {
        //raycast down
        RaycastHit2D hitM = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, RayCastLayers);
        RaycastHit2D hitL = Physics2D.Raycast(RayCastLeft.position, Vector2.down, Mathf.Infinity, RayCastLayers);
        RaycastHit2D hitR = Physics2D.Raycast(RayCastRight.position, Vector2.down, Mathf.Infinity, RayCastLayers);

        //if not over anything
        if (hitM.transform == null && hitL.transform == null && hitR.transform == null) {
            return;
        }

        //debug
        Debug.DrawLine(transform.position, hitM.point, Color.red);
        Debug.DrawLine(RayCastLeft.position, hitL.point, Color.red);
        Debug.DrawLine(RayCastRight.position, hitR.point, Color.red);

        //get object and position underneath player
        _hitObj = hitM.transform.gameObject;
        _hitObjL = hitL.transform.gameObject;
        _hitObjR = hitR.transform.gameObject;
        _hitPos = hitM.point;
        Debug.Log(_hitObj.name);

        //distance to object underneath
        _distance = Mathf.Abs(hitM.point.y - transform.position.y);
        _distanceL = Mathf.Abs(hitL.point.y - RayCastLeft.position.y);
        _distanceR = Mathf.Abs(hitR.point.y - RayCastRight.position.y);

        //if land on object and it's an enemy
        LandOnEnemy();
    }

    private void Move() {
        //Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, RayCastLayers);

        //ground check
        if (_distance < JumpOffset && _rb.velocity.y < 0) {
            _isGrounded = true;

            //move character to floor
            _rb.velocity = new Vector2(_rb.velocity.x, -2f);
        }

        //horizontal movement
        float movement = Input.GetAxis("Horizontal");
        _rb.velocity = new Vector2(movement * Speed, _rb.velocity.y);

        //turn character towards direction it's moving
        if (Input.GetKeyDown(KeyCode.LeftArrow) && transform.rotation.y >= 0) {
            //moving left and looking right ^
            transform.Rotate(new Vector2(0,180));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && transform.rotation.y < 0) {
            //moving right and looking left ^
            transform.Rotate(new Vector2(0, 180));
        }

        //jumping
        if (Input.GetKeyDown(InputJump) && _isGrounded) {
            Jump();
        }
    }
    private void Jump() {
        _isGrounded = false;
        _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Sqrt(JumpHeight * -2 * -9.8f));
        PlayerSound.PlayOneShot(Sounds[1]);
    }
    private void CheckHealth() {
        if (_inIFrames)
        {
            _iFramesTimer += Time.deltaTime;
            if (_iFramesTimer >= _iFrames)
            {
                _inIFrames = false;
                _iFramesTimer = 0.0f;
            }
        }
        

        if (_health <= 0) {
            //die, restarts current scene 
            print("YOU DIED");
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
            //Destroy(this);
        }
        else if (_healthIcons.Length == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                if (i < _health)
                {
                    _healthIcons[i].GetComponent<Image>().sprite = HealthIcon;
                }
                else
                {
                    _healthIcons[i].GetComponent<Image>().sprite = HealthIconEmpty;
                }
            }
        }
    }

    private void LandOnEnemy() {
        bool rightOnEnemy = _hitObjR.layer == LayerMask.NameToLayer("Enemy") || _hitObjR.layer == LayerMask.NameToLayer("JumpingEnemy");
        bool leftOnEnemy = _hitObjL.layer == LayerMask.NameToLayer("Enemy") || _hitObjL.layer == LayerMask.NameToLayer("JumpingEnemy");
        bool midOnEnemy = _hitObj.layer == LayerMask.NameToLayer("Enemy") || _hitObj.layer == LayerMask.NameToLayer("JumpingEnemy");

        bool midLanded = _distance < JumpOffset && midOnEnemy;
        bool leftLanded = _distanceL < LROffset && leftOnEnemy;
        bool rightLanded = _distanceR < LROffset && rightOnEnemy;

        if (_distance < JumpOffset || _distanceL < LROffset || _distanceR < LROffset) {
            Debug.DrawLine(transform.position, _hitPos, Color.green);
            Debug.DrawLine(RayCastLeft.position, _hitPos, Color.green);
            Debug.DrawLine(RayCastRight.position, _hitPos, Color.green);
        }

        //if land on enemy, bounce off and damage it
        //if it is a KOProjectile, dont damage it, destroy it
        if (midLanded) {
            Jump();
            if (_hitObj.tag != "KOProjectile") {
                _hitObj.GetComponent<EnemyHealth>().TakeDamage(_jumpDmg);
            } else if (_hitObj.tag == "KOProjectile") {
                Destroy(_hitObj.gameObject);
            }
        } else if (leftLanded) {
            Jump();
            if (_hitObjL.tag != "KOProjectile") {
                _hitObjL.GetComponent<EnemyHealth>().TakeDamage(_jumpDmg);
            } else if (_hitObjL.tag == "KOProjectile") {
                Destroy(_hitObjL.gameObject);
            }
        } else if (rightLanded) {
            Jump();
            if (_hitObjR.tag != "KOProjectile") {
                _hitObjR.GetComponent<EnemyHealth>().TakeDamage(_jumpDmg);
            } else if (_hitObjR.tag == "KOProjectile") {
                Destroy(_hitObjR.gameObject);
            }
        }
    }

    public void ChangeSpriteBack() {
        Invoke("ChangeSprite", 0.1f);
    }

    public void ChangeSprite() {
        _sprRend.sprite = Sprites[SprIndex];
    }

    public void DisablePlayer() {
        _rb.velocity = new Vector2(0, _rb.velocity.y);
        _attackComponent.InCutScene = true;
        InCutscene = true;

        //banana'd sprite
        SprIndex = 2;
        ChangeSprite();
    }

    public void EnablePlayer() {
        _attackComponent.InCutScene = false;
        InCutscene = false;

        //default sprite
        SprIndex = 0;
        ChangeSprite();
    }

    public void TakeDamage(float dmg) {
        if (_inIFrames) return;

        _inIFrames = true;
        _health -= dmg;
        PlayerSound.PlayOneShot(Sounds[0]);

        //change sprite to hurt sprite
        SprIndex = 1;
        ChangeSprite();
        //change sprite to default one
        SprIndex = 0;
        Invoke("ChangeSprite", 0.3f);

        //make player bounce away
    }

    public void GainHealth(float healing)
    {
        if (_health >= 3) return;
        else
        {
            _health += healing;
        }
    }


    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.layer != LayerMask.NameToLayer("Enemy") && col.gameObject.layer != LayerMask.NameToLayer("JumpingEnemy")) {
            return;
        }
        bool midNotTouching = _hitObj.layer != LayerMask.NameToLayer("Enemy") || _hitObj.layer != LayerMask.NameToLayer("JumpingEnemy");
        bool leftNotTouching = _hitObjL.layer != LayerMask.NameToLayer("Enemy") || _hitObjL.layer != LayerMask.NameToLayer("JumpingEnemy");
        bool rightNotTouching = _hitObjR.layer != LayerMask.NameToLayer("Enemy") || _hitObjR.layer != LayerMask.NameToLayer("JumpingEnemy");
        
        if (midNotTouching && leftNotTouching && rightNotTouching) {
            TakeDamage(1);
        }        
    }
}
