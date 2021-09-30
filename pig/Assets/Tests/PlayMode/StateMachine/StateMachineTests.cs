using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class StateMachineTests
    {
        [UnityTest]
        public IEnumerator StateTransitions()
        {
            GameObject stateMachineObject = new GameObject();
            TestStateMachineComponent stateMachine = stateMachineObject.AddComponent<TestStateMachineComponent>();

            yield return new WaitForSeconds(0.1f);

            Assert.AreEqual(stateMachine.stateMachine.CurrentState.GetType(), typeof(TestStateA));

            yield return new WaitForSeconds(1.05f);

            Assert.AreEqual(stateMachine.stateMachine.CurrentState.GetType(), typeof(TestStateB));

            yield return new WaitForSeconds(1.05f);

            Assert.AreEqual(stateMachine.stateMachine.CurrentState.GetType(), typeof(TestStateC));

            yield return new WaitForSeconds(1.05f);

            Assert.AreEqual(stateMachine.stateMachine.CurrentState.GetType(), typeof(TestStateA));

            yield return null;
        }
    }
}