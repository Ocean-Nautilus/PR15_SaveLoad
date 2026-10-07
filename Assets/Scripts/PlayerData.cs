using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    [Header("Основная информация")]
    public string playerName = "Игрок";
    public int level = 1;
    public int experience = 0;

    [Header("Характеристики")]
    public int health = 100;
    public int maxHealth = 100;
    public int mana = 50;
    public int maxMana = 50;
    public float speed = 5f;

    [Header("Ресурсы")]
    public int gold = 500;
    public int gems = 0;

    [Header("Позиция в мире")]
    public float posX = 0f;
    public float posY = 0f;
    public float posZ = 0f;

    [Header("Прогресс")]
    public List<string> unlockedLevels = new List<string>();
    public List<string> inventoryItems = new List<string>();
    public Dictionary<string, int> itemCounts = new Dictionary<string, int>();

    [Header("Настройки")]
    public float musicVolume = 0.7f;
    public float sfxVolume = 1.0f;
    public bool fullscreen = true;
    public int qualityLevel = 2;

    [Header("Время")]
    public string lastSaveTime = "";
    public float totalPlayTime = 0f;

    public PlayerData()
    {
        lastSaveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public string GetFullInfo()
    {
        string info = $"<b>ДАННЫЕ ИГРОКА</b>\n";
        info += $"Имя: {playerName}\n";
        info += $"Уровень: {level} (Опыт: {experience})\n";
        info += $"Здоровье: {health}/{maxHealth}\n";
        info += $"Мана: {mana}/{maxMana}\n";
        info += $"Скорость: {speed:F1}\n";
        info += $"Золото: {gold}\n";
        info += $"Кристаллы: {gems}\n";
        info += $"Позиция: ({posX:F1}, {posY:F1}, {posZ:F1})\n";
        info += $"Предметов в инвентаре: {inventoryItems.Count}\n";
        info += $"Разблокировано уровней: {unlockedLevels.Count}\n";
        info += $"Последнее сохранение: {lastSaveTime}\n";
        info += $"Общее время игры: {totalPlayTime:F0} сек\n";
        return info;
    }

    public void AddItem(string itemName)
    {
        inventoryItems.Add(itemName);
        if (itemCounts.ContainsKey(itemName))
            itemCounts[itemName]++;
        else
            itemCounts[itemName] = 1;
    }

    public bool RemoveItem(string itemName)
    {
        if (inventoryItems.Contains(itemName))
        {
            inventoryItems.Remove(itemName);
            if (itemCounts.ContainsKey(itemName))
            {
                itemCounts[itemName]--;
                if (itemCounts[itemName] <= 0)
                    itemCounts.Remove(itemName);
            }
            return true;
        }
        return false;
    }
}