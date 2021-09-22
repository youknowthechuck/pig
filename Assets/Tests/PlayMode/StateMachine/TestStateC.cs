using UnityEngine;

namespace Tests
{
    public class TestStateC : State
    {
        public float timer = 0.0f;

        public override void Enter()
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