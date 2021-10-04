/* ----------------------------------------------------------------------------
  Copyright (c) Chuck Martin and Connor Hollis. All Rights Reserved.
---------------------------------------------------------------------------- */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tests
{
    public class TestEventListener : MonoBehaviour
    {
        public int eventPayloadValue = 0;

        // Start is called before the first frame update
        void OnEnable()
        {
            EventCore.AddListener<TestEvent>(ListenMethod);
        }

        void OnDisable()
        {
            EventCore.RemoveListener<TestEvent>(ListenMethod);
        }

        void ListenMethod(TestEvent e)
        {
            eventPayloadValue = e.payload;
        }
    }
}