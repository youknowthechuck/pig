using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamLifetimeTravelToTargetState : ProjectileLifetimeTravelToTarget
{
    //fuck this is dumb
    private Vector3 defaultLocalScale = new Vector3(0.3f, 1.0f, 1.0f);
    private float defaultLossyScaleZ = 12.0f;

    void OnTriggerEnter(Collider other)
    {
        //m_hitTarget = other.gameObject.GetComponent<TargetBase>();
    }

    public override void Enter(object[] input)
    {

        m_target = input[0] as GameObject;
        Debug.Assert(m_target != null);
        Debug.Assert(m_projectileObject != null);

        //local cord space to target
        Vector3 dir = (m_target.transform.position - m_projectileObject.transform.position).normalized;
        Vector3 left = Vector3.Cross(Vector3.up, dir);
        Vector3 localUp = Vector3.Cross(dir, left);

        float scaleZ = (m_target.transform.position - m_projectileObject.transform.position).magnitude;
        Vector3 localScale = defaultLocalScale;// m_projectileObject.transform.localScale;
        //the default plane is 10x10 so scale the beam to be the exact length to the target
        localScale.z = scaleZ / (10.0f * defaultLossyScaleZ/*m_projectileObject.transform.lossyScale.z*/);

        Vector3 mid = m_projectileObject.transform.position + dir * (scaleZ / 2.0f);

        m_projectileObject.transform.forward = dir;
        m_projectileObject.transform.position = mid;
        m_projectileObject.transform.localScale = localScale;

        m_projectileObject.Show();

        //assume we hit the target cause we're beaming
        m_hitTarget = m_target.GetComponent<TargetBase>();
    }

    public override void Tick()
    {
    }
}
