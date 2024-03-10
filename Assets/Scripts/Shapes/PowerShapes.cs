using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PowerShapes : MonoBehaviour
{
    List<Vector2> points;
    float mass, rollSpeed, jumpHeight, moveSpeed, scaleOffset, bounciness, gravityScale, def;
    ShapeTangentMode shapeTangentMode;
    Color color;

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
    public void SetGravScale(float gravityScale)
    {
        this.gravityScale = gravityScale;
    }
    public void SetDef(float def)
    {
        this.def = def;
    }
    public void SetTangentMode(ShapeTangentMode shapeTangentMode)
    {
        this.shapeTangentMode = shapeTangentMode;
    }
    public void SetColor(Color color)
    {
        this.color = color;
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
    public float GetGravScale()
    {
        return this.gravityScale;
    }
    public float GetDef()
    { 
        return this.def; 
    }
    public ShapeTangentMode GetTangentMode()
    {
        return this.shapeTangentMode;
    }
    public Color GetColor()
    { 
        return this.color; 
    }

    public virtual void ActivatePower() { }
}
