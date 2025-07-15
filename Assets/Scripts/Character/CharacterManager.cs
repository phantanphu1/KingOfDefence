using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

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
    [SerializeField] private Image imageTime;
    [SerializeField] private CanvasGroup canvasGroup;
    private float totalTime = 15f;
    private float currentTime = 0;
    [SerializeField] GameObject characterBot;
    private bool isActiveBomb = false;

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
        imageTime.fillAmount = 0f;
        StartCoroutine(SetUpdateTime());

    }
    private void Update()
    {
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
    private void UpdateUITime(float currentTime)
    {
        imageTime.fillAmount = currentTime / totalTime;
    }
    private IEnumerator SetUpdateTime()
    {
        while (currentTime < totalTime)
        {
            UpdateActiveButton();
            currentTime += Time.deltaTime;
            UpdateUITime(currentTime);
            yield return null;
        }
    }
    private void UpdateActiveButton()
    {
        float timeNext = totalTime - 0.1f;
        if (currentTime < timeNext)
        {
            canvasGroup.interactable = false;
            canvasGroup.alpha = 0.4f;
        }
        else
        {
            canvasGroup.interactable = true;
            canvasGroup.alpha = 1f;
        }
    }
    public void ActiveCharacterBot()
    {
        characterBot.SetActive(true);
        currentTime = 0;
        StartCoroutine(SetUpdateTime());
    }
    private void EnableCharacterBot()
    {
        float data = Time.deltaTime;
        Debug.LogWarning($"data:{data}");
        if (data > 1.99)
        {
            characterBot.SetActive(false);
            data = 0;
        }
    }
}
