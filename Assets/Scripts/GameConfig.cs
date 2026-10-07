using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "PR15/Game Config", order = 2)]
public class GameConfig : ScriptableObject
{
    [Header("Общие настройки")]
    public string configName = "Основная конфигурация";
    public DifficultyLevel difficulty = DifficultyLevel.Normal;

    [Header("Параметры игрока")]
    public int startingHealth = 100;
    public int startingMana = 50;
    public float startingSpeed = 5f;
    public int startingGold = 500;

    [Header("Экономика")]
    public float sellPriceMultiplier = 0.5f;
    public float buyPriceMultiplier = 1.5f;
    public float experienceMultiplier = 1f;

    [Header("Сохранения")]
    public string saveFileName = "savegame";
    public bool useEncryption = false;
    public string encryptionKey = "MySecretKey123";

    public string GetFullInfo()
    {
        string info = $"<b>КОНФИГУРАЦИЯ ИГРЫ</b>\n";
        info += $"Название: {configName}\n";
        info += $"Сложность: {GetDifficultyName()}\n";
        info += $"Стартовое здоровье: {startingHealth}\n";
        info += $"Стартовая мана: {startingMana}\n";
        info += $"Стартовая скорость: {startingSpeed:F1}\n";
        info += $"Стартовое золото: {startingGold}\n";
        info += $"Множитель продажи: x{sellPriceMultiplier:F1}\n";
        info += $"Множитель покупки: x{buyPriceMultiplier:F1}\n";
        info += $"Имя файла сохранения: {saveFileName}\n";
        info += $"Шифрование: {(useEncryption ? "Да" : "Нет")}\n";
        return info;
    }

    private string GetDifficultyName()
    {
        switch (difficulty)
        {
            case DifficultyLevel.Easy: return "Лёгкая";
            case DifficultyLevel.Normal: return "Обычная";
            case DifficultyLevel.Hard: return "Сложная";
            case DifficultyLevel.Nightmare: return "Кошмар";
            default: return "Неизвестно";
        }
    }

    public void CycleDifficulty()
    {
        int next = ((int)difficulty + 1) % 4;
        difficulty = (DifficultyLevel)next;
    }
}

public enum DifficultyLevel
{
    Easy,
    Normal,
    Hard,
    Nightmare
}