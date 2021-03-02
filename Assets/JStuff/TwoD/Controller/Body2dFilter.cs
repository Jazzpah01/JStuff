using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body2dFilter : ScriptableObject
{
    public bool stackable;

    [Header("Physics variables factors")]
    [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    public float speed = 1;

    [SerializeField, Tooltip("Acceleration while grounded.")]
    public float walkAcceleration = 1;

    [SerializeField, Tooltip("Acceleration while in the air.")]
    public float airAcceleration = 1;

    [SerializeField, Tooltip("Deceleration applied when character is grounded and not attempting to move.")]
    public float groundDeceleration = 1;

    [SerializeField, Tooltip("Max height the character will jump regardless of gravity")]
    public float jumpHeight = 1;

    [SerializeField]
    public float gravityAcceleration = 1;

    [Header("Advanced physics")]
    [SerializeField, Tooltip("Gravitymultiplier on descend.")]
    public float GMDescend = 1;
    [SerializeField, Tooltip("Gravity multiplier on FallInput().")]
    public float GMFall = 1;
    [SerializeField, Tooltip("When hitting a wall, apply the inverse velocity x multiplied with the bounce factor.")]
    public float horizontalBounce = 1;
    [SerializeField, Tooltip("When hitting the ground, apply the inverse velocity y multiplied with the bounce factor.")]
    public float verticalBounce = 1;
    [SerializeField, Tooltip("The amount of frames where you can still jump after leaving ground.")]
    public float jackalFrames = 1;
}