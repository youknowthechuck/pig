/* ----------------------------------------------------------------------------
  Copyright (c) Chuck Martin and Connor Hollis. All Rights Reserved.
---------------------------------------------------------------------------- */

public class TestClassSingleton : SingletonClass<TestClassSingleton>
{
    public TestClassSingletonState state;

    protected override void Initialize()
    {
        state = TestClassSingletonState.Initialized;
    }

    protected override void Teardown()
    {
        state = TestClassSingletonState.TornDown;
    }
}
