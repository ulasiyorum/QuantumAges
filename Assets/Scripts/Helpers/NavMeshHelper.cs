namespace Helpers
{
    public static class NavMeshHelper
    {
        public static void StopAgent(this UnityEngine.AI.NavMeshAgent agent)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }
    }
}