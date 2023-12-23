using System;

namespace Managers.Abstract
{
    public interface IDamagable
    {
        Guid GetId();
        void TakeDamage(float damage);
    }
}