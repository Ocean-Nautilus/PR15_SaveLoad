using System;
using System.IO;
using System.Text;
using UnityEngine;

public static class SaveManager
{

    public static void SaveToPlayerPrefs(PlayerData data)
    {
        PlayerPrefs.SetString("PlayerName", data.playerName);
        PlayerPrefs.SetInt("Level", data.level);
        PlayerPrefs.SetInt("Experience", data.experience);
        PlayerPrefs.SetInt("Health", data.health);
        PlayerPrefs.SetInt("MaxHealth", data.maxHealth);
        PlayerPrefs.SetInt("Mana", data.mana);
        PlayerPrefs.SetInt("MaxMana", data.maxMana);
        PlayerPrefs.SetFloat("Speed", data.speed);
        PlayerPrefs.SetInt("Gold", data.gold);
        PlayerPrefs.SetInt("Gems", data.gems);
        PlayerPrefs.SetFloat("PosX", data.posX);
        PlayerPrefs.SetFloat("PosY", data.posY);
        PlayerPrefs.SetFloat("PosZ", data.posZ);
        PlayerPrefs.SetFloat("MusicVolume", data.musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", data.sfxVolume);
        PlayerPrefs.SetInt("Fullscreen", data.fullscreen ? 1 : 0);
        PlayerPrefs.SetInt("QualityLevel", data.qualityLevel);
        PlayerPrefs.SetString("LastSaveTime", data.lastSaveTime);
        PlayerPrefs.SetFloat("TotalPlayTime", data.totalPlayTime);

        PlayerPrefs.SetString("InventoryItems", string.Join("|", data.inventoryItems));
        PlayerPrefs.SetString("UnlockedLevels", string.Join("|", data.unlockedLevels));

        PlayerPrefs.Save();
        Debug.Log("[PlayerPrefs] Данные сохранены!");
    }

    public static PlayerData LoadFromPlayerPrefs()
    {
        if (!PlayerPrefs.HasKey("PlayerName"))
        {
            Debug.LogWarning("[PlayerPrefs] Сохранения не найдены!");
            return null;
        }

        PlayerData data = new PlayerData();
        data.playerName = PlayerPrefs.GetString("PlayerName", "Игрок");
        data.level = PlayerPrefs.GetInt("Level", 1);
        data.experience = PlayerPrefs.GetInt("Experience", 0);
        data.health = PlayerPrefs.GetInt("Health", 100);
        data.maxHealth = PlayerPrefs.GetInt("MaxHealth", 100);
        data.mana = PlayerPrefs.GetInt("Mana", 50);
        data.maxMana = PlayerPrefs.GetInt("MaxMana", 50);
        data.speed = PlayerPrefs.GetFloat("Speed", 5f);
        data.gold = PlayerPrefs.GetInt("Gold", 500);
        data.gems = PlayerPrefs.GetInt("Gems", 0);
        data.posX = PlayerPrefs.GetFloat("PosX", 0f);
        data.posY = PlayerPrefs.GetFloat("PosY", 0f);
        data.posZ = PlayerPrefs.GetFloat("PosZ", 0f);
        data.musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
        data.sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        data.fullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        data.qualityLevel = PlayerPrefs.GetInt("QualityLevel", 2);
        data.lastSaveTime = PlayerPrefs.GetString("LastSaveTime", "");
        data.totalPlayTime = PlayerPrefs.GetFloat("TotalPlayTime", 0f);

        string inventoryStr = PlayerPrefs.GetString("InventoryItems", "");
        if (!string.IsNullOrEmpty(inventoryStr))
            data.inventoryItems.AddRange(inventoryStr.Split('|'));

        string levelsStr = PlayerPrefs.GetString("UnlockedLevels", "");
        if (!string.IsNullOrEmpty(levelsStr))
            data.unlockedLevels.AddRange(levelsStr.Split('|'));

        Debug.Log("[PlayerPrefs] Данные загружены!");
        return data;
    }

    public static void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("[PlayerPrefs] Все данные удалены!");
    }


