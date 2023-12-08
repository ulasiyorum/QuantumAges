using System;
using UnityEngine;

namespace Helpers
{
    public class Main : MonoBehaviour
    {
        private void Update()
        {
            foreach (var anims in SoldierAnimator.soldiers)
            {
                anims.Animate();
            }
        }
    }
}