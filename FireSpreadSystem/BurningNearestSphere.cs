using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Script Purpose: 'BurningNearestSphere' is attached to GameObjects in Unity to simulate a fire spreading effect within a spherical range.
// It identifies and applies the burning effect to nearby objects tagged as 'Burnable' repeatedly within its range. 
//
// Public Variables:
// - burnTime (float): Time in seconds after which the GameObject this script is attached to will be destroyed.
// - spreadRange (float): Radius of the sphere within which the script checks for other burnable objects.
// - ignitionDelay (float): Delay in seconds before igniting each object within range (can be set to zero for instant ignition).
//
// Private Variables:
// - objectsInRange (List<GameObject>): Tracks all GameObjects within the spread range that have been or are to be ignited.
//
// Methods:
// - Start(): Sets the GameObject's material to red and starts the SpreadFire coroutine.
// - SpreadFire(): A continuous loop that periodically checks for burnable objects within range and starts ignition.
// - StartBurningAfterDelay(GameObject): Initiates a coroutine to add the 'BurningNearestSphere' component to objects within range after the specified delay.
// - OnEnable(): Destroys the GameObject after the specified 'burnTime'.
// - OnDrawGizmosSelected(): Visually represents the spread range in the editor using a red wire sphere.
//
// Notes:
// - This script uses Physics.OverlapSphere to find objects within the specified range and a LayerMask to filter for 'Burnable' objects.
// - Objects are added to 'objectsInRange' when they enter the range and removed when they exit or are destroyed.
// - The script ensures that the burning effect is only applied once per object by checking if 'BurningNearestSphere' is already attached.

public class BurningNearestSphere : MonoBehaviour
{
    public float burnTime = 6f; 
    public float spreadRange = 7f; 
    public float ignitionDelay = 0f; 

    private List<GameObject> objectsInRange = new List<GameObject>();

    private void Start()
    {
        if (GetComponent<Renderer>())
        {
            Material redMaterial = new Material(Shader.Find("Standard"));
            redMaterial.color = Color.red;
            GetComponent<Renderer>().material = redMaterial;
        }

        StartCoroutine(SpreadFire());
    }

    private IEnumerator SpreadFire()
    {
        while (true)
        {
            // Wait for a fixed time interval before checking again
            yield return new WaitForSeconds(0.25f);

            // Check for nearby burnable objects within the spreadRange
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, spreadRange, LayerMask.GetMask("Burnable"));
            foreach (var hitCollider in hitColliders)
            {
                // Add new objects to the list to track if they stay in range
                if (!objectsInRange.Contains(hitCollider.gameObject))
                {
                    objectsInRange.Add(hitCollider.gameObject);
                    StartCoroutine(StartBurningAfterDelay(hitCollider.gameObject));
                }
            }

            // Remove objects that are out of range
            objectsInRange.RemoveAll(obj => !obj || Vector3.Distance(transform.position, obj.transform.position) > spreadRange);
        }
    }

    private IEnumerator StartBurningAfterDelay(GameObject obj)
    {
        yield return new WaitForSeconds(ignitionDelay);

        if (objectsInRange.Contains(obj) && obj.GetComponent<BurningNearestSphere>() == null)
        {
            obj.AddComponent<BurningNearestSphere>();
        }
    }

    private void OnEnable()
    {
        Destroy(gameObject, burnTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spreadRange);
    }
}
