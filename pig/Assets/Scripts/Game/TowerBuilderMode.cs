/* ----------------------------------------------------------------------------
  Copyright (c) Chuck Martin and Connor Hollis. All Rights Reserved.
---------------------------------------------------------------------------- */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilderMode : PigScript
{
    private GameObject m_towerPrefab = null;
    private GameObject m_previewInstance = null;
    private bool m_isTowerBuilderEnabled = false;
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
    }

    public void PositionPreviewAt(Vector3 position, Quaternion rotation)
    {
        if (m_previewInstance == null)
        {
            throw new InvalidOperationException("No preview to position!");
        }

        m_previewInstance.transform.SetPositionAndRotation(position, rotation);
    }

    public bool ValidateTowerPosition()
    {
        return true;
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

        if (Physics.Raycast(ray, out hit))
        {
            PositionPreviewAt(hit.point, Quaternion.identity);
        }

        // Wow this is stupid and hacky who would ever hard code a mouse input directly in code???
        if(Input.GetMouseButtonDown(0) && ValidateTowerPosition())
        {
            //GameObject placedObject = Instantiate(m_towerPrefab, hit.point, Quaternion.identity);
            //jank
            GameObject player = GameObject.Find("Player");
            GameObject placedObject = Instantiate(m_towerPrefab, hit.point, Quaternion.identity, player.transform);//GlobalUtil.InstantiateWithOwner(m_towerPrefab, hit.point, Quaternion.identity, player );

            //this is a shitty hack to stop the placer prefab from flying into space..
            placedObject.GetComponent<CapsuleCollider>().enabled = true;

            TowerPlacedEvent e = new TowerPlacedEvent();
            e.Tower = placedObject;

            EventCore.SendTo<TowerPlacedEvent>(this, placedObject, e);

            DisableBuilderMode();
        }
        //hack, config this
        else if (Input.GetMouseButtonDown(1))
        {
            DisableBuilderMode();
        }
    }
}
