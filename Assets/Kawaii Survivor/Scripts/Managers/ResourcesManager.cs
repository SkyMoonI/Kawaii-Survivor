using UnityEngine;

public static class ResourcesManager
{
    const string STAT_ICONS_DATA_PATH = "Data/Stat Icons";

    private static StatIcon[] m_statIcons;

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
}
