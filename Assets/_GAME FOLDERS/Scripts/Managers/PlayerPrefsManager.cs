using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    private const string ZombieKillCountKey = "ZombieKillCount";
    private const string HighestZombieKillCountKey = "HighestZombieKillCount";
    public static int ZombieKillCount
    {
        get { return PlayerPrefs.GetInt(ZombieKillCountKey, 0); }
        set
        {
            PlayerPrefs.SetInt(ZombieKillCountKey, value);
            int highestKillCount = HighestZombieKillCount;
            if (value > highestKillCount)
            {
                HighestZombieKillCount = value;
            }
        }
    }

    public static int HighestZombieKillCount
    {
        get { return PlayerPrefs.GetInt(HighestZombieKillCountKey, 0); }
        private set { PlayerPrefs.SetInt(HighestZombieKillCountKey, value); }
    }

    public static void IncrementZombieKillCount()
    {
        ZombieKillCount++;
    }
}
