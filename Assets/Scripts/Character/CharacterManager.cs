using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;
    private float _mana = 110;
    [SerializeField] private TextMeshProUGUI _manaText;
    public List<CharacterItem> _lsCharacterLv1 { get; private set; } = new List<CharacterItem>();
    private TableObjectManage tableObjectManage;
    private ItemUiCharacter itemUiCharacter;
    [SerializeField] private Transform itemHolder;
    [SerializeField] private GameObject itemPrefabs;
    private CharacterItem characterItem;
    [SerializeField] private Transform itemBoard;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
        DontDestroyOnLoad(this);
        tableObjectManage = TableObjectManage.Instance;
    }
    void Start()
    {
        itemUiCharacter = FindObjectOfType<ItemUiCharacter>();
        _manaText.text = _mana.ToString();
        LoadData();
    }
    public void Mana(int mana)
    {
        _mana += mana;
        _manaText.text = _mana.ToString();
    }
    private void LoadCharacterItem()
    {
        foreach (var item in tableObjectManage.characterConfig.lsCharacterItem)
        {
            if (item.baseLevel == 1)
            {
                _lsCharacterLv1.Add(item);
            }
        }
    }
    private void LoadData()
    {
        LoadCharacterItem();
        foreach (Transform item in itemHolder)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in _lsCharacterLv1)
        {
            GameObject obj = Instantiate(itemPrefabs, itemHolder);
            var tam = obj.GetComponent<ItemUiCharacter>();
            tam.SetItem(item);
        }

    }
    public float ReturnManaData()
    {
        return _mana;
    }
    public void TakeMama(float mana)
    {
        _mana -= mana;
        _manaText.text = _mana.ToString();

    }
    public void RemoveCharacter(CharacterItem data)
    {
        foreach (var item in tableObjectManage.characterConfig.lsCharacterItem)
        {
            if (item.heroType == data.heroType && item.baseLevel == data.baseLevel)
            {
                _lsCharacterLv1.Remove(item);
            }
        }
    }
    public void AddCharacter(CharacterItem characterItem)
    {
        _lsCharacterLv1.Add(characterItem);
    }
    // public void UpgradeAllCharacterLevel(CharacterItem cha)
    // {
    //     foreach (Transform child in itemBoard)
    //     {
    //         if (child.childCount > 0)
    //         {
    //             Transform chillTransform = child.GetChild(0);

    //         }
    //         else
    //         {
    //             // Debug.LogWarning("khong co");
    //         }

    //     }
    // }
}
