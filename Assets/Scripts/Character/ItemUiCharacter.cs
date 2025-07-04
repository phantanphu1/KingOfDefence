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
    private Board _board;
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
    private void UpgradeCharacter()
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
            CharacterItem newCharacter = new CharacterItem(newCharacterItem);
            CharacterManager.Instance.AddCharacter(newCharacter);
            _board.GetComponent<Board>().AddCharacter(newCharacter);
            SetItem(newCharacter);
            _currentItemData = newCharacter;
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
