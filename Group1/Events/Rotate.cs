using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotationSpeed = 100f; 

    void Update()
    {
        transform.Rotate(new Vector3(1.0f, 0.0f, 1.0f) * Time.deltaTime * rotationSpeed, Space.World); 
    }
}
