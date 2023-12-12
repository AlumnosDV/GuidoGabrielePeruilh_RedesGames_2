using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerGun : NetworkBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _spawningBullet;
    [SerializeField] private LayerMask _collisionLayers;
    [SerializeField] private float _shootCooldown = 0.15f;

    private float _lastShootTime;

    [Networked (OnChanged = nameof(OnFiringChanged))]
    private bool IsFiring { get; set; }


    public void Shoot()
    {
        if (Time.time - _lastShootTime < _shootCooldown) return;

        _lastShootTime = Time.time;
        
        StartCoroutine(ShootCooldown());

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

    }

}
