using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BurningNearestSphere : MonoBehaviour
{
    // Public variables
    public float burnTime = 100f;
    public float spreadRange = 15f;
    public float ignitionDelay = 0f;
    public RuntimeAnimatorController animatorController;

    // Private variables
    private Animator animator;
    private List<GameObject> objectsInRange = new List<GameObject>();

    // Unity Methods
    private void Start()
    {
        InitializeBurningSphere();
        StartCoroutine(SpreadFire());
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

    // Initialization Methods
    private void InitializeBurningSphere()
    {
        SetupAnimator();
        SetupMaterial();
    }

    // Just a visual cue to see which objects are burning
    private void SetupAnimator()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            animator = gameObject.AddComponent<Animator>();
        }

        if (animatorController != null)
        {
            animator.runtimeAnimatorController = animatorController;
            animator.applyRootMotion = true;
            animator.Play("Move");
        }
        else
        {
            Debug.LogError("Animator Controller not assigned.");
        }
    }

    // Just a visual cue to see which objects are burning
    private void SetupMaterial()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            Material redMaterial = new Material(Shader.Find("Standard"));
            redMaterial.color = Color.red;
            renderer.material = redMaterial;
        }
    }

    // Fire Spreading Methods
    private IEnumerator SpreadFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);
            SpreadToNearbyObjects();
            RemoveOutOfRangeObjects();
        }
    }

    private void SpreadToNearbyObjects()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, spreadRange, LayerMask.GetMask("Burnable"));
        foreach (var hitCollider in hitColliders)
        {
            if (!objectsInRange.Contains(hitCollider.gameObject))
            {
                objectsInRange.Add(hitCollider.gameObject);
                StartCoroutine(StartBurningAfterDelay(hitCollider.gameObject));
            }
        }
    }

    private void RemoveOutOfRangeObjects()
    {
        objectsInRange.RemoveAll(obj => !obj || Vector3.Distance(transform.position, obj.transform.position) > spreadRange);
    }

    private IEnumerator StartBurningAfterDelay(GameObject obj)
    {
        yield return new WaitForSeconds(ignitionDelay);

        if (objectsInRange.Contains(obj) && obj.GetComponent<BurningNearestSphere>() == null)
        {
            SetupNewBurningSphere(obj);
        }
    }

    private void SetupNewBurningSphere(GameObject obj)
    {
        var burningSphere = obj.AddComponent<BurningNearestSphere>();
        burningSphere.CopyPropertiesFrom(this);
        burningSphere.InitializeBurningSphere();

        Destroy(obj, burnTime);
    }


    // Utility Methods
    public void CopyPropertiesFrom(BurningNearestSphere original)
    {
        burnTime = original.burnTime;
        spreadRange = original.spreadRange;
        ignitionDelay = original.ignitionDelay;
        animatorController = original.animatorController;
    }
}
