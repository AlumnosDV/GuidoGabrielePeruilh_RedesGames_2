using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerGun : NetworkBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _spawningBullet;
    [SerializeField] private ParticleSystem _fireParticles;

    [SerializeField] private float _shootCooldown = 0.15f;

    private float _lastShootTime;

    [Networked (OnChanged = nameof(OnFiringChanged))]
    private bool IsFiring { get; set; }
    
    public void Shoot()
    {
        if (Time.time - _lastShootTime < _shootCooldown) return;

        _lastShootTime = Time.time;
        
        StartCoroutine(ShootCooldown());

        Quaternion playerRotation = transform.rotation;

        Quaternion bulletRotation = Quaternion.LookRotation(transform.forward, Vector3.up);

        // Instanciar la bala con la rotación ajustada
        Runner.Spawn(_bulletPrefab, _spawningBullet.position, transform.rotation);

    }

    IEnumerator ShootCooldown()
    {
        IsFiring = true;
        yield return new WaitForSeconds(_shootCooldown);
        IsFiring = false;
    }
    
    static void OnFiringChanged(Changed<PlayerGun> changed)
    {
        bool currentFiring = changed.Behaviour.IsFiring;
        changed.LoadOld();
        bool oldFiring = changed.Behaviour.IsFiring;

        if (!oldFiring && currentFiring) changed.Behaviour.TurnOnShootingParticle();
    }

    void TurnOnShootingParticle()
    {
        if (_fireParticles == null) return;
        _fireParticles.Play();
    }
}
