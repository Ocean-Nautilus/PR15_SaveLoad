using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveLoadController : MonoBehaviour
{
    [Header("UI элементы")]
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI playerDataText;
    public TextMeshProUGUI configText;
    public Transform savesContainer;

    [Header("Кнопки")]
    public Button savePlayerPrefsButton;
    public Button saveJsonButton;
    public Button saveBinaryButton;
    public Button loadButton;
    public Button clearButton;
    public GameObject saveButtonPrefab;

    [Header("ScriptableObject ссылки")]
    public GameConfig gameConfig;
    public List<ItemData> itemDatabase;

    [Header("Настройки")]
    public string saveFileName = "savegame";

    private PlayerData currentPlayerData;
    private string lastSaveType = "";

    void Start()
    {
        if (savePlayerPrefsButton != null)
            savePlayerPrefsButton.onClick.AddListener(() => SaveData("PlayerPrefs"));
        if (saveJsonButton != null)
            saveJsonButton.onClick.AddListener(() => SaveData("JSON"));
        if (saveBinaryButton != null)
            saveBinaryButton.onClick.AddListener(() => SaveData("Binary"));
        if (loadButton != null)
            loadButton.onClick.AddListener(LoadData);
        if (clearButton != null)
            clearButton.onClick.AddListener(ClearAllData);

        CreateNewPlayerData();
        UpdatePlayerDataDisplay();
        RefreshSaveList();
        UpdateStatus("Система сохранений готова");
    }

    void CreateNewPlayerData()
    {
        currentPlayerData = new PlayerData();

        if (gameConfig != null)
        {
            currentPlayerData.health = gameConfig.startingHealth;
            currentPlayerData.maxHealth = gameConfig.startingHealth;
            currentPlayerData.mana = gameConfig.startingMana;
            currentPlayerData.maxMana = gameConfig.startingMana;
            currentPlayerData.speed = gameConfig.startingSpeed;
            currentPlayerData.gold = gameConfig.startingGold;
        }

        // Добавляем случайные предметы для теста
        if (itemDatabase != null && itemDatabase.Count > 0)
        {
            for (int i = 0; i < 3; i++)
            {
                ItemData randomItem = itemDatabase[Random.Range(0, itemDatabase.Count)];
                currentPlayerData.AddItem(randomItem.itemName);
            }
        }

        currentPlayerData.unlockedLevels.Add("Level_01");
        currentPlayerData.unlockedLevels.Add("Level_02");

        Debug.Log("Созданы новые данные игрока");
    }

    void SaveData(string saveType)
    {
        if (currentPlayerData == null)
        {
            UpdateStatus("Нет данных для сохранения!");
            return;
        }

        currentPlayerData.lastSaveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        switch (saveType)
        {
            case "PlayerPrefs":
                SaveManager.SaveToPlayerPrefs(currentPlayerData);
                lastSaveType = "PlayerPrefs";
                UpdateStatus("Сохранено в PlayerPrefs!");
                break;

            case "JSON":
                SaveManager.SaveToJson(currentPlayerData, saveFileName);
                lastSaveType = "JSON";
                UpdateStatus("Сохранено в JSON!");
                break;

            case "Binary":
                SaveManager.SaveToBinary(currentPlayerData, saveFileName);
                lastSaveType = "Binary";
                UpdateStatus("Сохранено в Binary!");
                break;
        }

        if (configText != null)
        {
            configText.text = $"Последнее сохранение: {saveType} | {currentPlayerData.lastSaveTime}";
        }

        RefreshSaveList();
    }

    void LoadData()
    {
        PlayerData loadedData = null;
        string loadedFrom = "";

        // Пробуем загрузить из JSON
        loadedData = SaveManager.LoadFromJson(saveFileName);
        if (loadedData != null) loadedFrom = "JSON";

        // Если JSON не найден, пробуем Binary
        if (loadedData == null)
        {
            loadedData = SaveManager.LoadFromBinary(saveFileName);
            if (loadedData != null) loadedFrom = "Binary";
        }

        // Если Binary не найден, пробуем PlayerPrefs
        if (loadedData == null)
        {
            loadedData = SaveManager.LoadFromPlayerPrefs();
            if (loadedData != null) loadedFrom = "PlayerPrefs";
        }

        if (loadedData != null)
        {
            currentPlayerData = loadedData;
            UpdatePlayerDataDisplay();
            UpdateStatus($"Загружено из {loadedFrom}!");

            if (configText != null)
            {
                configText.text = $"Загружено из: {loadedFrom} | {currentPlayerData.lastSaveTime}";
            }
        }
        else
        {
            UpdateStatus("Сохранения не найдены!");
        }

        RefreshSaveList();
    }

    void ClearAllData()
    {
        SaveManager.DeletePlayerPrefs();
        SaveManager.DeleteJson(saveFileName);
        SaveManager.DeleteBinary(saveFileName);

        CreateNewPlayerData();
        UpdatePlayerDataDisplay();
        UpdateStatus("Все сохранения удалены!");

        if (configText != null)
        {
            configText.text = "Все сохранения удалены";
        }

        RefreshSaveList();
    }

    void UpdatePlayerDataDisplay()
    {
        if (playerDataText != null && currentPlayerData != null)
        {
            playerDataText.text = currentPlayerData.GetFullInfo();
        }
    }

    void RefreshSaveList()
    {
        if (savesContainer == null) return;

        // Очищаем контейнер
        foreach (Transform child in savesContainer)
        {
            Destroy(child.gameObject);
        }

        // Создаём кнопки для каждого типа сохранения
        CreateSaveButton("PlayerPrefs", PlayerPrefs.HasKey("PlayerName"));
        CreateSaveButton("JSON", File.Exists(SaveManager.GetSavePath(saveFileName)));
        CreateSaveButton("Binary", File.Exists(SaveManager.GetBinarySavePath(saveFileName)));
    }

    void CreateSaveButton(string saveType, bool exists)
    {
        if (saveButtonPrefab == null) return;

        GameObject buttonObj = Instantiate(saveButtonPrefab, savesContainer);
        TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

        if (buttonText != null)
        {
            string status = exists ? "+" : "-";
            string color = exists ? "#90EE90" : "#FF6B6B";
            buttonText.text = $"<color={color}>{status}</color> {saveType}";
        }

        Button button = buttonObj.GetComponent<Button>();
        if (button != null)
        {
            string capturedType = saveType;
            button.onClick.AddListener(() => LoadSpecificSave(capturedType));
        }
    }

    void LoadSpecificSave(string saveType)
    {
        PlayerData loadedData = null;

        switch (saveType)
        {
            case "PlayerPrefs":
                loadedData = SaveManager.LoadFromPlayerPrefs();
                break;
            case "JSON":
                loadedData = SaveManager.LoadFromJson(saveFileName);
                break;
            case "Binary":
                loadedData = SaveManager.LoadFromBinary(saveFileName);
                break;
        }

        if (loadedData != null)
        {
            currentPlayerData = loadedData;
            UpdatePlayerDataDisplay();
            UpdateStatus($"Загружено из {saveType}!");
        }
        else
        {
            UpdateStatus($"{saveType}: сохранение не найдено!");
        }
    }

    void UpdateStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }

    void Update()
    {
        // Горячие клавиши
        if (Input.GetKeyDown(KeyCode.F5)) SaveData("JSON");
        if (Input.GetKeyDown(KeyCode.F9)) LoadData();
        if (Input.GetKeyDown(KeyCode.F10)) SaveData("Binary");
        if (Input.GetKeyDown(KeyCode.F11)) SaveData("PlayerPrefs");
        if (Input.GetKeyDown(KeyCode.Delete)) ClearAllData();

        // Обновляем время игры
        if (currentPlayerData != null)
        {
            currentPlayerData.totalPlayTime += Time.deltaTime;
        }
    }
}