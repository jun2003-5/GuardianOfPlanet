using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInCircle : MonoBehaviour
{
    public Transform mainPosition;

    [SerializeField]
    public float rotationRadius, angularSpeed;

    float posX, posY, angle = 0f;

    private void Start()
    {
        mainPosition = GameObject.FindGameObjectWithTag("MainWeapon").transform;
    }
    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.rotation = Quaternion.Euler(0,0, GameObject.FindGameObjectWithTag("MainWeapon").transform.rotation.z);

        posX = mainPosition.position.x + Mathf.Cos(angle) * rotationRadius * (Orbital.instance.extraRange ? 1.5f : 1);
        posY = mainPosition.position.y + Mathf.Sin(angle) * rotationRadius * (Orbital.instance.extraRange ? 1.5f : 1);
        transform.position = new Vector2(posX, posY);
        angle = angle + Time.deltaTime * angularSpeed * (Orbital.instance.extraSpeed ? 1.5f : 1);

        if (angle >= 360f)
            angle = 0f;
    }
}
