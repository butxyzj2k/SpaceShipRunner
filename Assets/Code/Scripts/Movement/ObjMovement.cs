using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class defining the basic movement logic of an object in the scene.
/// </summary>
public abstract class ObjMovement : ButMonobehavior
{
    [Header("ObjMovement")]
    protected Vector3 targetPosition = Vector3.zero; 
    protected float moveSpeed; 

    protected virtual void Update() {
        if(CheckCanUpdateMoving()) Moving();
    }

    protected abstract object GetObjCtrl();

    /// <summary>
    /// Method to set the speed of movement.
    /// </summary>
    /// <param name="moveSpeed"> speed of movement </param>
    public virtual void SetMoveSpeed(float moveSpeed){
        this.moveSpeed = moveSpeed;
    }

    protected abstract void UpdateTargetPosition();

    protected virtual bool CheckCanUpdateMoving(){
        return GameManager.Instance.CurrentGameState.Equals(GameState.Running) 
            || GameManager.Instance.CurrentGameState.Equals(GameState.Restarting)
            || GameManager.Instance.CurrentGameState.Equals(GameState.Over);
    }

    // Handle movement logic, moving the object's parent position towards the target position.
    protected virtual void Moving(){
        UpdateTargetPosition();

        if(transform.parent.position.Equals(targetPosition)) return;
        transform.parent.position = Vector3.MoveTowards(transform.parent.position, 
                                                            targetPosition, 
                                                            moveSpeed * (1 + DifficultyManager.Instance.GameSpeedRate) * Time.deltaTime);
    }
}
