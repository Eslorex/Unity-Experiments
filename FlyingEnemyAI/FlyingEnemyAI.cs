using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyAI : MonoBehaviour
{
    public float speed = 5.0f;
    public float obstacleDetectionDistance = 5.0f;
    public float obstacleAvoidanceForce = 20.0f;
    public LayerMask obstacleLayerMask;

    private Rigidbody rb;
  public float colorChangeSpeed = 1.0f;

    private Renderer objectRenderer;
    private float hue;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
          objectRenderer = GetComponent<Renderer>();
        hue = Random.Range(0f, 1f); // Start with a random hue
    }

    void Move(){

        transform.position += transform.forward * 10 * Time.deltaTime;
    }

    void Update()
    {
         hue += colorChangeSpeed * Time.deltaTime;
        if (hue >= 1f)
        {
            hue -= 1f;
        }

        Color newColor = Color.HSVToRGB(hue, 1f, 1f);
        objectRenderer.material.color = newColor;
        
        Vector3 forward = transform.forward;
        Vector3 upward = transform.up;
        Vector3 right = transform.right;

        RaycastHit hit;

        Vector3 avoidanceForce = Vector3.zero;
        Move();
        if (Physics.Raycast(transform.position, forward, out hit, obstacleDetectionDistance, obstacleLayerMask))
        {
            avoidanceForce += (forward * -1) * obstacleAvoidanceForce;
        }

        if (Physics.Raycast(transform.position, upward, out hit, obstacleDetectionDistance, obstacleLayerMask))
        {
            avoidanceForce += (upward * -1) * obstacleAvoidanceForce;
        }

        if (Physics.Raycast(transform.position, right, out hit, obstacleDetectionDistance, obstacleLayerMask))
        {
            avoidanceForce += (right * -1) * obstacleAvoidanceForce;
        }

        if (Physics.Raycast(transform.position, -right, out hit, obstacleDetectionDistance, obstacleLayerMask))
        {
            avoidanceForce += right * obstacleAvoidanceForce;
        }

        if (Physics.Raycast(transform.position, -upward, out hit, obstacleDetectionDistance, obstacleLayerMask))
        {
            avoidanceForce += upward * obstacleAvoidanceForce;
        }

        rb.AddForce(avoidanceForce);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);

        if (rb.velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rb.velocity), Time.deltaTime * 5.0f);
        }
        
    }
}
