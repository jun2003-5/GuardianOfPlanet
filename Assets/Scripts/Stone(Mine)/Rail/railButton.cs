using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class railButton : MonoBehaviour
{
    public Sprite[] twoFrame;
    public Image frame;
    public Image railButton_oreImage;
    public int index;

    public void setGUI(Sprite oreImage, int i)   
    {
        railButton_oreImage.sprite = oreImage;
        index = i;
    }

    public void railButtonClicked() {
        if(frame.sprite == twoFrame[0])
            frame.sprite = twoFrame[1];
        else
            frame.sprite = twoFrame[0];
    }
}
