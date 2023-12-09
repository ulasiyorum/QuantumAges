using UnityEngine;

namespace Helpers
{
    public static class GameObjectHelper
    {
        public static bool IsActive(this GameObject gameObject)
        {
            return gameObject.activeSelf;
        }
    }
}