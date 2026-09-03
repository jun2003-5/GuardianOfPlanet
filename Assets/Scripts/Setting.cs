using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine.Audio;

public class Setting : MonoBehaviour 
{
    [Header("Reset")]
    public GameObject WarningTab;

    [Header("Start Screen")]
    public GameObject StartScreen;

    [Header("Audio Mixer")]
    public AudioMixer audioController;
    public Slider[] musicSliders;
    public Slider[] SFXSliders;

    private void Start()
    {
        if(PlayerPrefs.HasKey("SFXVolume")) {
            LoadSFXVolume();
        } else {
            SFXSliders[0].value = SFXSliders[0].maxValue;
            changeSFXVolume(SFXSliders[0]);
        }

        if(PlayerPrefs.HasKey("MusicVolume")) {
            LoadMusicVolume();
        } else {
            musicSliders[0].value = musicSliders[0].maxValue;
            changeMusicVolume(musicSliders[0]);
        }
    }

    public void CopyToClipboard(TextMeshProUGUI IDText)
    {
        TextEditor textEditor = new TextEditor();
        textEditor.text = IDText.text;
        textEditor.SelectAll();
        textEditor.Copy();
    }

    public void PasteToClipboard(TMP_InputField field)
    {
        TextEditor textEditor = new TextEditor();
        textEditor.Paste();
        field.text = textEditor.text;
    }

    public void ResetGame(TMP_InputField inputField)
    {
        if(inputField.text == "√ ±‚»≠") {
            DataPersistenceManager.instance.DeleteGame();
        } else {
            inputField.text = "";
            WarningTab.SetActive(true);
        }
    }

    public void SignOut()
    {
        AuthenticationService.Instance.SignOut();
        PlayerPrefs.DeleteKey("GuestLogIn");
        PlayerPrefs.DeleteKey("GoogleLogIn");
        StartScreenScript.Instance.isSignedIn = false;
        StartScreen.SetActive(true);
    }

    public void changeSFXVolume(Slider slider)
    {
        float volume = slider.value;
        audioController.SetFloat("SFX", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("SFXVolume", volume);

        for(int i = 0; i < SFXSliders.Length; i++) {
            SFXSliders[i].value = volume;
        }
    }

    public void changeMusicVolume(Slider slider)
    {
        float volume = slider.value;
        audioController.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);

        for(int i = 0; i < musicSliders.Length; i++) {
            musicSliders[i].value = volume;
        }
    }

    public void LoadSFXVolume()
    {
        //SFX
        for(int i = 0; i < SFXSliders.Length; i++) {
            SFXSliders[i].value = PlayerPrefs.GetFloat("SFXVolume");
        }

        changeSFXVolume(SFXSliders[0]);
    }

    public void LoadMusicVolume()
    {       
        //Music
        for(int i = 0; i < musicSliders.Length; i++) {
            musicSliders[i].value = PlayerPrefs.GetFloat("MusicVolume");
        }
        changeMusicVolume(musicSliders[0]);
    }
}
