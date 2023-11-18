using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public float xMovement, yMovement, hoverMovement, rollMovement;
    public Vector2 lookInput, screenCenter;
    public Vector3 aimForwardVector;
    public NetworkBool isFirePressed;
}
