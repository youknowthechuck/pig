using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacementGhost : PigScript
{
    public int OverlapCount;

    private static string m_validityMaterialParam = "_Validity";

    void OnTriggerEnter(Collider other)
    {
        OverlapCount++;
    }

    void OnTriggerExit(Collider other)
    {
        OverlapCount--;
    }
    // Start is called before the first frame update
    void Start()
    {
        OverlapCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDisplayMaterial(Material material)
    { 
        foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>())
        {
            r.material = material;
        }
    }

public void SetValidity(float validity)
    {
        foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>())
        {
            r.material.SetFloat(m_validityMaterialParam, validity);
        }
    }
}
