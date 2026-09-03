using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootLazer : MonoBehaviour
{
    public Lazer lazer;

    public Transform laserFirePoint;
    public Transform laserEndPoint;
    public SpriteRenderer lazerRenderer;

    public bool isShooting;

    private void Update()
    {
        ShootLaser();
    }

    void ShootLaser()
    {
        lazerRenderer.enabled = isShooting;
    }
}
