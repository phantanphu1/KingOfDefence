using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ItemUiCharacter : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _textLevel;
    [SerializeField] private Text _manaText;
    private CharacterItem _currentItemData;
    public CanvasGroup canvasGroup;
    public Button buttonCharacter;
    private float _mana;
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void SetItem(CharacterItem characterItem)
    {
        _currentItemData = characterItem;
        _image.sprite = characterItem.ImageCharacter;
        _textLevel.text = "LV: " + characterItem.baseLevel.ToString();
        _manaText.text = characterItem.Mana.ToString();
        // Debug.Log($"SetItem: {JsonUtility.ToJson(characterItem)}");
    }
    private void Update()
    {
        _mana = CharacterManager.Instance.ReturnManaData();
        UpdateStateCharacter();

    }
    public void OnItemClicked()
    {
        if (_currentItemData.baseLevel < _currentItemData.MaxLeve)
        {
            UpgradeCharacter();
        }
        else
        {
            Debug.LogWarning("nhan vat da max level");
        }
    }
    public void UpgradeCharacter()
    {
        int nextLevel = _currentItemData.baseLevel + 1;
        CharacterItem newCharacterItem = null;

        foreach (var item in TableObjectManage.Instance.characterConfig.lsCharacterItem)
        {
            if (item.baseLevel == nextLevel && item.heroType == _currentItemData.heroType)
            {
                newCharacterItem = item;
                break;
            }
        }
        if (newCharacterItem != null)
        {
            CharacterManager.Instance.RemoveCharacter(_currentItemData);
            CharacterManager.Instance.TakeMama(_currentItemData.Mana);
            CharacterManager.Instance.AddCharacter(newCharacterItem);
            SetItem(newCharacterItem);

            // CharacterManager.Instance.UpgradeAllCharacterLevel(newCharacterItem);
            _currentItemData = newCharacterItem;
        }
        else
        {
            Debug.LogWarning("k tim thay");
        }
    }
    private void UpdateStateCharacter()
    {
        if (_mana >= _currentItemData.Mana)
        {
            canvasGroup.alpha = 1f;
            buttonCharacter.interactable = true;
        }
        else
        {
            canvasGroup.alpha = 0.4f;
            buttonCharacter.interactable = false;
        }
    }

}
