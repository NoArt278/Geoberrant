using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PowerShapes : MonoBehaviour
{
    List<Vector2> points;
    float mass, rollSpeed, jumpHeight, moveSpeed, scaleOffset, bounciness;
    ShapeTangentMode shapeTangentMode;

    // Setters
    public void SetPoints(List<Vector2> points)
    {
        this.points = points;
    }
    public void SetMass(float mass)
    {
        this.mass = mass;
    }
    public void SetRollSpeed(float rollSpeed)
    {
        this.rollSpeed = rollSpeed;
    }
    public void SetJumpHeight(float jumpHeight)
    {
        this.jumpHeight = jumpHeight;
    }
    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }
    public void SetScaleOffset(float scaleOffset)
    {
        this.scaleOffset = scaleOffset;
    }
    public void SetBounciness(float bounciness)
    {
        this.bounciness = bounciness;
    }
    public void SetTangentMode(ShapeTangentMode shapeTangentMode)
    {
        this.shapeTangentMode = shapeTangentMode;
    }

    // Getters
    public List<Vector2> GetPoints()
    {
        return this.points;
    }
    public float GetMass()
    {
        return this.mass;
    }
    public float GetRollSpeed()
    {
        return this.rollSpeed;
    }
    public float GetJumpHeight()
    {
        return this.jumpHeight;
    }
    public float GetMoveSpeed()
    {
        return this.moveSpeed;
    }
    public float GetScaleOffset()
    {
        return this.scaleOffset;
    }
    public float GetBounciness()
    {
        return this.bounciness;
    }
    public ShapeTangentMode GetTangentMode()
    {
        return this.shapeTangentMode;
    }

    public virtual void ActivatePower() { }
}
