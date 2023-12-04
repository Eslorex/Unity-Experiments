using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
