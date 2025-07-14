using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterItemConfig", menuName = "Character/ListCharacterItem")]
public class CharacterItemScriptableObject : ScriptableObject
{
    public List<CharacterItem> lsCharacterItem;
}
[Serializable]
public class CharacterItem
{
    public GameObject characterPrefab;
    public Sprite ImageCharacter;

    public int Damage;

    public int baseLevel;

    public HeroType heroType;

    public HeroRarity heroRarity;
    public float SpeedAttack;
    public int MaxLeve;
    public float Mana;
    public CharacterItem(CharacterItem item)
    {
        this.characterPrefab = item.characterPrefab;
        this.ImageCharacter = item.ImageCharacter;
        this.Damage = item.Damage;
        this.baseLevel = item.baseLevel;
        this.heroType = item.heroType;
        this.heroRarity = item.heroRarity;
        this.SpeedAttack = item.SpeedAttack;
        this.MaxLeve = item.MaxLeve;
        this.Mana = item.Mana;
    }
}