    public static string GetSavePath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName + ".json");
    }

    public static void SaveToJson(PlayerData data, string fileName = "savegame")
    {
        string json = JsonUtility.ToJson(data, true);
        string path = GetSavePath(fileName);
        File.WriteAllText(path, json, Encoding.UTF8);
        Debug.Log($"[JSON] Данные сохранены в: {path}");
    }

    public static PlayerData LoadFromJson(string fileName = "savegame")
    {
        string path = GetSavePath(fileName);

        if (!File.Exists(path))
        {
            Debug.LogWarning($"[JSON] Файл не найден: {path}");
            return null;
        }

        string json = File.ReadAllText(path, Encoding.UTF8);
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);
        Debug.Log($"[JSON] Данные загружены из: {path}");
        return data;
    }

    public static void DeleteJson(string fileName = "savegame")
    {
        string path = GetSavePath(fileName);
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"[JSON] Файл удалён: {path}");
        }
    }


    public static string GetBinarySavePath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName + ".dat");
    }

    public static void SaveToBinary(PlayerData data, string fileName = "savegame")
    {
        string path = GetBinarySavePath(fileName);

        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
        {
            writer.Write(data.playerName);
            writer.Write(data.level);
            writer.Write(data.experience);
            writer.Write(data.health);
            writer.Write(data.maxHealth);
            writer.Write(data.mana);
            writer.Write(data.maxMana);
            writer.Write(data.speed);
            writer.Write(data.gold);
            writer.Write(data.gems);
            writer.Write(data.posX);
            writer.Write(data.posY);
            writer.Write(data.posZ);
            writer.Write(data.musicVolume);
            writer.Write(data.sfxVolume);
            writer.Write(data.fullscreen);
            writer.Write(data.qualityLevel);
            writer.Write(data.lastSaveTime);
            writer.Write(data.totalPlayTime);

            writer.Write(data.inventoryItems.Count);
            foreach (var item in data.inventoryItems)
                writer.Write(item);

            writer.Write(data.unlockedLevels.Count);
            foreach (var level in data.unlockedLevels)
                writer.Write(level);
        }

        Debug.Log($"[Binary] Данные сохранены в: {path}");
    }

    public static PlayerData LoadFromBinary(string fileName = "savegame")
    {
        string path = GetBinarySavePath(fileName);

        if (!File.Exists(path))
        {
            Debug.LogWarning($"[Binary] Файл не найден: {path}");
            return null;
        }

        PlayerData data = new PlayerData();

        using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
        {
            data.playerName = reader.ReadString();
            data.level = reader.ReadInt32();
            data.experience = reader.ReadInt32();
            data.health = reader.ReadInt32();
            data.maxHealth = reader.ReadInt32();
            data.mana = reader.ReadInt32();
            data.maxMana = reader.ReadInt32();
            data.speed = reader.ReadSingle();
            data.gold = reader.ReadInt32();
            data.gems = reader.ReadInt32();
            data.posX = reader.ReadSingle();
            data.posY = reader.ReadSingle();
            data.posZ = reader.ReadSingle();
            data.musicVolume = reader.ReadSingle();
            data.sfxVolume = reader.ReadSingle();
            data.fullscreen = reader.ReadBoolean();
            data.qualityLevel = reader.ReadInt32();
            data.lastSaveTime = reader.ReadString();
            data.totalPlayTime = reader.ReadSingle();

            int inventoryCount = reader.ReadInt32();
            for (int i = 0; i < inventoryCount; i++)
                data.inventoryItems.Add(reader.ReadString());

            int levelsCount = reader.ReadInt32();
            for (int i = 0; i < levelsCount; i++)
                data.unlockedLevels.Add(reader.ReadString());
        }

        Debug.Log($"[Binary] Данные загружены из: {path}");
        return data;
    }

    public static void DeleteBinary(string fileName = "savegame")
    {
        string path = GetBinarySavePath(fileName);
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"[Binary] Файл удалён: {path}");
        }
    }


    public static bool SaveExists(string fileName = "savegame")
    {
        return File.Exists(GetSavePath(fileName)) || File.Exists(GetBinarySavePath(fileName));
    }

    public static string[] GetAllSaves()
    {
        string path = Application.persistentDataPath;
        if (!Directory.Exists(path)) return new string[0];

        string[] jsonFiles = Directory.GetFiles(path, "*.json");
        string[] datFiles = Directory.GetFiles(path, "*.dat");

        string[] allFiles = new string[jsonFiles.Length + datFiles.Length];
        jsonFiles.CopyTo(allFiles, 0);
        datFiles.CopyTo(allFiles, jsonFiles.Length);

        return allFiles;
    }

    public static string GetSaveDirectory()
    {
        return Application.persistentDataPath;
    }
}