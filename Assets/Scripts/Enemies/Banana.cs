using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    public AudioSource SlipSound;
    public PlayerManager Player;
    public float StunDuration;

    private void EnablePlayer()
    {
        Player.EnablePlayer();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //disable player and enable after stun duration
            Player.DisablePlayer();
            Invoke("EnablePlayer", StunDuration);

            //disable boxcollider
            GetComponent<BoxCollider2D>().enabled = false;

            //play slip sound
            SlipSound.Play();

            Destroy(this.gameObject, StunDuration + 0.001f);
        }
    }
}