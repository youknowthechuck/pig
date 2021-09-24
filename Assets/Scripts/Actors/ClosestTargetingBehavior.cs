using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestTargetingBehavior : TargetingBehavior
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override TargetBase FindTarget(float searchRadius)
    {
        TargetBase nearestTarget = null;
        float nearestDistance = Mathf.Infinity;

        float rangeSqr = searchRadius * searchRadius;

        GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag("Target");

        foreach (GameObject potential in potentialTargets)
        {
            float distance = (potential.transform.position - transform.position).sqrMagnitude;

            if (distance < nearestDistance && distance <= rangeSqr)
            {
                nearestTarget = potential.GetComponent<TargetBase>();
                nearestDistance = distance;
            }
        }

        return nearestTarget;
    }
}

