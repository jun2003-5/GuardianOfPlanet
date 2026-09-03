using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WebSocketSharp;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unity.Services.Authentication;

public class UserNameManager : MonoBehaviour
{
    public static UserNameManager Instance;

    public string UserName;
    public GameObject UserNameSetTab;

    public bool DuplicateChecked;
     
    public TMP_InputField userNameInputField;
    public TextMeshProUGUI duplicateConditionText;

    public GameObject ErrorTab;
    public TextMeshProUGUI ErrorName;

    public string[] lines;
    string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";


    private void Awake()
    {
        Instance = this;
        if(File.Exists("Assets/Scripts/Player/BadWord.txt")) {
            StreamReader word = new StreamReader("Assets/Scripts/Player/BadWord.txt");
            string source = word.ReadToEnd();
            word.Close();
            lines = Regex.Split(source, LINE_SPLIT_RE);
        }
    }

    public void checkIfUserNameSet()
    {
        if(PlayerPrefs.HasKey("username")) {
            if(PlayerPrefs.GetInt("username") == 0) {
                openUserNameTab();
            }
        } else {
            openUserNameTab();
            PlayerPrefs.SetInt("username", 1);
        }
    }

    public void openUserNameTab()
    {
        UserNameSetTab.SetActive(true);
    }

    public async void setUserName()
    {
        checkDuplication();
        await Task.Delay(300);
        if(DuplicateChecked) {
            UserName = userNameInputField.text;
            UserNameSetTab.SetActive(false);
            await AuthenticationService.Instance.UpdatePlayerNameAsync(UserName);
            Player.instance.PlayerName = await AuthenticationService.Instance.GetPlayerNameAsync();
            PlayerPrefs.SetInt("username", 1);
            StartScreenScript.Instance.checkIfSignedIn();
            LeaderBoardManager.Instance.AddInfiniteStageScore();
        } else {
            ErrorTab.SetActive(true);
        }
    }

    public void checkDuplication()
    {
        LeaderBoardManager.Instance.checkDuplication(userNameInputField.text);
    }

    public void setConditionText(bool a)
    {
        if(!userNameInputField.text.IsNullOrEmpty()) {
            if(userNameInputField.text.Contains(" ") || userNameInputField.text.Trim().Length != userNameInputField.text.Length) {
                duplicateConditionText.color = new Color(0.9056604f, 0, 0.01579501f);
                duplicateConditionText.text = "닉네임에는 공백이나 빈 공간을 포함할 수 없습니다.";
                ErrorName.text = "올바른 닉네임을 입력해주세요.";
                DuplicateChecked = false;
                return;
            }

            for(int i = 0; i < lines.Length; i++) {
                if(userNameInputField.text.Contains(lines[i])) {
                    duplicateConditionText.color = new Color(0.9056604f, 0, 0.01579501f);
                    duplicateConditionText.text = "비속어는 사용할 수 없습니다";
                    ErrorName.text = "부적절한 단어를 포함하고있습니다";
                    DuplicateChecked = false;
                    return;
                }
            }

            if(userNameInputField.text.Length < 2) {
                duplicateConditionText.color = new Color(0.9056604f, 0, 0.01579501f);
                duplicateConditionText.text = "닉네임이 너무 짧습니다.";
                ErrorName.text = "2자~8자 사이 닉네임으로 설정해주세요";
                DuplicateChecked = false;
                return;
            }

            if(userNameInputField.text.Length > 8) {
                duplicateConditionText.color = new Color(0.9056604f, 0, 0.01579501f);
                duplicateConditionText.text = "닉네임이 너무 깁니다.";
                ErrorName.text = "2자~8자 사이 닉네임으로 설정해주세요";
                DuplicateChecked = false;
                return;
            }

            if(a) {
                duplicateConditionText.color = new Color(0, 0.5566038f, 0.0490791f);
                duplicateConditionText.text = "사용가능한 이름입니다";
                DuplicateChecked = true;
            } else {
                duplicateConditionText.color = new Color(0.9056604f, 0, 0.01579501f);
                duplicateConditionText.text = "사용 불가능한 이름입니다";
                ErrorName.text = "중복된 닉네임입니다";
                DuplicateChecked = false;
            }
        } else {
            duplicateConditionText.color = new Color(0.9056604f, 0, 0.01579501f);
            duplicateConditionText.text = "사용 불가능한 이름입니다";
            ErrorName.text = "닉네임을 입력해주세요";
            DuplicateChecked = false;
        }
    }
}
