using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletData", menuName = "SO/General Bullet Data", order = 1)]
public class BulletDataSO : ScriptableObject
{
    [field: SerializeField] public float ForwardSpeed { get; private set; } = 100f;
    [field: SerializeField] public byte Damage { get; private set; } = 10;
}
