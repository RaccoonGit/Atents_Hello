using System.Collections;
using System.Collections.Generic;

namespace DataInfo
{
    [System.Serializable]
    public class GameData
    {
        public int killCount = 0;
        public float hp = 120.0f;
        public float damage = 25.0f;
        public float speed = 2.5f;
        public List<Item> equipItem;
    }

    [System.Serializable]
    public class Item
    {
        public enum ItemType { HP, SPEED, GRENADE, DAMAGE }
        public enum ItemCalc { INC_VALUE, PERCENT}

        public ItemType itemType;
        public ItemCalc itemCalc;
        public string name;
        public string desc;
        public float value;

    }
}
