using System;
using Managers.Abstract;

namespace Managers
{
    public sealed class RedManager : PlayerManager
    {
        private void Awake()
        {
            red_manager = this;
        }
    }
}