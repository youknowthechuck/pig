/* ----------------------------------------------------------------------------
  Copyright (c) Chuck Martin and Connor Hollis. All Rights Reserved.
---------------------------------------------------------------------------- */

using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests
{
    /// <summary>
    /// Runs all of the singleton class validation tests.
    /// Remember to manually teardown the current instance with StaticDestroy at the start of the test. In case other tests left the singleton in a bad state.
    /// </summary>
    public class SingletonClassTests
    {
        [UnityTest]
        public IEnumerator SingletonClassInstanceAccessor()
        {
            if(TestClassSingleton.HasInstance)
            {
                TestClassSingleton.StaticDestroy();
            }

            TestClassSingletonState state = TestClassSingleton.Instance.state;
            Assert.AreEqual(state, TestClassSingletonState.Initialized);

            TestClassSingleton.StaticDestroy();

            Assert.IsFalse(TestClassSingleton.HasInstance);

            yield return null;
        }

        [UnityTest]
        public IEnumerator SingletonClassStaticCreateAndDestroy()
        {
            if (TestClassSingleton.HasInstance)
            {
                TestClassSingleton.StaticDestroy();
            }

            TestClassSingleton.StaticConstruct();

            Assert.True(TestClassSingleton.HasInstance);

            Assert.AreEqual(TestClassSingleton.Instance.state, TestClassSingletonState.Initialized);

            TestClassSingleton.StaticDestroy();

            Assert.IsFalse(TestClassSingleton.HasInstance);

            yield return null;
        }

        [UnityTest]
        public IEnumerator SingletonClassTestDestroy()
        {
            if (TestClassSingleton.HasInstance)
            {
                TestClassSingleton.StaticDestroy();
            }

            TestClassSingleton singleton = new TestClassSingleton();

            Assert.True(TestClassSingleton.HasInstance);

            Assert.AreEqual(singleton.state, TestClassSingletonState.Initialized);

            TestClassSingleton.StaticDestroy();

            Assert.AreEqual(singleton.state, TestClassSingletonState.TornDown);

            yield return null;
        }

        [UnityTest]
        public IEnumerator SingletonClassTestAssertOnMultiNew()
        {
            if (TestClassSingleton.HasInstance)
            {
                TestClassSingleton.StaticDestroy();
            }

            TestClassSingleton singleton1 = new TestClassSingleton();

            Assert.Throws<InvalidOperationException>(
                delegate 
                {
                    new TestClassSingleton();
                }
            );

            yield return null;
        }

        [UnityTest]
        public IEnumerator SingletonClassTestAssertWithInstance()
        {
            if (TestClassSingleton.HasInstance)
            {
                TestClassSingleton.StaticDestroy();
            }

            TestClassSingletonState state = TestClassSingleton.Instance.state;
            Assert.AreEqual(state, TestClassSingletonState.Initialized);

            Assert.Throws<InvalidOperationException>(
                delegate
                {
                    new TestClassSingleton();
                }
            );

            yield return null;
        }

        [UnityTest]
        public IEnumerator SingletonClassTestAssertOnStaticConstructTheNnew()
        {
            if (TestClassSingleton.HasInstance)
            {
                TestClassSingleton.StaticDestroy();
            }

            TestClassSingleton.StaticConstruct();

            Assert.Throws<InvalidOperationException>(
                delegate
                {
                    new TestClassSingleton();
                }
            );

            yield return null;
        }
    }
}