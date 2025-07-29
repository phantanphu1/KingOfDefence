using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAIItemConfig", menuName = "PlayerAI/ListPlayerAIItem")]
public class PlayerItemScriptableObject : ScriptableObject
{
    public List<PlayerAIItem> lsPlayerAIItem;
}
[Serializable]
public class PlayerAIItem
{
    public GameObject playerAIPrefab;
    public Sprite ImagePlayerAI;

    public int Damage;

    public int baseLevel;

    public HeroType heroType;

    public HeroRarity heroRarity;
    public float SpeedAttack;
    public int MaxLeve;
    public float Mana;
    public PlayerAIItem(PlayerAIItem item)
    {
        this.playerAIPrefab = item.playerAIPrefab;
        this.ImagePlayerAI = item.ImagePlayerAI;
        this.Damage = item.Damage;
        this.baseLevel = item.baseLevel;
        this.heroType = item.heroType;
        this.heroRarity = item.heroRarity;
        this.SpeedAttack = item.SpeedAttack;
        this.MaxLeve = item.MaxLeve;
        this.Mana = item.Mana;
    }
}