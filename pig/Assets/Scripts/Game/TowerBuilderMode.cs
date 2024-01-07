/* ----------------------------------------------------------------------------
  Copyright (c) Chuck Martin and Connor Hollis. All Rights Reserved.
---------------------------------------------------------------------------- */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class TowerBuilderMode : PigScript
{
    private GameObject m_towerPrefab = null;
    private GameObject m_previewInstance = null;
    private bool m_isTowerBuilderEnabled = false;

    [SerializeField]
    private Material m_ghostMaterial = null;

    public GameObject TowerPrefab
    {
        get { return m_towerPrefab; }
    }

    public GameObject PreviewInstance
    {
        get { return m_previewInstance; }
    }

    public bool IsBuilderModeEnabled
    {
        get { return m_isTowerBuilderEnabled; }
    }

    public void AssignTowerPrefab(GameObject prefab)
    {
        m_towerPrefab = prefab;
    }

    public void EnableBuilderMode()
    {
        m_isTowerBuilderEnabled = true;
    }

    public void DisableBuilderMode()
    {
        m_isTowerBuilderEnabled = false;
        
        if(m_previewInstance)
        {
            Destroy(m_previewInstance);
        }
    }

    public void CreatePreviewInstance()
    {
        if(m_towerPrefab == null)
        {
            throw new InvalidOperationException("No prefab to create preview instance from!");
        }

        if(m_previewInstance)
        {
            Destroy(m_previewInstance);
        }

        m_previewInstance = Instantiate(m_towerPrefab);

        TowerPlacementGhost newGhost = m_previewInstance.AddComponent<TowerPlacementGhost>();
        newGhost.SetDisplayMaterial(m_ghostMaterial);
    }

    public void PositionPreviewAt(Vector3 position, Quaternion rotation)
    {
        if (m_previewInstance == null)
        {
            throw new InvalidOperationException("No preview to position!");
        }

        m_previewInstance.transform.SetPositionAndRotation(position, rotation);
    }

    public bool ValidateTowerPosition(Vector3 pos)
    {
        bool overlap = false;

        foreach (PathObject path in FindObjectsOfType<PathObject>())
        {
            Vector3 nearest = CubicInterpUtils.Closest_Point(pos, path);
            //@todo: does per node width make sense?
            float distance = (nearest - pos).magnitude;

            overlap |= distance <= (path.Nodes[0].m_width + m_towerPrefab.GetComponent<CapsuleCollider>().radius);
        }

        overlap |= (m_previewInstance.GetComponent<TowerPlacementGhost>().OverlapCount > 0);

        return !overlap;
    }

    private void Update()
    {
        if(!IsBuilderModeEnabled)
        {
            return;
        }


        if (m_towerPrefab && !m_previewInstance)
        {
            CreatePreviewInstance();
        }

        if(!m_previewInstance)
        {
            return;
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        int terrainMask = 1 << 3; //3 == ground collision layer...

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainMask))
        {
            PositionPreviewAt(hit.point, Quaternion.identity);
        }

        bool valid = ValidateTowerPosition(hit.point);

        m_previewInstance.GetComponent<TowerPlacementGhost>().SetValidity(valid ? 1.0f : 0.0f);

        // Wow this is stupid and hacky who would ever hard code a mouse input directly in code???
        if (Input.GetMouseButtonDown(0) && valid)
        {
            //jank
            GameObject player = GameObject.Find("Player");
            GameObject placedObject = Instantiate(m_towerPrefab, hit.point, Quaternion.identity, player.transform);

            SendEvent(new TowerPlacedEvent() { Tower = placedObject }, placedObject);

            DisableBuilderMode();
        }
        //hack, config this
        else if (Input.GetMouseButtonDown(1))
        {
            DisableBuilderMode();
        }
    }
}
