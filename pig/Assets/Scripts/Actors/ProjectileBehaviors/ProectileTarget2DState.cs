using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProectileTarget2DState : ProjectileLifetimeTravelToTarget
{
    //move towards target location in r2
    private Vector3 m_direction;

    public override void Enter(object[] input)
    {
        m_target = input[0] as GameObject;
        Debug.Assert(m_target != null);
        Debug.Assert(m_projectileObject != null);

        //2d targeting assumes everything happens on the same Y plane
        PathFollower pathTarget = m_target.GetComponent<PathFollower>();
        if (pathTarget != null)
        {
            //assume that the time to time from shot origin to target doesn't change as the target moves along the path, because predicting that would be really hard
            Vector3 vectorToTarget = m_target.transform.position - m_projectileObject.transform.position;

            float distToTarget = vectorToTarget.sqrMagnitude;

            float timeToTarget = distToTarget / (m_projectileObject.Speed * m_projectileObject.Speed);

            Vector3 prediction = pathTarget.EvalPosition(pathTarget.LifeTime + timeToTarget);
            prediction.y = m_projectileObject.transform.position.y;

            m_direction = (prediction - m_projectileObject.transform.position).normalized;
        }
        else
        {
            Vector3 targetLoc = m_target.transform.position;
            targetLoc.y = m_projectileObject.transform.position.y;

            m_direction = (m_target.transform.position - m_projectileObject.transform.position).normalized;
        }
    }

    public override void Tick()
    {
        Debug.Assert(m_projectileObject != null);

        m_projectileObject.transform.position += m_direction * m_projectileObject.Speed * Time.deltaTime;

        //if the projectile is some distance away from the origin beyond this arbitrary number, kill it cause its far away
        const double KILL_THRESHOLD = 1e08;

        if (m_projectileObject.transform.position.sqrMagnitude > KILL_THRESHOLD)
        {
            m_timedOut = true;
        }
    }
}
