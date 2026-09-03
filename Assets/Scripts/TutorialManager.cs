using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public GameObject Tutorial1;
    public GameObject Tutorial2;

    public railShop shop;

    public void SkipTutorial()
    {
        PlayerPrefs.SetInt("NewUser", 0);
        Tutorial1.SetActive(false);
        GameManager.instance.ResumeGame();
        NotificationManager.instance.InvokeRepeating("checkObjSetwtf", 0, 1f);

        WeaponManager.instance.mainWeapon.gameObject.SetActive(true);
        WeaponManager.instance.mainWeapon.UpgradeWeapon();

        GameManager.SetOre(30);
        shop.Upgrade();

        InfiniteStage.Instance.SetStageBeforeEntering();
    }
    public void ResumeTutorial()
    {
        Tutorial1.SetActive(false);
        Tutorial2.SetActive(true);
        ind = 0;
        nextIndex();
    }

    [Header("#------Scene 1")]
    public Button NextIndexBtn;
    public TextMeshProUGUI tutorialScene1_Text;
    public TextMeshProUGUI tutorialScene2_Text;
    public TextMeshProUGUI tutorialScene3_Text;
    public GameObject[] tutorialBubbles;
    public Transform[] tutorialHandsPosition;
    public GameObject[] tutorialHands;
    public Button[] tutorialButtons;
    public GameObject[] tutorialUIs;
    public int ind;

    public Enemy ExampleEnemy;

    Enemy SpawnedEnemy;
    public GameObject RewardTab;

    public string scene1Texts(int index)
    {
        StartCoroutine(ButtonActive());

        for(int i = 0; i < tutorialHands.Length; i++) {
            tutorialHands[i].SetActive(false);
        }

        for(int i = 0; i < tutorialButtons.Length; i++) {
            tutorialButtons[i].gameObject.SetActive(false);
        }

        for(int i = 0; i < tutorialUIs.Length; i++) {
            tutorialUIs[i].gameObject.SetActive(false);
        }

        for(int i = 0; i < tutorialBubbles.Length; i++) {
            tutorialBubbles[i].gameObject.SetActive(false);
        }

        switch(index) {
            case 0:
                tutorialBubbles[0].gameObject.SetActive(true);
                tutorialUIs[0].SetActive(true);
                return "현재 지구에는 많은 몬스터들이 존재합니다.";
            case 1:
                tutorialBubbles[0].gameObject.SetActive(true);
                setTutorialHandPosition(0);
                tutorialHands[0].SetActive(true);
                tutorialUIs[1].SetActive(true);
                return "위에 보이는 스테이지에 따라 몬스터의 체력, 경험치, 골드가 달라집니다.";
            case 2:
                tutorialBubbles[0].gameObject.SetActive(true);
                tutorialUIs[2].SetActive(true);
                setTutorialHandPosition(1);
                tutorialHands[1].SetActive(true);
                return "체력은 몬스터에게 공격 받을 시 줄어듭니다.\n전부 소모할 경우 해당 스테이지를 다시 시작합니다.";
            case 3:
                setTutorialHandPosition(2);
                tutorialBubbles[0].gameObject.SetActive(true);
                tutorialUIs[3].SetActive(true);
                tutorialHands[2].SetActive(true);
                return "해골은 남은 몬스터 수를 표시합니다.\n해당 숫자가 0이되거나 보스를 처치하면\n다음 스테이지로 넘어갑니다.";
            case 4:
                tutorialBubbles[0].gameObject.SetActive(true);
                GameManager.SetMoney(20);
                tutorialUIs[4].SetActive(true);
                return "몬스터는 우주선을 통해 처치합니다. 아래 보이는 강화를 눌러 우주선을 만들어보세요. 20골드를 지급해드리겠습니다. ";
            case 5:
                setTutorialHandPosition(3);
                tutorialHands[3].SetActive(true);
                tutorialButtons[0].gameObject.SetActive(true);
                return "";
            case 6:
                setTutorialHandPosition(4);
                tutorialHands[4].SetActive(true);
                tutorialButtons[1].gameObject.SetActive(true);
                return "";
            case 7:
                setTutorialHandPosition(5);
                tutorialHands[5].SetActive(true);
                tutorialButtons[2].gameObject.SetActive(true);
                return "";
            case 8:
                Player.instance.addAutoAttackTime_Sec(300);
                StageManager.instance.isInStage = true;
                WeaponManager.instance.shootType = WeaponManager.ShootType.NormalShoot;
                StartCoroutine(spawnEnemyAfterTime());
                return "";
            case 9:
                tutorialBubbles[0].gameObject.SetActive(true);
                return "몬스터 한마리를 소환해보겠습니다. 클릭하여 잡아보세요";
            case 10:
                tutorialBubbles[0].gameObject.SetActive(true);
                tutorialUIs[5].SetActive(true);
                return "스테이지마다 보상이 주어집니다. *보상 (골드, 강화석, 뽑기권, 다이아)";
            case 11:
                tutorialBubbles[0].gameObject.SetActive(true);
                return "더 강한 우주선을 만들기 위해 모험을 떠날 수 있어요";
            case 12:
                setTutorialHandPosition(6);
                tutorialBubbles[0].gameObject.SetActive(true);
                tutorialHands[6].SetActive(true);
                tutorialButtons[3].gameObject.SetActive(true);
                return "모험을 클릭해보세요";
            case 13:
                setTutorialHandPosition(7);
                tutorialBubbles[1].gameObject.SetActive(true);
                tutorialButtons[4].gameObject.SetActive(true);
                tutorialHands[7].SetActive(true);
                return "행성은 주인의 이름을 가져왔습니다. 첫번째 행성은 베헤모스에요. 클릭해보세요";
            case 14:
                setTutorialHandPosition(8);
                tutorialBubbles[1].gameObject.SetActive(true);
                tutorialButtons[5].gameObject.SetActive(true);
                tutorialHands[8].SetActive(true);
                return "스테이지 진입 전 행성정보를 통해 행성 몬스터의 정보를 알 수 있어요. 클릭해보세요.";
            case 15:
                tutorialBubbles[1].gameObject.SetActive(true);
                return "방금 잡았던 몬스터보다 훨씬 약합니다.";
            case 16:
                setTutorialHandPosition(9);
                tutorialBubbles[1].gameObject.SetActive(true);
                tutorialHands[9].SetActive(true);
                tutorialButtons[6].gameObject.SetActive(true);
                return "정보창을 닫고 모험을 떠나봅시다";
            case 17:
                setTutorialHandPosition(10);
                tutorialBubbles[1].gameObject.SetActive(true);
                tutorialButtons[7].gameObject.SetActive(true);
                tutorialHands[10].SetActive(true);
                return "전투 시작 버튼을 눌러보세요!";
            case 18:
                tutorialBubbles[1].gameObject.SetActive(true);
                return "5마리 몬스터가 나와요. 혼자서 클리어해보세요!";
            case 19:
                if(StageManager.instance.StageCleared.activeSelf) {
                    setTutorialHandPosition(11);
                    tutorialBubbles[1].gameObject.SetActive(true);
                    tutorialButtons[8].gameObject.SetActive(true);
                    tutorialHands[11].SetActive(true);
                    return "축하드립니다! 보상을 획득하고 모험 종료를 클릭하세요.";
                } else if(StageManager.instance.StageFailed.activeSelf) {
                    setTutorialHandPosition(12);
                    tutorialBubbles[1].gameObject.SetActive(true); 
                    tutorialButtons[9].gameObject.SetActive(true);
                    tutorialHands[12].SetActive(true);
                    ind--;
                    return "아쉽게 패배하셨어요.... 재시도를 클릭해보세요!";
                } else {
                    ind--;
                }
                return "";
            case 20:
                tutorialBubbles[0].gameObject.SetActive(true);
                tutorialUIs[6].SetActive(true);
                return "장비, 버프, 특성 등 다양한 컨텐츠로 우주선을 강화 할 수 있습니다.";
            case 21:
                tutorialUIs[7].SetActive(true);
                tutorialBubbles[0].gameObject.SetActive(true); 
                return "장비는 일반, 레어, 에픽, 유니크, 레전더리, 고대 등급이 있어요.";
            case 22:
                tutorialUIs[8].SetActive(true);
                tutorialBubbles[0].gameObject.SetActive(true);
                return "버프는 우주석으로 강화가 가능합니다. 하지만 우주석은 모험에서 구하는건 불가능해요.";
            case 23:
                tutorialUIs[9].SetActive(true);
                tutorialBubbles[0].gameObject.SetActive(true);
                return "지구에는 광산이 있는데 광산을 통해서 우주석을 제작하거나 획득할 수 있어요!";
            case 24:
                setTutorialHandPosition(13);
                tutorialBubbles[0].gameObject.SetActive(true);
                tutorialHands[13].SetActive(true);
                tutorialButtons[10].gameObject.SetActive(true);
                return "광산을 클릭해보세요.";
            case 25:
                tutorialBubbles[0].gameObject.SetActive(true);
                tutorialHands[14].SetActive(true);
                tutorialButtons[11].gameObject.SetActive(true);
                return "여기는 광산입니다. 한번 화면을 터치해보세요!";
            case 26:
                tutorialBubbles[0].gameObject.SetActive(true);
                return "화면을 터치하면 기본 광석을 획득할 수 있어요!";
            case 27:
                setTutorialHandPosition(15);
                tutorialBubbles[0].gameObject.SetActive(true);
                tutorialHands[15].SetActive(true);
                tutorialButtons[12].gameObject.SetActive(true);
                return "아래 광석 버튼을 눌러보세요.";
            case 28:
                setTutorialHandPosition(16);
                tutorialBubbles[1].gameObject.SetActive(true);
                tutorialHands[16].SetActive(true);
                tutorialButtons[13].gameObject.SetActive(true);
                GameManager.SetOre(30);
                return "지구에는 여러가지 종류의 돌이 있어요. 첫 번째 광석을 구매해보세요.";
            case 29:
                tutorialBubbles[0].gameObject.SetActive(true);
                return "광석마다 레일과 드릴이 있어요. 레일을 통해 광석이 전달되면 위 또는 아래로 나누어집니다.";
            case 30:
                tutorialBubbles[0].gameObject.SetActive(true);
                return "위는 드릴로 채굴이 가능합니다.";
            case 31:
                tutorialBubbles[0].gameObject.SetActive(true);
                return "반대로 아래는 미확인 광석으로 저장소에 저장됩니다.";
            case 32:
                tutorialBubbles[0].gameObject.SetActive(true);
                tutorialButtons[14].gameObject.SetActive(true);
                return "드릴을 클릭해보세요.";
            case 33:
                tutorialBubbles[2].gameObject.SetActive(true);
                Railmanager.Instance.openRailInfoTab(Railmanager.Instance.currentRails[0]);
                return "드릴로 채굴한 광석은 온전한 광석으로 저장됩니다.";
            case 34:
                tutorialBubbles[2].gameObject.SetActive(true);
                return "온전한 광석으로 데미지, 공격속도 또는 행운을 강화할 수 있어요.";
            case 35:
                setTutorialHandPosition(17);
                tutorialBubbles[2].gameObject.SetActive(true);
                tutorialHands[17].SetActive(true);
                tutorialButtons[15].gameObject.SetActive(true); 
                return "제철소를 클릭해보세요!";
            case 36:
                tutorialBubbles[2].gameObject.SetActive(true);
                return "아래로 내려간 광석은 미확인 광석으로 제철소를 통해 교환이 가능해요!";
            case 37:
                tutorialBubbles[2].gameObject.SetActive(true);
                return "광석 가치에 따라 교환 양이 달라지니 주의하세요!";
            case 38:
                setTutorialHandPosition(18);
                tutorialBubbles[2].gameObject.SetActive(true);
                tutorialHands[18].SetActive(true);
                tutorialButtons[16].gameObject.SetActive(true); 
                return "창을 닫고 사냥터로 돌아가보세요";
            case 39:
                setTutorialHandPosition(19);
                tutorialHands[19].SetActive(true);
                tutorialButtons[17].gameObject.SetActive(true);
                return "";
            case 40:
                tutorialBubbles[0].gameObject.SetActive(true);
                return "최강의 우주선을 만들어 행성을 지켜주세요!";
            case 41:
                tutorialBubbles[0].gameObject.SetActive(true);
                return "행운을 빕니다.";
            case 42:
                tutorialBubbles[0].gameObject.SetActive(true);
                return "모든 튜토리얼이 끝났습니다. 튜터리얼 보상과 함께 게임을 시작합니다.";
            default:
                return "";
        }
    }

    public void setTutorialHandPosition(int index)
    {
        tutorialHands[index].transform.position = new Vector3(tutorialHandsPosition[index].position.x, tutorialHandsPosition[index].position.y, tutorialHandsPosition[index].position.z);
    }

    IEnumerator ButtonActive()
    {
        NextIndexBtn.interactable = false;
        for(int i = 0; i < tutorialButtons.Length; i++) {
            tutorialButtons[i].interactable = false;
        }
        yield return new WaitForSeconds(1);

        NextIndexBtn.interactable = true;
        for(int i = 0; i < tutorialButtons.Length; i++) {
            tutorialButtons[i].interactable = true;
        }

        if(SpawnedEnemy != null) {
            if(SpawnedEnemy.gameObject.activeSelf) {
                NextIndexBtn.interactable = false;
                for(int i = 0; i < tutorialButtons.Length; i++) {
                    tutorialButtons[i].interactable = false;
                }
            }
        }
    }

    IEnumerator spawnEnemyAfterTime()
    {
        NextIndexBtn.interactable = false;
        for(int i = 0; i < tutorialButtons.Length; i++) {
            tutorialButtons[i].interactable = false;
        }
        yield return new WaitForSeconds(1);
        Enemy enemy = ObjectPoolEnemy.Instance.GetPoolObject(ExampleEnemy.typePool).GetComponent<Enemy>();
        SpawnedEnemy = enemy;
        float pos_X = 0;
        float pos_Y = Camera.main.ScreenToWorldPoint(EnemyManager.Instance.TopEnemySpawnLoc.position).y + 1f;
        enemy.transform.position = new Vector3(pos_X, pos_Y, 0);
        enemy._typeofEnemy = Enemy.typeofEnemy.LabMonster;
        enemy.gameObject.SetActive(true);
        enemy.EnemyHealth = 70;
        enemy.enemyMovingSpeed = 0.2f;
        enemy.CurrentHealth = 70;
        EnemyManager.Instance.enemyData.Add(enemy);

        float totalTime = 0;
        while(SpawnedEnemy.gameObject.activeSelf) {
            NextIndexBtn.interactable = false;
            for(int i = 0; i < tutorialButtons.Length; i++) {
                tutorialButtons[i].interactable = false;
            }
            yield return new WaitForSeconds(0.5f);
            totalTime += Time.deltaTime;
            if(totalTime > 7) {
                enemy.enemyMovingSpeed = 0;
            }
        }
        for(int i = 0; i < tutorialButtons.Length; i++) {
            tutorialButtons[i].interactable = true;
        }
        NextIndexBtn.interactable = true;
        nextIndex();
    }

    public void nextIndex()
    {
        tutorialScene1_Text.text = scene1Texts(ind);
        tutorialScene2_Text.text = tutorialScene1_Text.text;
        tutorialScene3_Text.text = tutorialScene2_Text.text;
        ind++;
        if(ind == 43) {
            RewardTab.SetActive(true);
            GameManager.SetMoney(5000);
            Tutorial1.SetActive(false);
            Tutorial2.SetActive(false);
            GameManager.instance.ResumeGame();
            NotificationManager.instance.InvokeRepeating("checkObjSetwtf", 0, 1f);
            PlayerPrefs.SetInt("NewUser", 0);
        }
    }
}
