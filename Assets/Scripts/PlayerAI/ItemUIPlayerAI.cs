using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIPlayerAI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _textLevel;
    private PlayerAIItem _currentItemData;
    private float _mana;
    public CanvasGroup canvasGroup;
    public Button buttonPlayerAI;


    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void SetItem(PlayerAIItem playerAIItem)
    {
        _currentItemData = playerAIItem;
        _image.sprite = playerAIItem.ImagePlayerAI;
        _textLevel.text = "LV: " + playerAIItem.baseLevel.ToString();
    }
    private void Update()
    {
        _mana = PlayerAIManager.Instance.ReturnManaData();
        UpdateStatePlayer();

    }
    public void OnItemClicked()
    {
        if (_currentItemData.baseLevel < _currentItemData.MaxLeve)
        {
            UpgradePlayerAI();
        }
        else
        {
            Debug.LogWarning("nhan vat da max level");
        }
    }
    public void UpgradePlayerAI()
    {
        int nextLevel = _currentItemData.baseLevel + 1;
        PlayerAIItem newPlayerItem = null;

        foreach (var item in TableObjectManage.Instance.playerAIConfig.lsPlayerAIItem)
        {
            if (item.baseLevel == nextLevel && item.heroType == _currentItemData.heroType)
            {
                newPlayerItem = item;
                break;
            }
        }
        if (newPlayerItem != null)
        {
            PlayerAIManager.Instance.RemovePlayerAI(_currentItemData);
            PlayerAIManager.Instance.TakeMama(_currentItemData.Mana);
            PlayerAIManager.Instance.AddPlayerAI(newPlayerItem);
            SetItem(newPlayerItem);

            _currentItemData = newPlayerItem;
        }
        else
        {
            Debug.LogWarning("k tim thay");
        }
    }
    private void UpdateStatePlayer()
    {
        if (_mana >= _currentItemData.Mana)
        {
            canvasGroup.alpha = 1f;
            buttonPlayerAI.interactable = true;
            // OnItemClicked();
        }
        else
        {
            canvasGroup.alpha = 0.4f;
            buttonPlayerAI.interactable = false;
        }
    }
}
