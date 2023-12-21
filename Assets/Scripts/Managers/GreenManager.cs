using System;
using Managers.Abstract;

namespace Managers
{
    public sealed class GreenManager : PlayerManager
    {
        private void Awake()
        {
            green_manager = this;
        }
    }
}