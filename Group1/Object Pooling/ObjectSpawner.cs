using System.Collections;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public ObjectPooler objectPooler;
    public float spawnRate = 0.1f;
    public float throwForce = 10.0f;
    public float disableAfter = 5.0f;

    private float nextSpawnTime;

    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            SpawnObject();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnObject()
    {
        GameObject obj = objectPooler.GetPooledObject();
        if (obj != null)
        {
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
            obj.SetActive(true);

            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 randomForce = new Vector3(
                    Random.Range(-throwForce / 2.0f, throwForce / 2.0f),
                    Random.Range(throwForce, throwForce * 2.0f),
                    Random.Range(-throwForce / 2.0f, throwForce / 2.0f)
                );
                rb.AddForce(randomForce);
            }

            StartCoroutine(DisableObjectAfterTime(obj, disableAfter));
        }
    }


    IEnumerator DisableObjectAfterTime(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
}
