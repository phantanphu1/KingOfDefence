using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAIManager : MonoBehaviour
{
    public static PlayerAIManager Instance;
    public List<PlayerAIItem> _lsPlayerAILV1 { get; private set; } = new List<PlayerAIItem>();
    private TableObjectManage tableObjectManage;
    [SerializeField] private Transform itemHolder;
    [SerializeField] private GameObject itemPrefabs;
    private ItemUIPlayerAI itemUIPlayerAI;
    private float _mana = 30;

    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
        DontDestroyOnLoad(this);

        tableObjectManage = TableObjectManage.Instance;
        LoadData();


    }
    private void Start()
    {
        itemUIPlayerAI = FindObjectOfType<ItemUIPlayerAI>();
    }
    private void LoadCharacterItem()
    {
        foreach (var item in tableObjectManage.playerAIConfig.lsPlayerAIItem)
        {
            if (item.baseLevel == 1)
            {
                _lsPlayerAILV1.Add(item);
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
        foreach (var item in _lsPlayerAILV1)
        {
            GameObject obj = Instantiate(itemPrefabs, itemHolder);
            var tam = obj.GetComponent<ItemUIPlayerAI>();
            tam.SetItem(item);
        }

    }
    public void Mana(int mana)
    {
        _mana += mana;
    }
    public void TakeMama(float mana)
    {
        _mana -= mana;

    }
    public float ReturnManaData()
    {
        Debug.Log($"_mana:{_mana}");
        return _mana;
    }
    public void RemovePlayerAI(PlayerAIItem data)
    {
        foreach (var item in tableObjectManage.playerAIConfig.lsPlayerAIItem)
        {
            if (item.heroType == data.heroType && item.baseLevel == data.baseLevel)
            {
                _lsPlayerAILV1.Remove(item);
                Debug.Log($"count:{tableObjectManage.playerAIConfig.lsPlayerAIItem.Count}");
            }
        }
    }
    public void AddPlayerAI(PlayerAIItem playerAIItem)
    {
        _lsPlayerAILV1.Add(playerAIItem);
        Debug.Log($"count1:{tableObjectManage.playerAIConfig.lsPlayerAIItem.Count}");

    }
}
