using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLine : MonoBehaviour
{
    public Transform laserFirePoint;
    public Transform laserEndPoint;
    public LineRenderer m_lineRenderer;

    public void ShootLaser()
    {
        Draw2DRay(laserFirePoint.position, laserEndPoint.position);
    }

    void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        m_lineRenderer.SetPosition(0, startPos);
        m_lineRenderer.SetPosition(1, endPos);
    }
}
