using System;
using UnityEngine;

public readonly struct MovementRequest : IEquatable<MovementRequest>
{
    /// <summary>
    /// Direction of the movement
    /// </summary>
    public readonly Vector3 Direction;

    /// <summary>
    /// Acceleration for the movement
    /// </summary>
    public readonly float Acceleration;

    /// <summary>
    /// Goal Speed for the movement
    /// </summary>
    public readonly float GoalSpeed;

    public static MovementRequest InvalidRequest => new(Vector3.zero, 0, 0);

    public MovementRequest(Vector3 direction,
                           float goalSpeed,
                           float acceleration)
    {
        Direction = direction;
        Acceleration = acceleration;
        GoalSpeed = goalSpeed;
    }

    public Vector3 GetGoalVelocity() => Direction * GoalSpeed;
    public Vector3 GetAccelerationVector() => Direction * Acceleration;

    public bool IsValid() => this != InvalidRequest;

    public bool Equals(MovementRequest other)
    {
        return Direction.Equals(other.Direction)
               && Acceleration.Equals(other.Acceleration)
               && GoalSpeed.Equals(other.GoalSpeed);
    }

    public override bool Equals(object obj)
    {
        return obj is MovementRequest other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Direction.GetHashCode();
            hashCode = (hashCode * 397) ^ GoalSpeed.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString()
    {
        return IsValid()
            ? $"MovementRequest({Direction}, {GoalSpeed}, {Acceleration})"
            : "MovementRequest(Invalid)";
    }

    public static bool operator ==(MovementRequest one, MovementRequest two)
    {
        return one.Equals(two);
    }

    public static bool operator !=(MovementRequest one, MovementRequest two)
    {
        return !(one == two);
    }
}
