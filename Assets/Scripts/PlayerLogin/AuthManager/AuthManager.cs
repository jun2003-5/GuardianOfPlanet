using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using TMPro;

public class AuthManager : MonoBehaviour
{
    public bool isLogining;

    async void Awake()
    {
        await UnityServices.InitializeAsync();
        await CheckAutoLogIn();
    }

    public async Task CheckAutoLogIn()
    {
        if(PlayerPrefs.GetString("GuestLogIn") == "true") {
            await SignInAnonymouslyAsync();
        }
    }

    public async void SignIn()
    {
        isLogining = true;
        await SignInAnonymouslyAsync();
    }

    async Task SignInAnonymouslyAsync()
    {
        try {

            PlayerPrefs.SetString("GuestLogIn", "true");

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            StartScreenScript.Instance.isSignedIn = true;
            DataPersistenceManager.instance.LoadGame();

            if(isLogining)
                StartScreenScript.Instance.checkIfSignedIn();

            Player.instance.PlayerID = AuthenticationService.Instance.PlayerId;
            Debug.Log(AuthenticationService.Instance.PlayerId);

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

    public async void testmethod()
    {
        string s = await AuthenticationService.Instance.GetPlayerNameAsync();
        Debug.Log(s);
    }
}
