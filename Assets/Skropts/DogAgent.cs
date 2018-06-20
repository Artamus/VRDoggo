using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using UnityEngine.UI;
using System;

public class DogAgent : Agent {
    public float speed = 10;
    private float previousDistance = float.MaxValue;
    private Plane plane;
    private Color yesColor = new Color(0f, 0f, 255.0f);
    private Color noColor = new Color(255.0f, 0f, 0f);


    Rigidbody rBody;
    public Text text;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        plane = new Plane(Vector3.up, transform.position);
    }

    public override void AgentReset()
    {
        // Move the target to a new spot
        this.transform.position = new Vector3(0, 0.5f, 0);
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
    }

    public override void CollectObservations()
    {
        // Calculate relative position
        Vector3 relativePosition = relativePos();

        // Relative position
        AddVectorObs(relativePosition.x / 25);
        AddVectorObs(relativePosition.z / 25);

        /*
        // Distance to edges of platform
        AddVectorObs((this.transform.position.x + 5) / 5);
        AddVectorObs((this.transform.position.x - 5) / 5);
        AddVectorObs((this.transform.position.z + 5) / 5);
        AddVectorObs((this.transform.position.z - 5) / 5);
        
        // Agent velocity
        AddVectorObs(rBody.velocity.x / 5);
        AddVectorObs(rBody.velocity.z / 5);
        // */
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // Fell off platform
        if (this.transform.position.y < -1.0)
        {
            AddReward(-15.0f);
            Done();
        }
        
        // Rewards by user 
        if (Input.GetKeyDown("right"))
        {
            AddReward(50.0f);
        }

        if (Input.GetKeyDown("left"))
        {
            AddReward(-50.0f);
        }

        // Distance
        float distanceToTarget = Vector3.Distance(this.transform.position, projectionPoint());

        // Reached target
        if (distanceToTarget < 1.0f)
        {
            AddReward(15.0f);
            Done();
        }

        // /*
        // Getting closer
        if (distanceToTarget < previousDistance)
        {
            AddReward(1 / (distanceToTarget + 1));
        }
        else
        {
            AddReward(-0.9f / (distanceToTarget + 1));
        }
        previousDistance = distanceToTarget;

        // Time penalty
        AddReward(-0.05f);
        // */

        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = Mathf.Clamp(vectorAction[0], -1, 1);
        controlSignal.z = Mathf.Clamp(vectorAction[1], -1, 1);
        updateText(controlSignal);
        rBody.AddForce(controlSignal * speed);
        
    }

    private Vector3 relativePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance = 1000;
        Vector3 relativePosition = Vector3.zero;
        if (plane.Raycast(ray, out distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            Vector3 projectionPoint = new Vector3(hitPoint.x, 0.0f, hitPoint.z);
            relativePosition = projectionPoint - transform.position;
        }
        return relativePosition;
    }

    private Vector3 projectionPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance = 1000;
        Vector3 hitPoint = Vector3.zero;
        Vector3 projectionPoint = Vector3.zero;
        if (plane.Raycast(ray, out distance))
        {
            hitPoint = ray.GetPoint(distance);
            projectionPoint = new Vector3(hitPoint.x, 0.0f, hitPoint.z);
        }
        return projectionPoint;
    }

    private void updateText(Vector3 controlSignal)
    {
        Vector3 relPos = relativePos();
        if (Math.Sign(relPos.x) == Math.Sign(controlSignal.x) && Math.Sign(relPos.z) == Math.Sign(controlSignal.z))
        {
            text.text = "REWARD";
            text.color = yesColor;
        }
        else
        {
            text.text = "PUNISH";
            text.color = noColor;
        }
    }
}
