using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidMovement : MonoBehaviour {
    [SerializeField] private ListBoidVariable boids;
    private float radius = 2f;
    private float forwardSpeed = 5f;
    private float visionAngle = 270f;
    public Vector3 Verlocity { get; private set; }

    private void FixedUpdate() {
        // Cập nhật Verlocity theo hướng di chuyển
        Verlocity = Vector2.Lerp(Verlocity, transform.forward.normalized * forwardSpeed, Time.fixedDeltaTime);
        // Tính toán cập nhật vị trí mới dựa theo vận tốc
        transform.position += Verlocity * Time.fixedDeltaTime;
        LookRotation();
    }

    private void LookRotation() {
        transform.rotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(Verlocity), Time.fixedDeltaTime);
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


}
