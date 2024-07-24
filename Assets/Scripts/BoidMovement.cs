using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidMovement : MonoBehaviour {
    [SerializeField] private ListBoidVariable boids;
    private float radius = 2f;
    private float forwardSpeed = 5f;
    private float visionAngle = 270f;
    private float turnSpeed = 10f;
    public Vector3 Velocity { get; private set; }

    private void FixedUpdate() {
        // Cập nhật Velocity theo hướng di chuyển
        Velocity = Vector2.Lerp(Velocity, CalculateVelocity(),turnSpeed / 2 * Time.fixedDeltaTime);
        // Tính toán cập nhật vị trí mới dựa theo vận tốc
        transform.position += Velocity * Time.fixedDeltaTime;
        LookRotation();
    }

    private Vector2 CalculateVelocity() {
        var boidsInRage = BoidsInRage();
        Vector2 velocity = ((Vector2)transform.forward + Separation(boidsInRage)).normalized * forwardSpeed;
        return velocity;
    }

    private void LookRotation() {
        transform.rotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(Velocity),turnSpeed * Time.fixedDeltaTime);
    }

    private List<BoidMovement> BoidsInRage() {
        var listBoid = boids.boidMovements.FindAll(boid => boid != this
        && (boid.transform.position - transform.position).magnitude <= radius
        && InVisionCone(boid.transform.position));
    return listBoid;
    }

    private bool InVisionCone(Vector2 position) {
        Vector2 directionToPosition = position - (Vector2)transform.position;
        float dotProduct = Vector2.Dot(transform.forward, directionToPosition);
        float cosHalfVisionAngle = Mathf.Cos(visionAngle * 0.5f * Mathf.Deg2Rad);
        return dotProduct >= cosHalfVisionAngle;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);

        var boidsInRage = BoidsInRage();
        foreach(var boid in boidsInRage){
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, boid.transform.position);
        }
    }

    private Vector2 Separation(List<BoidMovement> boidMovements) { 
        Vector2 diretion = Vector2.zero;
        foreach (var boid in boidMovements) {
            float ratio = Mathf.Clamp01((boid.transform.position - transform.position).magnitude / radius);
            diretion -= ratio * (Vector2)(boid.transform.position - transform.position);
        }
        return diretion.normalized;
    }
}
