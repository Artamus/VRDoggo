using System.Collections.Generic;
using UnityEngine;

public class DogAgent : Agent {
    public float speed = 10;
    private float previousDistance = float.MaxValue;
    private Plane plane;

    Rigidbody rBody;

    void Start() {
        rBody = GetComponent<Rigidbody>();
        plane = new Plane(Vector3.up, transform.position);
    }

    public override void AgentReset() {
        // Move the target to a new spot
        this.transform.position = new Vector3(0, 0.5f, 0);
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
    }

    public override void CollectObservations() {
        // Calculate relative position
        Vector3 relativePosition = relativePos();

        // Relative position
        AddVectorObs(relativePosition.x / 5);
        AddVectorObs(relativePosition.z / 5);

        // Distance to edges of platform
        AddVectorObs((this.transform.position.x + 5) / 5);
        AddVectorObs((this.transform.position.x - 5) / 5);
        AddVectorObs((this.transform.position.z + 5) / 5);
        AddVectorObs((this.transform.position.z - 5) / 5);

        // Agent velocity
        AddVectorObs(rBody.velocity.x / 5);
        AddVectorObs(rBody.velocity.z / 5);
    }

    public override void AgentAction(float[] vectorAction, string textAction) {
        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.position, projectionPoint());

        // Reached target
        if (distanceToTarget < 1.42f) {
            Done();
            AddReward(1.0f);
        }

        // Getting closer
        if (distanceToTarget < previousDistance) {
            AddReward(0.1f);
        }

        // Time penalty
        AddReward(-0.05f);

        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = Mathf.Clamp(vectorAction[0], -1, 1);
        controlSignal.z = Mathf.Clamp(vectorAction[1], -1, 1);
        rBody.AddForce(controlSignal * speed);
    }

    private Vector3 relativePos() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance = 1000;
        Vector3 relativePosition = Vector3.zero;
        if (plane.Raycast(ray, out distance)) {
            Vector3 hitPoint = ray.GetPoint(distance);
            Vector3 projectionPoint = new Vector3(hitPoint.x, 0.0f, hitPoint.z);
            relativePosition = projectionPoint - transform.position;
        }
        return relativePosition;
    }

    private Vector3 projectionPoint() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance = 1000;
        Vector3 hitPoint = Vector3.zero;
        if (plane.Raycast(ray, out distance)) {
            hitPoint = ray.GetPoint(distance);
            Vector3 projectionPoint = new Vector3(hitPoint.x, 0.0f, hitPoint.z);
        }
        return hitPoint;
    }
}