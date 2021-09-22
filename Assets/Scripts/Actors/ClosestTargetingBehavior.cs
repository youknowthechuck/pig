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

    public override PathFollower FindTarget(float searchRadius)
    {
        PathFollower nearestTarget = null;
        float nearestDistance = Mathf.Infinity;

        float rangeSqr = searchRadius * searchRadius;

        GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag("PathFollower");

        foreach (GameObject potential in potentialTargets)
        {
            PathFollower potentialFollower = potential.GetComponent<PathFollower>();

            if (potentialFollower == null)
            {
                continue;
            }

            float distance = (potentialFollower.transform.position - transform.position).sqrMagnitude;

            if (distance < nearestDistance && distance <= rangeSqr)
            {
                nearestTarget = potentialFollower;
                nearestDistance = distance;
            }
        }

        return nearestTarget;
    }
}

