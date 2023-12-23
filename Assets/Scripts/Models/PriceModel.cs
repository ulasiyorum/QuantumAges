using Consts;

namespace Models
{
    public class PriceModel
    {
        public int Crystal { get; }
        public ResourceType ResourceType { get; }
        public SoldierEnum SoldierType { get; }

        public PriceModel(int crytsal , ResourceType type, SoldierEnum soldierType)
        {
            Crystal = crytsal;
            ResourceType = type;
            SoldierType = soldierType;
        }
    }
}