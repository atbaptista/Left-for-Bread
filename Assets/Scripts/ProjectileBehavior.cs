using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public AudioSource HitSound;
    public SpriteRenderer Sprite;
    public GameObject Shooter;

    [HideInInspector]
    public float Speed;
    [HideInInspector]
    public float Dmg;
    [HideInInspector]
    public float DestroyDelay;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, DestroyDelay);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * Speed; 
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") || collision.gameObject.layer == LayerMask.NameToLayer("JumpingEnemy")) {
            if (collision.gameObject.tag == "KOProjectile") {
                Destroy(collision.gameObject);
            } else if (Shooter.CompareTag("Player")){
                collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(Dmg);
            } 
        }

        if (HitSound != null) {
            HitSound.Play();
        }

        if (Shooter.CompareTag("Enemy") && collision.gameObject.tag != "Enemy") {
            Destroy(this.gameObject, 0.1f);
            GetComponent<BoxCollider2D>().enabled = false;
            Sprite.enabled = false;
        }

        //player shoots
        if (Shooter.CompareTag("Player")) {
            Destroy(this.gameObject, 0.1f);
            GetComponent<BoxCollider2D>().enabled = false;
            Sprite.enabled = false;
        }
    }
}
