#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using TMPro;

public class GPGSManager : MonoBehaviour
{
    public string Token;
    public bool isLogining;
    public GameObject LoginButton;

#if UNITY_ANDROID

    async void Awake()
    {
        LoginButton.SetActive(true);
        await UnityServices.InitializeAsync();

        PlayGamesPlatform.Activate();
        LoginGooglePlayGames();

        Invoke("AutoLoginWithGoogle", 2.5f);
    }

    public void LoginGooglePlayGames()
    {
        PlayGamesPlatform.Instance.Authenticate((success) => {
            if(success == SignInStatus.Success) {
                Debug.Log("Login with Googld Play games successful.");

                PlayGamesPlatform.Instance.RequestServerSideAccess(true, code => {
                    Debug.Log("Authorization code: " + code);
                    Token = code;
                });
            } else {
                Debug.Log("Login Unsuccessful");
            }
        });
    }
#elif UNITY_IOS
    public void Awake()
    {
        LoginButton.SetActive(false);
    }
#endif

    public void AutoLoginWithGoogle()
    {
        if(PlayerPrefs.GetString("GoogleLogIn") != "") {
            SignIn();
        }
    }

    public void SignInButtonClicked()
    {
        isLogining = true;
        SignIn();
        PlayerPrefs.SetString("GoogleLogIn", Token);
    }

    public async void SignIn()
    {
        await SignInWithGooglePlayGamesAsync(Token);
    }

    async Task SignInWithGooglePlayGamesAsync(string authCode)
    {
        try {
            await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(authCode);
            Debug.Log("SignIn is successful");

            StartScreenScript.Instance.isSignedIn = true;
            DataPersistenceManager.instance.LoadGame();

            if(isLogining)
                StartScreenScript.Instance.checkIfSignedIn();

            Player.instance.PlayerID = AuthenticationService.Instance.PlayerId;
        } catch(AuthenticationException ex) {

            Debug.LogException(ex);
        } catch(RequestFailedException ex) {

            Debug.LogException(ex);
        }
    }
}
