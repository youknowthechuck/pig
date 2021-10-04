/* ----------------------------------------------------------------------------
  Copyright (c) Chuck Martin and Connor Hollis. All Rights Reserved.
---------------------------------------------------------------------------- */

// Bootstrap Bill you're a liar and will spend an eternity aboard this ship!
public class Bootstrap : SingletonBehaviour<Bootstrap>
{
    private void OnEnable()
    {
        DontDestroyOnLoad(this);

        gameObject.AddComponent<GameCore>();
    }
}
