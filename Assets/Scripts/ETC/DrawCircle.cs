using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCircle : MonoBehaviour
{
    private LineRenderer linerenderer;

    public float radius;
    // Start is called before the first frame update
    void Start()
    {
        linerenderer = GetComponent<LineRenderer>();
        DrawCircles(100, radius);
    }

    private void Update()
    {
        DrawCircles(100, radius);
    }
    void DrawCircles(int steps, float radius)
    {
        linerenderer.positionCount = steps;

        for (int currentStep = 0; currentStep < steps; currentStep++)
        {
            float circumferenceProgress = (float)currentStep / (steps - 1);

            float currentRadian = circumferenceProgress * 2 * Mathf.PI;

            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;

            Vector3 currentPosition = new Vector3(x, y, 0);

            linerenderer.SetPosition(currentStep, currentPosition);
        }
    }
}
