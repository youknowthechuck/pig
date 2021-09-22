using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEventBroadcaster : MonoBehaviour
{
    public void BroadcastEvent(int payload)
    {
        TestEvent e = new TestEvent();
        e.payload = payload;

        EventCore.BroadcastEvent(this, e);
    }
}
