using UnityEngine;

/// <summary>
///  Becase of desired angle is unstable, this code uses lerp to make the rotation smooth
/// </summary>
public abstract class ObjRotateByUnstableEulerAngle : ObjRotation
{
    // Method to get the final rotation of object.
    protected virtual Vector3 GetTheDesiredAngle(){
        //noop
        return Vector3.zero;
    }

    // Method interpolate smoothly from the current angle to the final angle
    protected virtual Vector3 CalculateTheAngleToDesiredAngle(Vector3 _desiredAngle){
        Vector3 _angleToRotate;
        //If the current rotation is close to the target angle (within the rotation threshold), set the target angle directly
        if(Quaternion.Angle(this.objModel.rotation, Quaternion.Euler(_desiredAngle)) < rotationThreshold) _angleToRotate = _desiredAngle;
        //Otherwise, interpolate smoothly from the current angle to the target angle using Quaternion.Lerp
        else _angleToRotate = Quaternion.Lerp(
                objModel.rotation,
                Quaternion.Euler(_desiredAngle),
                rotateSpeed * (1 + DifficultyManager.Instance.GameSpeedRate)
            ).eulerAngles;
        
        return _angleToRotate;
    } 

    protected override Vector3 CalculateTheCurrentRotationAngle(){
        var _desiredAngle = GetTheDesiredAngle();
        var _angleToRotate = CalculateTheAngleToDesiredAngle(_desiredAngle);
        return _angleToRotate;
    }
}