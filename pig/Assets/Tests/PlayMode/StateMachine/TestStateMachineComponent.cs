using UnityEngine;

namespace Tests
{
    public class TestStateMachineComponent : MonoBehaviour
    {
        public StateMachine stateMachine = new StateMachine();

        private void Start()
        {
            TestStateA testA = new TestStateA();
            TestStateB testB = new TestStateB();
            TestStateC testC = new TestStateC();

            stateMachine.AddState(testA);
            stateMachine.AddState(testB);
            stateMachine.AddState(testC);

            testA.AddStateLink(new StateLink(testB, testA.CanPerformTransition));
            testB.AddStateLink(new StateLink(testC, testB.CanPerformTransition));
            testC.AddStateLink(new StateLink(testA, testC.CanPerformTransition));

            stateMachine.Start<TestStateA>();
        }

        private void Update()
        {
            stateMachine.Tick();
        }
    }
}