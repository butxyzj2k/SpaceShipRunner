using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ObjectConfig/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    [Header("Movement")]
    public float InitialMoveSpeed;

    [Header("Rotation")]
    public float InitialRotateSpeed;

    [Header("Collision")]
    public float InitialColliderRadius;
    public List<ObjTagCollision> InitialTagOfCollisionableObject;
    public ObjTagCollision InitialTagOfObject;
}