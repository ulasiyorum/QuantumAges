using UnityEngine;

namespace Helpers
{
    public static class PopUp
    {
        public static void Error(string err)
        {
            StartPopUpMessage.Message(err, Color.red);
        }
        
        public static void Success(string msg)
        {
            StartPopUpMessage.Message(msg, Color.green);
        }
        
        public static void Warning(string msg)
        {
            StartPopUpMessage.Message(msg, Color.yellow);
        }
    }
}