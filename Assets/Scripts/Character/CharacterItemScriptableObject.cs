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

    public int Damage;

    public int baseLevel;

    public HeroType heroType;

    public HeroRarity heroRarity;
    public float SpeedAttack;
    public int MaxLeve;
}