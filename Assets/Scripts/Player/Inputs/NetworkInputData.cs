using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public float xMovement, yMovement, hoverMovement, rollMovement;
    public Vector2 _lookInput, _screenCenter, _mouseDistance;
    public NetworkBool isFirePressed;
}
