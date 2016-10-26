﻿using System;
using System.Collections.Generic;

#if UNITY_5
using UnityEngine;
#endif

using Utility;

[Serializable]
public class Boid
{
#if UNITY_5
    [SerializeField]
#endif
    private Vector m_Position;
#if UNITY_5
    [SerializeField]
#endif
    private Vector m_Velocity;

    private static readonly List<Boid> s_Boids = new List<Boid>();

    private static float s_Cohesion = 1f;
    private static float s_Separation = 1f;
    private static float s_Alignment;

    private static float s_VelocityLimit = 10f;

    private static float s_TendTowards = 1f;
    private static Vector s_TendToPosition;

    private static float s_Avoid = 0.95f;
    private static Vector s_AvoidPosition;

    public static float cohesion
    {
        get { return s_Cohesion; }
        set { s_Cohesion = value; }
    }
    public static float separation
    {
        get { return s_Separation; }
        set { s_Separation = value; }
    }
    public static float alignment
    {
        get { return s_Alignment; }
        set { s_Alignment = value; }
    }

    public static float velocityLimit
    {
        get { return s_VelocityLimit; }
        set { s_VelocityLimit = value; }
    }

    public static float tendTowardsPower
    {
        get { return s_TendTowards; }
        set { s_TendTowards = value; }
    }
    public static Vector tendToPosition
    {
        get { return s_TendToPosition; }
        set { s_TendToPosition = value; }
    }

    public static float avoidPower
    {
        get { return s_Avoid; }
        set { s_Avoid = value; }
    }
    public static Vector avoidPosition
    {
        get { return avoidPosition; }
        set { s_AvoidPosition = value; }
    }

    public Vector position
    {
        get { return m_Position; }
    }

    public Vector velocity
    {
        get { return m_Velocity; }
    }

    public Boid(Vector position = new Vector())
    {
        m_Position = position;

        s_Boids.Add(this);
    }

    public static bool Update(float deltaTime)
    {
        if (s_Boids.Count == 0)
            return false;

        foreach (var boid in s_Boids)
        {
            var cohesion = boid.Cohesion() * s_Cohesion;
            var separation = boid.Separation() * s_Separation;
            var alignment = boid.Alignment() * s_Alignment;

            var tendTowards = boid.TendTowards() * s_TendTowards;
            var avoid = boid.Avoid() * s_Avoid;

            boid.m_Velocity += cohesion + alignment + separation + tendTowards + avoid;
            if (boid.m_Velocity.magnitude > s_VelocityLimit)
                boid.m_Velocity = boid.m_Velocity / boid.m_Velocity.magnitude * s_VelocityLimit;

            boid.m_Position += boid.m_Velocity * deltaTime;
        }

        return true;
    }

    // Cohesion
    private Vector Cohesion()
    {
        var percievedCenter = new Vector();
        foreach (var boid in s_Boids)
            if (boid != this)
                percievedCenter += boid.m_Position;

        percievedCenter /= s_Boids.Count - 1;

        return (percievedCenter - m_Position).normalized;
    }

    // Separation
    private Vector Separation()
    {
        var displacement = new Vector();
        foreach (var boid in s_Boids)
            if (boid != this)
                if ((boid.m_Position - m_Position).magnitude <= 2f)
                    displacement -= boid.m_Position - m_Position;

        return displacement;
    }

    // Alignment
    private Vector Alignment()
    {
        var percievedVelocity = new Vector();
        foreach (var boid in s_Boids)
        {
            if (boid != this)
                percievedVelocity += boid.m_Velocity;
        }

        percievedVelocity /= s_Boids.Count - 1;

        return (percievedVelocity - m_Velocity).normalized;
    }

    private Vector TendTowards()
    {
        return (s_TendToPosition - position).normalized;
    }

    private Vector Avoid()
    {
        return -(s_AvoidPosition - position).normalized;
    }

    ~Boid()
    {
        s_Boids.Remove(this);
    }
}
