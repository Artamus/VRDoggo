using UnityEngine;
using System.Collections;

public class CameraOrbit : MonoBehaviour {
    public float turnSpeed = 4.0f;
    public Transform player;

    private Vector3 offset;

    void Start() {
        offset = transform.position - player.transform.position;
    }

    void LateUpdate() {
        offset = Quaternion.AngleAxis(Input.GetAxis("Horizontal") * turnSpeed, Vector3.up) * offset;
        transform.position = player.position + offset;
        transform.LookAt(player.position);
    }
}