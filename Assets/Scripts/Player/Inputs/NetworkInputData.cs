using Fusion;

public struct NetworkInputData : INetworkInput
{
    public float xMovement;
    public float yMovement;
    public float hoverMovement;
    public NetworkBool isJumpPressed;
    public NetworkBool isFirePressed;
}
