using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamLifetimeTravelToTargetState : ProjectileLifetimeTravelToTarget
{
    //default implementation - move towards target location in r3
    private Vector3 m_direction;

    void OnTriggerEnter(Collider other)
    {
        m_hitTarget = other.gameObject.GetComponent<TargetBase>();
    }

    public override void Enter(object[] input)
    {
        m_target = input[0] as GameObject;
        Debug.Assert(m_target != null);
        Debug.Assert(m_parent != null);

        //local cord space to target
        Vector3 dir = (m_target.transform.position - m_parent.transform.position).normalized;
        Vector3 left = Vector3.Cross(Vector3.up, dir);
        Vector3 localUp = Vector3.Cross(dir, left);

        float scaleZ = (m_target.transform.position - m_parent.transform.position).magnitude;
        Vector3 localScale = m_parent.transform.localScale;
        //the default plane is 10x10 so scale the beam to be the exact length to the target
        localScale.z = scaleZ / (10.0f * m_parent.transform.lossyScale.z);

        Vector3 mid = m_parent.transform.position + dir * (scaleZ / 2.0f);

        m_parent.transform.forward = dir;
        m_parent.transform.position = mid;
        m_parent.transform.localScale = localScale;

        m_parent.Show();
    }

    public override void Tick()
    {
    }
}
