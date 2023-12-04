using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Script Purpose: This script is responsible for handling the spreading of a 'burning' effect among nearby game objects in a Unity scene.
// 'BurningNearest' is attached to game objects, marking them as 'burnable'. When enabled, it searches for the nearest burnable object within a specified range and applies the same burning effect to it after a delay.
//
// Public Variables:
// - burnTime (float): Duration in seconds before the game object destroys itself.
// - spreadRange (float): The maximum range within which the script can affect other burnable objects.
// - ignitionDelay (float): The delay in seconds before igniting the nearest burnable object.
//
// Private Variables:
// - alreadyIgnited (HashSet<GameObject>): A collection of game objects that have already been ignited to prevent re-ignition.
//
// Methods:
// - Start(): Initializes the object's material to red when the script starts.
// - Update(): Continuously checks and starts the ignition process of nearby burnable objects.
// - FindAndStartIgnition(): Finds the nearest burnable object and triggers the ignition process.
// - StartBurningAfterDelay(GameObject): Starts a coroutine to ignite an object after a specified delay.
// - OnEnable(): Destroys the game object after 'burnTime' seconds when the script is enabled.
// - OnDrawGizmosSelected(): Draws a wire sphere in the editor to visualize the spread range.
//
// Notes:
// - Sphere version of this script is suppose to be a lot more efficient. 
// - The script uses tags to identify burnable objects.
// - It uses a coroutine for delayed ignition and HashSet to track ignited objects.

public class BurningNearest : MonoBehaviour
{
    public float burnTime = 10f;
    public float spreadRange = 15f;
    public float ignitionDelay = 0.4f;
    private HashSet<GameObject> alreadyIgnited = new HashSet<GameObject>();

    private void Start()
    {
        if (GetComponent<Renderer>())
        {
            Material redMaterial = new Material(Shader.Find("Standard"));
            redMaterial.color = Color.red;
            GetComponent<Renderer>().material = redMaterial;
        }

       // Debug.Log(gameObject.name + " BurningNearest script started.");
    }

    private void Update()
    {
        FindAndStartIgnition();
    }

    private void FindAndStartIgnition()
    {
        float nearestDistanceSqr = spreadRange * spreadRange;
        GameObject nearestBurnableObject = null;

        GameObject[] burnableObjects = GameObject.FindGameObjectsWithTag("Burnable");
        //Debug.Log(gameObject.name + " found " + burnableObjects.Length + " burnable objects.");

        foreach (var obj in burnableObjects)
        {
            if (!alreadyIgnited.Contains(obj) && obj != gameObject)
            {
                float distanceSqr = (transform.position - obj.transform.position).sqrMagnitude;
                if (distanceSqr < nearestDistanceSqr)
                {
                    nearestDistanceSqr = distanceSqr;
                    nearestBurnableObject = obj;
                }
            }
        }

        if (nearestBurnableObject != null)
        {
            //Debug.Log(gameObject.name + " found nearest burnable object: " + nearestBurnableObject.name);
            if (nearestBurnableObject.GetComponent<BurningNearest>() == null)
            {
                StartCoroutine(StartBurningAfterDelay(nearestBurnableObject));
                alreadyIgnited.Add(nearestBurnableObject);
               // Debug.Log("Coroutine started for " + nearestBurnableObject.name);
            }
            else
            {
               // Debug.Log(nearestBurnableObject.name + " already has BurningNearest component.");
            }
        }
        else
        {
            //Debug.Log(gameObject.name + " did not find a suitable object to ignite.");
        }
    }

    private IEnumerator StartBurningAfterDelay(GameObject objectToIgnite)
    {
        //Debug.Log("Waiting to ignite " + objectToIgnite.name);
        yield return new WaitForSeconds(ignitionDelay);

        if (objectToIgnite != null && objectToIgnite.GetComponent<BurningNearest>() == null)
        {
            //Debug.Log("Igniting " + objectToIgnite.name);
            objectToIgnite.AddComponent<BurningNearest>();
        }
        else
        {
            //Debug.Log("Ignition canceled or object already ignited: " + objectToIgnite.name);
        }
    }

    private void OnEnable()
    {
        //Debug.Log(gameObject.name + " is enabled and will be destroyed in " + burnTime + " seconds");
        Destroy(gameObject, burnTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spreadRange);
    }
}
