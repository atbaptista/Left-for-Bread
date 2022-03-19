using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float Healing = 1f;
    public AudioSource PickupSound;
    public float BounceHeight = 0.001f;

    private void Update()
    {
        transform.position = new Vector3 (transform.position.x, transform.position.y + Mathf.Sin(Time.fixedTime * Mathf.PI) * BounceHeight, transform.position.z);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerManager>().GainHealth(Healing);
            this.GetComponent<CircleCollider2D>().enabled = false;
            this.GetComponent<SpriteRenderer>().enabled = false;
            PickupSound.Play();

            Destroy(gameObject, 1);
        }
    }
}
