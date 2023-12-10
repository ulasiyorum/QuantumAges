using System;
using System.Collections.Generic;
using Models;
using UnityEngine;

namespace Helpers
{
    public static class PopUp
    {
        public static void Error(string err, Action onCompleteAction = null)
        {
            StartPopUpMessage.Message(err, Color.red, onCompleteAction);
        }
        
        public static void Success(string msg, Action onCompleteAction = null)
        {
            StartPopUpMessage.Message(msg, Color.green, onCompleteAction);
        }
        
        public static void Warning(string msg, Action onCompleteAction = null)
        {
            StartPopUpMessage.Message(msg, Color.yellow, onCompleteAction);
        }
    }
}