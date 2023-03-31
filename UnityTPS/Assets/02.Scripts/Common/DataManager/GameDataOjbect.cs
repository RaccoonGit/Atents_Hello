using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Data SO", menuName = "Create Gamedata", order = 1)]
public class GameDataOjbect : ScriptableObject
{
    public int killCount = 0;
    public float hp = 120;
    public float damage = 25;
    public float speed = 6.0f;
    public List<ItemInfo> equipItem;
}
