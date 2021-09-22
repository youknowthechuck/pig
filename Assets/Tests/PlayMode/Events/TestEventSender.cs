using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tests
{
    public class TestEventSender : MonoBehaviour
    {
        public void SendEvent(int payload, GameObject target)
        {
            TestEvent e = new TestEvent();
            e.payload = payload;

            EventCore.SendTo(this, target, e);
        }
    }
}