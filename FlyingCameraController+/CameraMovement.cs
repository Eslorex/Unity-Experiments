using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private float sprintMultiplier = 4f; 
    [SerializeField] private float lookSensitivity = 1f; 
    [SerializeField] private float damping = 5f; 

    private Vector3 velocity; 
    private bool cursorLocked = true; 

    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnlockCursor();
        }
        else if (!cursorLocked && Input.GetMouseButtonDown(0))
        {
            LockCursor();
        }

        if (cursorLocked)
        {
            UpdateCameraPosition();
        }

        UpdateCameraRotation();
    }

    private void UpdateCameraPosition()
    {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        Vector3 accelerationVector = transform.TransformDirection(moveInput) * moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            accelerationVector *= sprintMultiplier;
        }

        velocity = Vector3.Lerp(velocity, accelerationVector, Time.deltaTime);

        if (Input.GetKey(KeyCode.Space))
        {
            velocity += Vector3.up * moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            velocity += Vector3.down * moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            velocity += Vector3.up * moveSpeed * Time.deltaTime;
        }

        // Update camera position
        transform.position += velocity * Time.deltaTime;

        // Apply damping to velocity
        velocity = Vector3.Lerp(velocity, Vector3.zero, damping * Time.deltaTime);
    }

    private void UpdateCameraRotation()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        float rotationX = transform.localEulerAngles.y + mouseX * lookSensitivity;
        float rotationY = transform.localEulerAngles.x - mouseY * lookSensitivity;
        transform.localEulerAngles = new Vector3(rotationY, rotationX, 0f);
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cursorLocked = true;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cursorLocked = false;
    }
}
