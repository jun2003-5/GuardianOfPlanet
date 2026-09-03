using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class strShootLazer : MonoBehaviour
{
    public strLazer lazer;

    public Transform laserFirePoint;
    public Transform laserEndPoint;
    public LineRenderer m_lineRenderer;

    public bool isShooting;

    private void Update()
    {
        ShootLaser();
    }

    void ShootLaser()
    {
        m_lineRenderer.enabled = isShooting;
        if(isShooting)
            Draw2DRay(laserFirePoint.position, laserEndPoint.position);
    }

    void Draw2DRay(Vector3 startPos, Vector3 endPos)
    {
        m_lineRenderer.SetPosition(0, startPos);
        m_lineRenderer.SetPosition(1, endPos);
    }
}
