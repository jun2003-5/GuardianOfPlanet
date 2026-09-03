using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    public GameObject scrollbar;
    float scroll_pos = 0;
    float[] pos;

    public GameObject Image_Change;

    public GameObject contents;
    public GameObject BoughtALL;

    public Button nextButton;
    public Button PreviousButton;

    void Update()
    {
        if(transform.childCount > 1) {
            pos = new float[transform.childCount];
            float distance = 1f / (pos.Length - 1f);
            for(int i = 0; i < pos.Length; i++) {
                pos[i] = distance * i;
            }
            if(Input.GetMouseButton(0))
                scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
            else {
                for(int i = 0; i < pos.Length; i++) {
                    if(scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2)) {
                        scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);

                        if(Image_Change != null) {
                            for(int a = 0; a < Image_Change.transform.childCount; a++) {
                                Image_Change.transform.GetChild(a).gameObject.SetActive(i == a);
                            }
                        }
                    }
                }
            }

            for(int i = 0; i < pos.Length; i++) {
                if(scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2)) {
                    transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1, 1), 0.1f);
                    for(int a = 0; a < pos.Length; a++) {
                        if(a != i) {
                            transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                        }
                    }
                }
            }

            //UI
            if(nextButton != null) {
                if(scrollbar.GetComponent<Scrollbar>().value >= 0.98f) {
                    nextButton.gameObject.SetActive(false);
                } else {
                    nextButton.gameObject.SetActive(true);
                }
            }

            if(PreviousButton != null) {
                if(scrollbar.GetComponent<Scrollbar>().value <= 0.02f) {
                    PreviousButton.gameObject.SetActive(false);
                } else {
                    PreviousButton.gameObject.SetActive(true);
                }
            }


        } else if(transform.childCount == 1){
            transform.GetChild(0).localScale = new Vector2(1, 1);
            nextButton.gameObject.SetActive(false);
            PreviousButton.gameObject.SetActive(false);
        } else {
            BoughtALL.SetActive(true);
        }
    }

    public void nextPackage()
    {
        // Calculate the index of the next content
        int nextIndex = Mathf.Clamp(Mathf.FloorToInt(scrollbar.GetComponent<Scrollbar>().value * (pos.Length - 1)) + 1, 0, pos.Length - 1);

        // Calculate the target scroll position
        float targetScrollPos = pos[nextIndex];

        // Smoothly scroll to the target position
        StartCoroutine(SmoothScroll(targetScrollPos));
    }

    public void previousPackage()
    {
        // Calculate the index of the previous content
        int prevIndex = Mathf.Clamp(Mathf.FloorToInt(scrollbar.GetComponent<Scrollbar>().value * (pos.Length - 1)) - 1, 0, pos.Length - 1);

        // Calculate the target scroll position
        float targetScrollPos = pos[prevIndex];

        // Smoothly scroll to the target position
        StartCoroutine(SmoothScroll(targetScrollPos));
    }


    IEnumerator SmoothScroll(float target)
    {
        float elapsedTime = 0;
        float duration = 0.35f; // Adjust this value for the duration of scrolling

        float initialScrollPos = scrollbar.GetComponent<Scrollbar>().value;

        nextButton.interactable = false;
        PreviousButton.interactable = false;

        while(elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(initialScrollPos, target, elapsedTime / duration);
            yield return null;
        }

        nextButton.interactable = true;
        PreviousButton.interactable = true;

        scroll_pos = target;
        scrollbar.GetComponent<Scrollbar>().value = target; // Set the scrollbar value to the target position explicitly
    }
}

