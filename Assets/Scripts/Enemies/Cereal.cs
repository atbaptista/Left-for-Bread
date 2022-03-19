using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cereal : MonoBehaviour, EnemyHealth
{
    public float Health = 2;
    [Range(-1, 1)]
    public int Direction = 1;

    [Header("Projectile")]
    public ProjectileBehavior CerealShot;
    public Transform CerealOffset;

    [Header("Proj Stats")]
    public float Damage;
    public float ProjectileSpeed;
    public float ProjLifetime;


    public float FireRate;
    private float _waitTill = -999f;

    void Start() {
        
    }
    void Update() { 
        CheckHealth();
        Shoot();
    }

    private void Shoot() {
        if (Time.time <= _waitTill) return;
        _waitTill = Time.time + FireRate;
        CerealShot.Dmg = Damage;
        CerealShot.Speed = ProjectileSpeed * Direction;
        CerealShot.DestroyDelay = ProjLifetime;
        Instantiate(CerealShot, CerealOffset.position, transform.rotation);
    }

    private void CheckHealth() {
        if (Health > 0) return;
        Destroy(gameObject);
    }

    public void TakeDamage(float dmg) {
        Health -= dmg;
    }
}
