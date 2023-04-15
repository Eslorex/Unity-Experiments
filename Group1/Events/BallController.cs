using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 10.0f; // Speed of the ball

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // Get input from the horizontal axis
        float verticalInput = Input.GetAxis("Vertical"); // Get input from the vertical axis

        Vector3 movement = new Vector3(horizontalInput, 0.0f, verticalInput); // Create a movement vector

        GetComponent<Rigidbody>().AddForce(movement * speed); // Add force to the rigidbody
    }
}
