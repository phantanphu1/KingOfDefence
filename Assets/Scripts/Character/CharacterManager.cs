using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
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
    [SerializeField] private Image imageTime;
    [SerializeField] private CanvasGroup canvasGroup;
    private float totalTime = 30f;
    private float currentTime = 0;
    [SerializeField] private TextMeshProUGUI txtText;
    [SerializeField] private string text;
    private float delayBetweenCharacters = 0.03f;
    [SerializeField] GameObject botUI;
    private int maxHealth = 3;
    private int currentHealth;
    public List<Image> hearts;
    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;
    private Animator animator;
    [SerializeField] private TextMeshProUGUI txtTimePlayer;
    private int minutePlayer = 2;
    private int secondPlayer = 0;
    private float timeNext;
    [SerializeField] private GameObject characterBotPrefab;
    private Enemy enemy;
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
        _manaText.text = _mana.ToString();
        LoadData();
        imageTime.fillAmount = 0f;
        StartCoroutine(SetUpdateTime());
        currentHealth = maxHealth;
        UpdateHealthUI();
        animator = GetComponent<Animator>();
        txtTimePlayer.text = $"{minutePlayer:D2}:{secondPlayer:D2}";
        // enemy = FindObjectOfType<Enemy>();

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
    private void Update()
    {
        timeNext += Time.deltaTime;
        if (timeNext >= 1f)
        {
            LoadTimePlayer();
            timeNext = 0f;
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
    private void ActiveCharacterBot()
    {
        Enemy enemyPosition = EnemySpawner.Instance.GetFarthestEnemy();
        Vector3 posi = enemyPosition.transform.position;
        var tam = EnemySpawner.Instance.GetWaypointIndexForFarthestEnemy();

        if (tam == 1 && posi.y < -0.70)
        {
            posi.y += 0.5f;
        }
        else if (tam == 2 && posi.x < 1.8)
        {
            posi.x += 0.5f;
        }
        else if (tam == 3 && posi.y > -1.54)
        {
            posi.y -= 0.5f;
        }
        else
        {
            posi.x = -0.06f;
            posi.y = -0.09f;
        }

        GameObject CharacterBotActive = Instantiate(characterBotPrefab, posi, Quaternion.identity);
        // Debug.Log($"vao:{CharacterBotActive}");
        botUI.SetActive(false);
        currentTime = 0;
        StartCoroutine(SetUpdateTime());
        StartCoroutine(EnableCharacterBot(CharacterBotActive));
    }
    IEnumerator EnableCharacterBot(GameObject gam)
    {
        yield return new WaitForSeconds(4f);
        Destroy(gam);
    }

    IEnumerator ShowText()
    {
        txtText.text = "";
        foreach (var item in text)
        {
            txtText.text += item;
            yield return new WaitForSeconds(delayBetweenCharacters);
        }
        // yield return new WaitForSeconds(2f);
        ActiveCharacterBot();
    }
    public void LoadDataText()
    {
        botUI.SetActive(true);
        StartCoroutine(ShowText());
    }
    void UpdateHealthUI()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeartSprite;
                hearts[i].color = Color.white;
            }
            else
            {
                // if (emptyHeartSprite != null)
                // {
                //     Debug.Log("he");
                //     hearts[i].sprite = emptyHeartSprite;
                // }
                // else
                // {
                hearts[i].color = new Color(1f, 1f, 1f, 0.3f);
                // }
            }
        }
    }
    public void TakeHealth(int health)
    {
        currentHealth -= health;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        if (currentHealth <= 0)
        {
            GameOver();
        }
    }
    private void GameOver()
    {
        StartCoroutine(LoadUI());
    }
    private void LoadTranformUI()
    {
        SceneManager.LoadScene("Main");

    }
    IEnumerator LoadUI()
    {
        yield return new WaitForSeconds(3f);
    }
    private void LoadTimePlayer()
    {
        secondPlayer--;
        if (secondPlayer <= 0)
        {
            minutePlayer--;
            secondPlayer = 59;
        }
        txtTimePlayer.text = $"{minutePlayer:D2}:{secondPlayer:D2}";
        if (secondPlayer == 0 && minutePlayer == 0)
        {
            Debug.Log("hoa game");
        }
    }
}
