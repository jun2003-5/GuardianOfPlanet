using System.Text;
using UnityEngine;

// External dependencies
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Interfaces;
using AppleAuth.Native;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;

public class IOSManager : MonoBehaviour
{
    IAppleAuthManager m_AppleAuthManager;
    public string Token { get; private set; }
    public string Error { get; private set; }
    public bool isLogining;
    public GameObject LoginButton;

#if UNITY_IOS
    public void Awake()
    {
        Debug.Log("UNITY_IOS");
        LoginButton.SetActive(true);
        LoginToApple();
        Invoke("AutoLoginWithIOS", 2.5f);
    }

    public void Initialize()
    {
        var deserializer = new PayloadDeserializer();
        m_AppleAuthManager = new AppleAuthManager(deserializer);
    }

    public void Update()
    {
        if(m_AppleAuthManager != null) {
            m_AppleAuthManager.Update();
        }
    }
#elif UNITY_ANDROID
    public void Awake()
    {
        LoginButton.SetActive(false);
    }
#elif UNITY_EDITOR
    public void Awake()
    {
        LoginButton.SetActive(true);
    }
#endif

    public void LoginToApple()
    {
        var loginArgs = new AppleAuthLoginArgs(AppleAuth.Enums.LoginOptions.IncludeEmail | AppleAuth.Enums.LoginOptions.IncludeFullName);

        m_AppleAuthManager.LoginWithAppleId(
            loginArgs,
            credential => {
                var appleIdCredential = credential as IAppleIDCredential;
                if(appleIdCredential != null) {
                    var userId = appleIdCredential.User;
                    var email = appleIdCredential.Email;
                    var fullName = appleIdCredential.FullName;
                    var identityToken = Encoding.UTF8.GetString(appleIdCredential.IdentityToken);
                    var authorizationCode = Encoding.UTF8.GetString(appleIdCredential.AuthorizationCode);
                    // 로그인처리
                }
            },
            error => {
                Debug.Log("Apple Signin Error");
            });
    }

    public void AutoLoginWithIOS()
    {
        if(PlayerPrefs.GetString("IOSLogIn") != "") {
            SignIn();
        }
    }

    public async void SignIn()
    {
        await SignInWithAppleAsync(Token);
    }

    public void SignInButtonClicked()
    {
        isLogining = true;
        SignIn();
        PlayerPrefs.SetString("IOSLogIn", Token);
    }

    async Task SignInWithAppleAsync(string idToken)
    {
        try {
            await AuthenticationService.Instance.SignInWithAppleAsync(idToken);
            Debug.Log("SignIn is successful.");

            StartScreenScript.Instance.isSignedIn = true;
            DataPersistenceManager.instance.LoadGame();

            if(isLogining)
                StartScreenScript.Instance.checkIfSignedIn();

            Player.instance.PlayerID = AuthenticationService.Instance.PlayerId;

        } catch(AuthenticationException ex) {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        } catch(RequestFailedException ex) {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
}
