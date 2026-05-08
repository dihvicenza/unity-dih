using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "DIHVicenza/PlayerData")]
public class PlayerData : ScriptableObject
{
    public float force;
    public float sensibility;
    public float maxAngle;
    public float jumpForce;
    public float gravity;
}
