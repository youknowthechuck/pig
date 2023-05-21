using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowestHealthTargetingBehavior : TargetingBehavior
{
    // Start is called before the first frame update
    void Start()
    {
        PrettyName = "Lowest Health";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override TargetBase FindTarget(float searchRadius)
    {
        TargetBase weakestTarget = null;
        int minLife = int.MaxValue;

        float rangeSqr = searchRadius * searchRadius;

        //@todo - is this faster than objects with tag "Target"? does it matter?
        TargetBase[] potentialTargets = GameObject.FindObjectsOfType<TargetBase>();

        //@todo: tie breaker?
        foreach (TargetBase potential in potentialTargets)
        {
            float distance = (potential.transform.position - transform.position).sqrMagnitude;

            if (distance <= rangeSqr && potential.GetCurrentHealthTotal() < minLife)
            {
                weakestTarget = potential;
                minLife = potential.GetCurrentHealthTotal();
            }
        }

        return weakestTarget;
    }
}
