using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEventAutoRegister : PigScript
{
    public int eventPayloadValue = 0;

    [AutoRegisterEvent]
    void ListenMethod(TestEvent e)
    {
        eventPayloadValue = e.payload;
    }
}
