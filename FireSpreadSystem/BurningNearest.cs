using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
