using UnityEngine;

public static class StaticUtilities
{
    // 1) Animations
    public static readonly int XSpeedAnimId = Animator.StringToHash("xSpeed");
    public static readonly int YSpeedAnimId = Animator.StringToHash("ySpeed");
    public static readonly int IdleAnimId = Animator.StringToHash("IdleState");
    public static readonly int AttackAnimID = Animator.StringToHash("Attack");
    public static readonly int TurnAnimID = Animator.StringToHash("Turn");

    // 2) Layers
    public static readonly int GroundLayer = 1 << LayerMask.NameToLayer("Ground");
    public static readonly int PlayerLayer = 1 << LayerMask.NameToLayer("Player");
    public static readonly int EnemyLayer =  1  << LayerMask.NameToLayer("Enemy");
    
    public static readonly int MoveLayerMask = GroundLayer | EnemyLayer;
    

    // 3) Shaders
    public static readonly int Color = Shader.PropertyToID("_Color");
}
