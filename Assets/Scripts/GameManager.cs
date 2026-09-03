using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text.RegularExpressions;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager instance;

    [Header("카메라")]
    public Camera mainCamera;

    [Header("스타트")]
    public GameObject StartScreen;

    [Header("Ingame Money")]
    public long money;
    public int Diamond;
    public long Ore;
    public long Parts;

    public long maxMoney;

    [Header("Game Information")]
    public float totalPlayedTime;

    [Header("GameSpeed")]
    public bool GameSpeedBought;

    [Header("#---Wifi Error")]
    public GameObject WifiErrorTab;

    [Header("#----Quit Tab")]
    public GameObject QuitTab;

    [Header("#----Real time")]
    public GameObject RealTimeErrorTab;
    const string API_URL = "https://worldtimeapi.org/api/ip";
    DateTime currentDateTime = DateTime.Now;

    float temp = 1;

    bool checkTimeDifferrence;
    public bool noAdsBought;

    private void Awake()
    {
        Time.timeScale = 1;
        instance = this;

        //스크린 타임 방지
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.runInBackground = true;

        InvokeRepeating("checkRealTimeError", 0, 1f);

        RemoteSettings.ForceUpdate();
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        StartScreen.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        Canvas.ForceUpdateCanvases();
        if(Time.timeScale > 0.9f) {
            //TotalPlayedTime
            totalPlayedTime += Time.deltaTime / Time.timeScale;

            //최대 돈
            if(money >= maxMoney)
                maxMoney = money;
        }

        //Wifi connection
        if(Application.internetReachability == NetworkReachability.NotReachable && !WifiErrorTab.activeSelf) {
            Error();
        }

        //Escape Button
#if UNITY_ANDROID
        if(Input.GetKeyDown(KeyCode.Escape)) {
            QuitTab.SetActive(true);
            PauseGame();
        }
#endif
    }

    public void checkRealTimeError()
    {
        if(checkTimeDifferrence) {
            if(!RealTimeErrorTab.activeSelf) {
                StartCoroutine(GetRealDateTimeFromAPI());
            }
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if(!hasFocus) {
            checkTimeDifferrence = false;
        } else {
            if(checkTimeDifferrence == false) {
                currentDateTime = DateTime.Now;
            }
            checkTimeDifferrence = true;
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if(pauseStatus) {
            checkTimeDifferrence = false;
        }
    }

    IEnumerator GetRealDateTimeFromAPI()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(API_URL);
        yield return webRequest.SendWebRequest();

        if(webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError) {
        } else {
            string timeData = webRequest.downloadHandler.text;
            currentDateTime = ParseDateTime(timeData);
        }
        if(Mathf.Abs((float)(currentDateTime - DateTime.Now).TotalMinutes) > 15) { 
            Time.timeScale = 0;
            RealTimeErrorTab.SetActive(true);
        }
    }

    DateTime ParseDateTime(string dateTime)
    {
        string date = Regex.Match(dateTime, @"\d{4}-\d{2}-\d{2}").Value;
        string time = Regex.Match(dateTime, @"\d{2}:\d{2}:\d{2}").Value;
        return DateTime.Parse(string.Format("{0} {1}", date, time));
    }

    public void Error()
    {
        Time.timeScale = 0;
        WifiErrorTab.SetActive(true);
    }

    public void ErrorTabOkay()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif  
    }

    public void PauseGame()
    {
        temp = Time.timeScale;
        Time.timeScale = 0.0000001f;
    }
    public void ResumeGame()
    {
        if(temp < 1) {
            temp = 1;
        }
        setTimeScale(temp);
    }

    public void setTimeScale(float a)
    {
        Time.timeScale = a;
    }

    public void saveGameSpeedandAuto(float a, bool b)
    {
        if(a >= 1)
            PlayerPrefs.SetFloat("lastGameSpeed", a);
        PlayerPrefs.SetInt("lastAutoAttack", b ? 1 : 0);
    }

    public void loadGameSpeedandAuto()
    {
        if(PlayerPrefs.HasKey("lastGameSpeed")) {
            if(PlayerPrefs.GetFloat("lastGameSpeed") < 1) {
                PlayerPrefs.SetFloat("lastGameSpeed", 1);
            }
            setTimeScale(PlayerPrefs.GetFloat("lastGameSpeed"));
        } else {
            PlayerPrefs.SetFloat("lastGameSpeed", 1);
            setTimeScale(PlayerPrefs.GetFloat("lastGameSpeed"));
        }

        if(PlayerPrefs.HasKey("lastAutoAttack")) {
            WeaponManager.instance.shootType = PlayerPrefs.GetInt("lastAutoAttack") == 1 ? WeaponManager.ShootType.AutoShoot : WeaponManager.ShootType.NormalShoot;
        } else {
            PlayerPrefs.SetInt("lastAutoAttack", 0);
            WeaponManager.instance.shootType = PlayerPrefs.GetInt("lastAutoAttack") == 1 ? WeaponManager.ShootType.AutoShoot : WeaponManager.ShootType.NormalShoot;
        }

        saveGameSpeedandAuto(Time.timeScale, WeaponManager.instance.shootType == WeaponManager.ShootType.AutoShoot);
    }

    public static string MoneyStringForGamemanager(long M)
    {
        if(M < 10000) {
            return string.Format("{0:#,###0}", M);
        } else if(M < 1000000) {
            return ((float)(M / 1000.0)).ToString("F2") + "K";
        } else if(M < 1000000000) {
            return ((float)(M / 1000000.0)).ToString("F2") + "M";
        } else if(M < 1000000000000) {
            return ((float)(M / 1000000000.0)).ToString("F2") + "B";
        } else if(M < 1000000000000000) {
            return ((float)(M / 1000000000000.0)).ToString("F2") + "T";
        } else if(M < 1000000000000000000)
        {
            return ((float)(M / 1000000000000000.0)).ToString("F2") + "Q";
        }
        return "";
    }
    public static string MoneyString(long M)
    {
        if(M < 10000) {
            return string.Format("{0:#,##0.##}", M);
        } else if(M < 1000000) {
            return string.Format("{0:#,###.##}K", M / 1000.0f);
        } else if(M < 1000000000) {
            return string.Format("{0:#,###.##}M", M / 1000000.0f);
        } else if(M < 1000000000000) {
            return string.Format("{0:#,###.##}B", M / 1000000000.0);
        } else if(M < 1000000000000000) {
            return string.Format("{0:#,###.##}T", M / 1000000000000.0);
        } else if(M < 1000000000000000000) {
            return string.Format("{0:#,###.##}Q", M / 1000000000000000.0);
        }
        return "";
    }

    public static string MoneyStringForShop(long M)
    {
        if(M < 10000) {
            return string.Format("{0:#,###0}", M);
        } else if(M < 1000000) {
            return Mathf.RoundToInt((float)(M / 1000.0)) + "K";
        } else if(M < 1000000000) {
            return Mathf.RoundToInt((float)(M / 1000000.0)) + "M";
        } else if(M < 1000000000000) {
            return Mathf.RoundToInt((float)(M / 1000000000.0)) + "B";
        } else if(M < 1000000000000000) {
            return Mathf.RoundToInt((float)(M / 1000000000000.0)) + "T";
        } else if(M < 1000000000000000000) {
            return string.Format("{0:#,###.##}Q", M / 1000000000000000.0);
        }
        return "";
    }

    public static string MoneyStringForTower(long M)
    {
        if(M < 1000) {
            return string.Format("{0:#,###0}", M);
        } else if(M < 1000000) {
            return Mathf.RoundToInt((float)(M / 1000.0)) + "K";
        } else if(M < 1000000000) {
            return Mathf.RoundToInt((float)(M / 1000000.0)) + "M";
        } else if(M < 1000000000000) {
            return Mathf.RoundToInt((float)(M / 1000000000.0)) + "B";
        } else if(M < 1000000000000000) {
            return Mathf.RoundToInt((float)(M / 1000000000000.0)) + "T";
        } else if(M < 1000000000000000000) {
            return string.Format("{0:#,###.##}Q", M / 1000000000000000.0);
        }
        return "";
    }

    public string MoneyStringForAchievement(long M)
    {
        if(M < 10000) {
            return string.Format("{0:#,###0}", M);
        } else if(M < 1000000) {
            return string.Format("{0:#,###0.##}", (float)(M / 1000.0)) + "K";
        } else if(M < 1000000000) {
            return ((float)(M / 1000000.0)).ToString("F2") + "M";
        } else if(M < 1000000000000) {
            return ((float)(M / 100000000.0)).ToString("F2") + "B";
        } else if(M < 1000000000000000) {
            return ((float)(M / 100000000000.0)).ToString("F2") + "T";
        } else if(M < 1000000000000000000) {
            return string.Format("{0:#,###.##}Q", M / 1000000000000000.0);
        }
        return "";
    }

    public string moneyStringForMine(long M)
    {
        if(M < 1000000) {
            return string.Format("{0:#,##0}", M);
        } else if(M < 1000000000) {
            return string.Format("{0:#,###}M", (float)(M / 1000000));
        } else if(M < 1000000000000) {
            return string.Format("{0:#,###}B", (float)(M / 1000000000));
        } else if(M < 1000000000000000) {
            return string.Format("{0:#,###}T", (float)(M / 1000000000000));
        } else if(M < 1000000000000000000) {
            return string.Format("{0:#,###.##}Q", M / 1000000000000000.0);
        }
        return "";
    }


    public string MoneyStringForInfiniteStage(long M)
    {
        if(M < 10000) {
            return string.Format("{0:#,##0}", M);
        } else if(M < 1000000) {
            return string.Format("{0:#,###}K", (float)(M / 1000));
        } else if(M < 1000000000) {
            return string.Format("{0:#,###}M", (float)(M / 1000000));
        } else if(M < 1000000000000) {
            return string.Format("{0:#,###}B", (float)(M / 1000000000));
        } else if(M < 1000000000000000) {
            return string.Format("{0:#,###}T", (float)(M / 1000000000000));
        } else if(M < 1000000000000000000) {
            return string.Format("{0:#,###.##}Q", M / 1000000000000000.0);
        }
        return "";
    }

    //경험치 올리기
    public static void Enemy_Dead_Player_EXP_UP(int exp)
    {
        Player.instance.PlayerEXPUP(exp);
    }

    //돈 올리기
    public static void Enemy_Dead_Money_UP(long money)
    {
        if(instance.money + money > 0)
            instance.money += money;
        else {
            instance.money = 0;
        }
    }

    public static int GetTotalExpOfEnemy(Enemy enemy)
    {
        return (int)((enemy.Enemy_EXP + EquipingManager.Instance.ExtraEXPByNumber_Equip) * (1 + Player.GetStatsByType(Upgrade_StatsPrefab.StatsType.ExtraEXP)));
    }

    public static long GetTotalGoldOfEnemy(Enemy enemy)
    {
        return (long)((enemy.enemy_Money + EquipingManager.Instance.ExtraGoldByNumber_Equip) * (1 + Player.GetStatsByType(Upgrade_StatsPrefab.StatsType.ExtraMoney)));
    }

    public static int GetDiamond()
    {
        return instance.Diamond;
    }

    public static void SetDiamond(int n)
    {
        instance.Diamond += n;
    }

    public void addDiamond(int n) => Diamond += n;

    public static long GetOre()
    {
        return instance.Ore;
    }

    public static void SetOre(long n)
    {
        instance.Ore += n;
    }

    public void addOre(long n) => Ore += n;

    public static long GetMoney()
    {
        return instance.money;
    }

    public static void SetMoney(long n)
    {
        instance.money += n;
    }

    public void addMoney(long n) => money += n;

    public static long GetParts()
    {
        return instance.Parts;
    }

    public static void SetParts(long n)
    {
        instance.Parts += n;
    }

    public void addParts(long n) => Parts += n;

    public static void SetTimeScale()
    {
        if(Time.timeScale <= 1)
            Time.timeScale = 1.5f;
        else if(Time.timeScale == 1.5f) {
            if(instance.GameSpeedBought)
                Time.timeScale = 2f;
            else
                Time.timeScale = 1f;
        } else if (Time.timeScale == 2f){
            Time.timeScale = 1;
        } else if(Time.timeScale >= 3.5f)
            Time.timeScale = 1f;

        instance.saveGameSpeedandAuto(Time.timeScale, WeaponManager.instance.shootType == WeaponManager.ShootType.AutoShoot);
    }


    public void LoadData(GameData data)
    {
        this.money = data.Money;
        this.Diamond = data.Diamond;
        this.Ore = data.Ores;
        this.Parts = data.parts;
        this.totalPlayedTime = data.totalPlayedTime;
        this.maxMoney = data.maxMoney;
        this.GameSpeedBought = data.gameSpeedBought;
        noAdsBought = data.noAdsBought;
    }

    public void SaveData(GameData data)
    {
        data.Money = this.money;
        data.Diamond = this.Diamond;
        data.Ores = this.Ore;
        data.parts = this.Parts;
        data.totalPlayedTime = this.totalPlayedTime;
        data.maxMoney = this.maxMoney;
        data.gameSpeedBought = this.GameSpeedBought;
        data.noAdsBought = noAdsBought;
    }
}
