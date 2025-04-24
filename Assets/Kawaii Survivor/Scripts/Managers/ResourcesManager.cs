using UnityEngine;

public static class ResourcesManager
{
    const string STAT_ICONS_DATA_PATH = "Data/Stat Icons";
    const string OBJECTS_DATA_PATH = "Data/Objects/";
    const string WEAPONS_DATA_PATH = "Data/Weapons/";

    private static StatIcon[] m_statIcons;
    private static ObjectDataSO[] m_objectDatas;
    private static WeaponDataSO[] m_weaponDatas;


    public static Sprite GetStatIcon(Stat stat)

    {
        if (m_statIcons == null)
        {
            StatIconDataSO statIconsData = Resources.Load<StatIconDataSO>(STAT_ICONS_DATA_PATH);
            m_statIcons = statIconsData.StatIcons;
        }

        foreach (StatIcon statIcon in m_statIcons)
        {
            if (statIcon.stat == stat)
            {
                return statIcon.icon;
            }
        }

        Debug.LogError($"Stat icon for {stat} not found in Resources.");

        return null;
    }

    public static ObjectDataSO[] Objects
    {
        get
        {
            if (m_objectDatas == null)
            {
                m_objectDatas = Resources.LoadAll<ObjectDataSO>(OBJECTS_DATA_PATH);
            }

            return m_objectDatas;
        }
        private set { }
    }

    public static ObjectDataSO GetRandomObject()
    {
        return Objects[Random.Range(0, Objects.Length)];
    }

    public static WeaponDataSO[] Weapons
    {
        get
        {
            if (m_weaponDatas == null)
            {
                m_weaponDatas = Resources.LoadAll<WeaponDataSO>(WEAPONS_DATA_PATH);
            }

            return m_weaponDatas;
        }
        private set { }
    }

    public static WeaponDataSO GetRandomWeapon()
    {
        return Weapons[Random.Range(0, Weapons.Length)];
    }
}
