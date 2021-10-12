using UnityEngine;

namespace Tests
{
    public class TestStateB : State
    {
        public float timer = 0.0f;

        public override void Enter(object[] unused)
        {
            timer = 0.0f;
        }

        public override void Tick()
        {
            timer += Time.deltaTime;
        }

        public bool CanPerformTransition()
        {
            return timer > 1.0f;
        }
    }
}