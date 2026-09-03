using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class StartScreenScript : MonoBehaviour
{
    public static StartScreenScript Instance;

    public TutorialManager tutorialManager;

    public GameObject[] tabs;

    [Header("#------UIs")]
    public TextMeshProUGUI loadingProgress;
    public TextMeshProUGUI versionText;
    public GameObject LoadingUI;
    public Slider loadingBar;
    public GameObject LoadingCircle;
    public GameObject LoginTab;

    [Header("#----Terms Of Service")]
    public GameObject TermsTab;
    public Toggle Toggle1;
    public Toggle Toggle2;
    public GameObject TermsErrorTab;

    public bool isSignedIn;

    public void Awake()
    {
        Instance = this;

        if(!PlayerPrefs.HasKey("NewUser")) {
            PlayerPrefs.SetInt("NewUser", 1);
        }
    }

    public void OnEnable()
    {
        versionText.text = "Ver. " + Application.version;
        LoadingUI.SetActive(true);
        loadingBar.gameObject.SetActive(true);
        LoginTab.SetActive(false);
        StartCoroutine(loadTabs());

        SoundManager.Instance.playMusic(SoundManager.MusicType.loadingScreen);
    }

    IEnumerator loadTabs()
    {
        loadingBar.maxValue = tabs.Length;
        for(int i = 0; i < tabs.Length; i++) {
            loadingBar.value = i + 1;
            tabs[i].SetActive(true);
            yield return new WaitForSeconds(0.2f);
            tabs[i].SetActive(false);

            loadingProgress.text = ((loadingBar.value / loadingBar.maxValue) * 100).ToString("F0") + "%";
        }
        loadingBar.value = loadingBar.maxValue;
        loadingProgress.text = "100%";

        yield return new WaitForSeconds(3);
        LoadingUI.gameObject.SetActive(false);

        //StageManager
        StageManager.instance.checkAllPlanetState();

        checkIfSignedIn();
    }

    void Update()
    {
        if(LoadingUI.gameObject.activeSelf) {
            LoadingCircle.transform.Rotate(new Vector3(0, 0, 2f));
        }
    }

    public void checkIfSignedIn()
    {
        if(isSignedIn) {
            LoginTab.SetActive(false);
            gameObject.SetActive(false);

            if(PlayerPrefs.GetInt("username") == 0) {
                UserNameManager.Instance.checkIfUserNameSet();
                return;
            }

            if(PlayerPrefs.GetInt("NewUser") == 0) {
                NotificationManager.instance.InvokeRepeating("checkObjSetwtf", 0, 1f);
                InfiniteStage.Instance.SetStageBeforeEntering();
            } else {
                InfiniteStage.Instance.SetStageBeforeEntering();
                InfiniteStage.Instance.PauseInfiniteStage();
                tutorialManager.Tutorial1.SetActive(true);
            }

            LeaderBoardManager.Instance.setLeaderBoard();

        } else {
            TermsTab.SetActive(true);
        }
    }

    public void agreeAll()
    {
        Toggle1.isOn = true;
        Toggle2.isOn = true;
    }

    public void checkTermsAgreed()
    {
        if(Toggle1.isOn && Toggle2.isOn) {
            PlayerPrefs.SetInt("NewUser", 1);
            PlayerPrefs.SetInt("username", 0);
            LoginTab.SetActive(true);
        } else {
            TermsErrorTab.SetActive(true);
        }
    }
}
