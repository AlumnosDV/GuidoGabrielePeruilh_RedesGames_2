using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public float xMovement, yMovement, hoverMovement, rollMovement;
    public Vector2 lookInput, screenCenter;
    public Quaternion aimForwardVector;
    public NetworkBool isFirePressed;
}
