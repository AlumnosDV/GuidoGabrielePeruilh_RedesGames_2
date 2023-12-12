using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "SO/General Player Data", order = 0)]
public class PlayerDataSO : ScriptableObject
{
    [field: SerializeField] public float ForwardSpeed { get; private set; } = 50f;
    [field: SerializeField] public float SideSpeed { get; private set; } = 15f;
    [field: SerializeField] public float HoverSpeed { get; private set; } = 10f;
    [field: SerializeField] public float RollSpeed { get; private set; } = 30f;
    [field: SerializeField] public float LookRotateSpeed { get; private set; } = 30f;
    [field: SerializeField] public float ShootCooldown { get; private set; } = 0.2f;
    [field: SerializeField] public byte MaxLife { get; private set; } = 100;
    [field: SerializeField] public byte Lives = 3;
}
