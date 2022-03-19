using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("In Cutscene")]
    public bool InCutScene = false;

    [Header("Bind & Sound")]
    public KeyCode InputAttack;
    public AudioSource PlayerAudio;
    public AudioClip ShootSound;

    [Header("Stats")]
    public float Damage;
    public float ProjectileSpeed;
    public float WaitTime;

    [Header("Projectile")]
    public ProjectileBehavior Projectile;
    public Transform ProjectileOffset;
    public float DestroyDelay;

    private PlayerManager _playerM;
    private float _waitTill = -999f;

    void Start()
    {
        Projectile.Dmg = Damage;
        Projectile.Speed = ProjectileSpeed;
        Projectile.DestroyDelay = DestroyDelay;
        _playerM = GetComponent<PlayerManager>();
    }

    void Update()
    {
        if (InCutScene) return;
        Shoot();
    }

    private void Shoot() {
        if (!Input.GetKeyDown(InputAttack)) return;
        if (Time.time <= _waitTill) return;
        _waitTill = Time.time + WaitTime;

        //change sprite
        _playerM.SprIndex = 3;
        _playerM.ChangeSprite();
        _playerM.SprIndex = 0;
        _playerM.ChangeSpriteBack();

        //spawn spitball and play audio
        Instantiate(Projectile, ProjectileOffset.position, transform.rotation);
        PlayerAudio.PlayOneShot(ShootSound);
    }
}
