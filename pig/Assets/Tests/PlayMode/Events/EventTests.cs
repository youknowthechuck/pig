using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class EventTests
    {
        [UnityTest]
        public IEnumerator SendTo()
        {
            GameObject sourceObject = new GameObject();
            TestEventSender sender = sourceObject.AddComponent<TestEventSender>();

            GameObject targetObject = new GameObject();
            TestEventListener listener = targetObject.AddComponent<TestEventListener>();

            sender.SendEvent(1000, targetObject);

            Assert.AreEqual(1000, listener.eventPayloadValue);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Broadcast()
        {
            GameObject sourceObject = new GameObject();
            TestEventBroadcaster broadcaster = sourceObject.AddComponent<TestEventBroadcaster>();

            GameObject targetObject = new GameObject();
            TestEventListener listener = targetObject.AddComponent<TestEventListener>();

            broadcaster.BroadcastEvent(1000);

            Assert.AreEqual(1000, listener.eventPayloadValue);

            yield return null;
        }

        [UnityTest]
        public IEnumerator AutoRegister()
        {
            GameObject sourceObject = new GameObject();
            TestEventSender sender = sourceObject.AddComponent<TestEventSender>();

            GameObject targetObject = new GameObject();
            TestEventAutoRegister listener = targetObject.AddComponent<TestEventAutoRegister>();

            sender.SendEvent(1000, targetObject);

            Assert.AreEqual(1000, listener.eventPayloadValue);

            yield return null;
        }

        [UnityTest]
        public IEnumerator DisableEventAutoRegister()
        {
            GameObject sourceObject = new GameObject();
            TestEventSender sender = sourceObject.AddComponent<TestEventSender>();

            GameObject targetObject = new GameObject();
            TestEventAutoRegister listener = targetObject.AddComponent<TestEventAutoRegister>();

            listener.enabled = false;
            sender.SendEvent(1000, targetObject);
            Assert.AreEqual(0, listener.eventPayloadValue);

            listener.enabled = true;
            sender.SendEvent(1000, targetObject);
            Assert.AreEqual(1000, listener.eventPayloadValue);

            yield return null;
        }
    }
}