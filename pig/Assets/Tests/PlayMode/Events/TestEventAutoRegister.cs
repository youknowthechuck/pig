/* ----------------------------------------------------------------------------
  Copyright (c) Chuck Martin and Connor Hollis. All Rights Reserved.
---------------------------------------------------------------------------- */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tests
{
    public class TestEventAutoRegister : PigScript
    {
        public int eventPayloadValue = 0;

        [AutoRegisterEvent]
        void ListenMethod(TestEvent e)
        {
            eventPayloadValue = e.payload;
        }
    }
}