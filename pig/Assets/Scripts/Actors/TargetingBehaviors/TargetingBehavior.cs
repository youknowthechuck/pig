using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingBehavior : PigScript
{
    [HideInInspector]
    public string PrettyName = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual TargetBase FindTarget(float searchRadius)
    {
        //default implementation, you get nothing good day sir
        return null;
    }
}
