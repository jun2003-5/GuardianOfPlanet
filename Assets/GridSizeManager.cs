using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSizeManager : MonoBehaviour
{
    private void OnEnable()
    {
		ResizeSpriteToScreen();
    }
    public void ResizeSpriteToScreen()
	{

		transform.localScale = new Vector3(1, 1, 1);


		var worldScreenHeight = (Camera.main.orthographicSize + 1) * 2.0;
		var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

		transform.localScale = new Vector3((float)worldScreenWidth, (float)worldScreenHeight , transform.localScale.z);
	}
}
