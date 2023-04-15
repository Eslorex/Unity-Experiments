using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset; 

    void Update()
    {
        transform.position = target.position + offset; 
        transform.LookAt(target); 
    }
}
