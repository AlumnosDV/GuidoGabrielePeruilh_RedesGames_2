using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerGun : NetworkBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _spawningBullet;
    [SerializeField] private ParticleSystem _fireParticles;
    [SerializeField] private LayerMask _collisionLayers;
    [SerializeField] private byte _damage = 10;
    [SerializeField] private float _shootCooldown = 0.15f;

    private float _lastShootTime;

    [Networked (OnChanged = nameof(OnFiringChanged))]
    private bool IsFiring { get; set; }


    public void Shoot(Vector3 aimForward)
    {
        if (Time.time - _lastShootTime < _shootCooldown) return;

        _lastShootTime = Time.time;
        
        StartCoroutine(ShootCooldown());

        Runner.Spawn(_bulletPrefab, _spawningBullet.position, transform.rotation);

        var raycast = Runner.LagCompensation.Raycast(origin: _spawningBullet.position,
                                                        direction: aimForward,
                                                        length: 100,
                                                        player: Object.InputAuthority,
                                                        hit: out var hitInfo,
                                                        _collisionLayers,
                                                        HitOptions.IncludePhysX);
        var hitDistance = 100f;
        var isHitOtherPlayer = false;

        if (hitInfo.Distance > 0)
            hitDistance = hitInfo.Distance;

        if (isHitOtherPlayer)
        {
            Debug.DrawRay(_spawningBullet.position, aimForward * hitDistance, Color.red, 1);
        }
        else
        {
            Debug.DrawRay(_spawningBullet.position, aimForward * hitDistance, Color.green, 1);
        }

        if (!raycast) return;

        Debug.Log(hitInfo.Hitbox.Root.gameObject.name);
        hitInfo.GameObject.GetComponentInParent<LifeHandler>()?.TakeDamage(_damage);

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
