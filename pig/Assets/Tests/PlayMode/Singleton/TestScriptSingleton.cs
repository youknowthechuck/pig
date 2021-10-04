/* ----------------------------------------------------------------------------
  Copyright (c) Chuck Martin and Connor Hollis. All Rights Reserved.
---------------------------------------------------------------------------- */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScriptSingleton : SingletonBehaviour<TestScriptSingleton>
{
    public TestClassSingletonState state;

    protected override void OnInitialize()
    {
        state = TestClassSingletonState.Initialized;
    }

    protected override void OnTeardown()
    {
        state = TestClassSingletonState.TornDown;
    }
}
