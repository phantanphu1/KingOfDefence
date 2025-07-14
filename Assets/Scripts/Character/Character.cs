using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Character : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Board board;
    private TableObjectManage tableObjectManage;
    private CharacterItem characterItem;
    [HideInInspector] public Transform parentAfterDrag;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    private Cell currentCell;

    private void Awake()
    {
        tableObjectManage = TableObjectManage.Instance;

        rectTransform = GetComponent<RectTransform>();

        currentCell = GetComponentInParent<Cell>();
        if (currentCell != null)
        {
            currentCell.IsEmpty = false;
            currentCell.CharacterOnCell = this;
        }
    }
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public bool TryMergeCharacter(Character targetCharacter)
    {
        var targetCharacterItem = targetCharacter.characterItem;
        if (targetCharacter == null)
        {
            Debug.LogError("k co targetCharacter");
            return false;
        }
        if (this.characterItem.heroType != targetCharacterItem.heroType)
        {
            Debug.LogError("type nhan vat khac nhau");
            return false;
        }
        int sumCharacter = this.characterItem.baseLevel + targetCharacterItem.baseLevel;
        if (sumCharacter > this.characterItem.MaxLeve || sumCharacter > targetCharacterItem.MaxLeve)
        {
            Debug.Log("nhan vat da maxLevel");
            return false;
        }

        int level = targetCharacterItem.baseLevel;
        int levelDrag = this.characterItem.baseLevel;
        int sum = level + levelDrag;
        foreach (var characterUpgrade in tableObjectManage.characterConfig.lsCharacterItem)
        {
            if (characterUpgrade.heroType == targetCharacterItem.heroType && characterUpgrade.baseLevel == sum)
            {
                OnUpgradeCharacter(characterUpgrade, targetCharacter.currentCell);
                return true;
            }

        }
        Debug.Log("Không thể hợp nhất: khoong tin thay.");

        return false;
    }

    private void OnUpgradeCharacter(CharacterItem upgradedCharacterItem, Cell targetCell)
    {
        if (upgradedCharacterItem == null || targetCell == null)
        {
            Debug.LogError("Nâng cấp thất bại: CharacterItem được nâng cấp hoặc ô mục tiêu là null.");
            return;
        }

        GameObject upgradedPrefab = upgradedCharacterItem.characterPrefab;
        if (upgradedPrefab == null)
        {
            Debug.LogError("Prefab nhân vật được nâng cấp là null.");
            return;
        }
        ReturnCharacter(targetCell.CharacterOnCell);
        // Khởi tạo nhân vật mới tại vị trí của ô mục tiêu
        GameObject newCharacterGO = Instantiate(upgradedPrefab, targetCell.transform);
        newCharacterGO.transform.localPosition = new Vector3(0f, -45f, 0f); // Duy trì cùng offset

        Character newCharacter = newCharacterGO.GetComponent<Character>();
        if (newCharacter != null)
        {
            newCharacter.characterItem = upgradedCharacterItem; // Gán dữ liệu item đã nâng cấp cho nhân vật mới
            newCharacter.currentCell = targetCell; // Liên kết nhân vật mới với ô
            targetCell.CharacterOnCell = newCharacter; // Cập nhật tham chiếu của ô đến nhân vật mới
        }
        Debug.Log($"Đã nâng cấp lên: {upgradedCharacterItem.heroType} trong ô {targetCell.name}");
        ReturnCharacter(this);
    }
    private void ReturnCharacter(Character character)
    {
        // character.gameObject.SetActive(false);
        Destroy(character.gameObject);
    }


    // bắt đầu kéo
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Debug.Log("OnBeginDrag");
        originalPosition = rectTransform.anchoredPosition;

        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.6f;
        }

    }
    // đang kéo
    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta;
    }

    // kết thúc kéo
    public void OnEndDrag(PointerEventData eventData)
    {

        if (canvasGroup != null)
        {

            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
        }
        if (this.characterItem == null)
        {
            rectTransform.anchoredPosition = originalPosition;
            return;
        }
        Cell targetCell = null;
        Character targetCharacter = null;

        foreach (GameObject hoveredObject in eventData.hovered)
        {
            if (hoveredObject.CompareTag("Cell"))
            {

                Cell cell = hoveredObject.GetComponent<Cell>();
                if (cell != null)
                {
                    targetCell = cell;
                    targetCharacter = cell.CharacterOnCell;
                    break;
                }
            }
        }

        bool droppedValidly = false;
        if (targetCharacter != null)
        {
            if (TryMergeCharacter(targetCharacter))
            {
                droppedValidly = true;
            }
        }
        else if (targetCell != null)
        {
            transform.SetParent(targetCell.transform);
            transform.localPosition = new Vector3(0f, -45f, 0f);
            currentCell = targetCell;
            targetCell.IsEmpty = false;
            targetCell.CharacterOnCell = this;
            droppedValidly = true;
        }
        if (!droppedValidly)
        {
            Debug.Log("K co vt phu hop:");
            rectTransform.anchoredPosition = originalPosition;
        }
    }
    public void SetupCharacter(CharacterItem characterItem)
    {
        this.characterItem = characterItem;
    }
    public void chadf(CharacterItem chaf)
    {
        characterItem = chaf;
    }

}
