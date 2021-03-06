using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class CubicInterpUtils
{
    private struct GaussLengendreCoefficient
    {
        public float abscissa; // xi
        public float weight;   // wi
    };

    //http://paulbourke.net/miscellaneous/interpolation/
    public static Vector3 Eval_Hermite(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t, float tension, float bias)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        Vector3 c0 = (p1 - p0) * (1 + bias) * (1 - tension) / 2;
                c0 += (p2 - p1) * (1 - bias) * (1 - tension) / 2;
        Vector3 c1 = (p2 - p1) * (1 + bias) * (1 - tension) / 2;
                c1 += (p3 - p2) * (1 - bias) * (1 - tension) / 2;

        float ti0 = 2 * t3 - 3 * t2 + 1;
        float ti1 =     t3 - 2 * t2 + t;
        float ti2 =     t3 - t2;
        float ti3 = -2 * t3 + 3 * t2;

        return (ti0 * p1 + ti1 * c0 + ti2 * c1 + ti3 * p2);
    }

    public static Vector3 Eval_Tangent_Hermite(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t, float tension, float bias)
    {
        float t2 = t * t;

        Vector3 c0 = (p1 - p0) * (1 + bias) * (1 - tension) / 2;
                c0 += (p2 - p1) * (1 - bias) * (1 - tension) / 2;
        Vector3 c1 = (p2 - p1) * (1 + bias) * (1 - tension) / 2;
                c1 += (p3 - p2) * (1 - bias) * (1 - tension) / 2;

        //first derivatives of coefficients above
        float dti0 = 6 * t2 - 6 * t;
        float dti1 = 3 * t2 - 4 * t + 1;
        float dti2 = 3 * t2 - 2 * t;
        float dti3 = -6 * t2 + 6 * t;

        return (dti0 * p1 + dti1 * c0 + dti2 * c1 + dti3 * p2);
    }

    //closest spline point is non-trivial, I guess. Heres how this approximation works:
    //1) treat each segment as a straight line, project worldLocation onto that line segment.
    //2) use that projection to estimate the t value that will be closest to spline on that segment
    //3) lookup actual spline value at t to adjust for curvature, find closest spline segment to location
    //4) return closest of all the found segment points
    //
    //not guaranteed accurate but should be good enough for government work
    public static Vector3 Closest_Point(Vector3 worldLocation, PathObject path)
    {
        (float, Vector3) closestSegment = (Mathf.Infinity, Vector3.zero);

        for (int i = 0; i < path.Nodes.Count - 1; ++i)
        {
            PathNode point = path.Nodes[i];
            PathNode next = path.GetNextNode(point);

            Vector3 segment = next.m_transform.position - point.m_transform.position;

            Vector3 to = worldLocation - point.m_transform.position;

            float toDot = Vector3.Dot(to, segment.normalized) / segment.magnitude;

            PathNode prev = path.GetPreviousNode(point);
            PathNode nextnext = path.GetNextNode(next);

            Vector3 pathPoint = Eval_Hermite(prev.m_transform.position, point.m_transform.position, next.m_transform.position, nextnext.m_transform.position, Mathf.Clamp(toDot, 0.0f, 1.0f), 0.0f, 0.0f);
            
            float d = (pathPoint - worldLocation).sqrMagnitude;
            
            if (d < closestSegment.Item1)
            {
                closestSegment = (d, pathPoint);
            }
        }

        return closestSegment.Item2;
    }

    //https://medium.com/@all2one/how-to-compute-the-length-of-a-spline-e44f5f04c40
    public static float Compute_Length(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        //@todo: i dont know if this actually works, i think the basis functions is different from the one above
        Vector3 c0 = p0;
        Vector3 c1 = 6.0f * (p2 - p1) - 4.0f * p0 - 2.0f * p3;
        Vector3 c2 = 6.0f * (p1 - p2) + 3.0f * (p0 + p3);
        
        GaussLengendreCoefficient[] gauss_lengendre_coefficients =
        {
            new GaussLengendreCoefficient{ abscissa = 0.0f,         weight = 0.5688889f },
            new GaussLengendreCoefficient{ abscissa = -0.5384693f,  weight = 0.47862867f },
            new GaussLengendreCoefficient{ abscissa = 0.5384693f,   weight = 0.47862867f },
            new GaussLengendreCoefficient{ abscissa = -0.90617985f, weight = 0.23692688f },
            new GaussLengendreCoefficient{ abscissa = 0.90617985f,  weight = 0.23692688f }
        };

        float length = 0.0f;
        foreach (GaussLengendreCoefficient coefficient in gauss_lengendre_coefficients)
        {
            float t = 0.5f * (1.0f + coefficient.abscissa); // This and the final (0.5 *) below are needed for a change of interval to [0, 1] from [-1, 1]
            length += (c0 + t * (c1 + t * c2)).magnitude * coefficient.weight;
        }

        return 0.5f * length;
    }
}
