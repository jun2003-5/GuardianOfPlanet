using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.CloudCode;
using Unity.Services.Core;
using UnityEngine;

/*
* Note that you need to have a published script in order to use the Cloud Code SDK.
* You can do that from the Unity Dashboard - https://dashboard.unity3d.com/
*/
public class CloudCode : MonoBehaviour
{
    public void OnClick()
    {
        CallMethod();
    }
    // ResultType structure is the serialized response from the RollDice script in Cloud Code
    private class ResultType
    {
        public Dictionary<string, object> Result;
        public string UserName;
        public bool HasSucceed;
    }

    public async void CallMethod()
    {
        // Call out to the Roll Dice script in Cloud Code
        var response = await CloudCodeService.Instance.CallEndpointAsync<ResultType>("CheckUserName", new Dictionary<string, object>() { { "UserName", AuthenticationService.Instance.PlayerId } });

        // Log the response of the script in console
        Debug.Log($"{response.UserName} / {response.HasSucceed} / {response.Result}");
    }
}