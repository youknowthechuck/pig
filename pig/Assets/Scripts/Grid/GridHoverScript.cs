using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHoverScript : PigScript
{
    public Material gridDefaultMaterial;
    public Material gridHoverMaterial;

    public MeshRenderer targetRenderer;

    // BUGBUG: This only works with the OLD unity input system. If we ever want to use the new input action system we'll have to replace this
    void OnMouseEnter()
    {
        targetRenderer.material = gridHoverMaterial;
    }

    // BUGBUG: This only works with the OLD unity input system. If we ever want to use the new input action system we'll have to replace this
    void OnMouseExit()
    {
        targetRenderer.material = gridDefaultMaterial;
    }
}
