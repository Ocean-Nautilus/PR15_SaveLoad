using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "PR15/Item Data", order = 1)]
public class ItemData : ScriptableObject
{
    [Header("Основная информация")]
    public string itemName = "Новый предмет";
    [TextArea(3, 5)]
    public string description = "Описание предмета";

    [Header("Характеристики")]
    public int damage = 0;
    public int defense = 0;
    public float weight = 1f;
    public int price = 10;

    [Header("Визуал")]
    public Sprite icon;
    public Color itemColor = Color.white;

    [Header("Категория")]
    public ItemCategory category = ItemCategory.Misc;
    public ItemRarity rarity = ItemRarity.Common;

    public string GetFullInfo()
    {
        string info = $"<b>{itemName}</b>\n";
        info += $"<i>{description}</i>\n";
        info += $"Категория: {GetCategoryName()}\n";
        info += $"Редкость: {GetRarityName()}\n";
        info += $"Урон: {damage}\n";
        info += $"Защита: {defense}\n";
        info += $"Вес: {weight:F1} кг\n";
        info += $"Цена: {price} золота\n";
        return info;
    }

    private string GetCategoryName()
    {
        switch (category)
        {
            case ItemCategory.Weapon: return "Оружие";
            case ItemCategory.Armor: return "Броня";
            case ItemCategory.Potion: return "Зелье";
            case ItemCategory.Misc: return "Разное";
            default: return "Неизвестно";
        }
    }

    private string GetRarityName()
    {
        switch (rarity)
        {
            case ItemRarity.Common: return "Обычный";
            case ItemRarity.Rare: return "Редкий";
            case ItemRarity.Epic: return "Эпический";
            case ItemRarity.Legendary: return "Легендарный";
            default: return "Неизвестно";
        }
    }
}

public enum ItemCategory
{
    Weapon,
    Armor,
    Potion,
    Misc
}

public enum ItemRarity
{
    Common,
    Rare,
    Epic,
    Legendary
}