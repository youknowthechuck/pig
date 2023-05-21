using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontTargetingBehavior : TargetingBehavior
{
    // Start is called before the first frame update
    void Start()
    {
        PrettyName = "Front";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override TargetBase FindTarget(float searchRadius)
    {
        TargetBase frontTarget = null;
        float targetLifeTime = float.MinValue;

        float rangeSqr = searchRadius * searchRadius;

        //@todo - is this faster than objects with tag "Target"? does it matter?
        TargetBase[] potentialTargets = GameObject.FindObjectsOfType<TargetBase>();

        foreach (TargetBase potential in potentialTargets)
        {
            float distance = (potential.transform.position - transform.position).sqrMagnitude;

            if (distance <= rangeSqr)
            {
                PathFollower path = potential.GetComponent<PathFollower>();

                if (path != null && path.LifeTimeNormalized > targetLifeTime)
                {
                    frontTarget = potential;
                    targetLifeTime = path.LifeTimeNormalized;
                }
            }
        }

        return frontTarget;
    }
}
