using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Derived class from ObjectMoveByPointRecycleLoop that creates a recycle movement trajectory loop for walkable tile.
/// </summary>
public class WalkableTileMoveByPointRecycleLoop : ObjectMoveByStaticPointRecycleLoop
{
    protected override void LoadValue()
    {
        base.LoadValue();
        moveSpeed = ((WalkableTileCtrl)GetObjCtrl()).walkableTileConfig.InitialMoveSpeed;
        spawnPoint = ((WalkableTileCtrl)GetObjCtrl()).walkableTileConfig.InitialSpawnPoint;
        targetPoint = ((WalkableTileCtrl)GetObjCtrl()).walkableTileConfig.InitialTargetPoint;
        remainingLoops = ((WalkableTileCtrl)GetObjCtrl()).walkableTileConfig.InitialRemainingLoops;
    }

    protected override object GetObjCtrl()
    {
        return this.transform.parent.GetComponent<WalkableTileCtrl>();
    }
    protected override void InitializeResetMoving()
    {
        base.InitializeResetMoving();
        //Post event that walkabletile is reseted
        Observer.PostEvent(EventID.ResetWalkableTile, new KeyValuePair<EventParameterType, object>(EventParameterType.ResetWalkableTile_WalkableTileObject, this.transform.parent.gameObject));
    }
}