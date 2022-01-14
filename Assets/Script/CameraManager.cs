using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Rigidbody Player;
    public float Sensitivity = 1500f;
    float rotateX = 0f;
    float rotateY = 0f;

    void Update()
    {
        TracePlayer();
        CameraControl();
    }

    void TracePlayer()
    {
        transform.position = Player.transform.position + new Vector3(0f, 3f, 0f);
    }

    void CameraControl()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotateX += Sensitivity * mouseX * Time.deltaTime;
        rotateY += Sensitivity * mouseY * Time.deltaTime;

        rotateX = Mathf.Clamp(rotateX, -80, 80);
        rotateY = Mathf.Clamp(rotateY, -5, 30);

        // X, Y 는 회전축을 나타내기에 위치가 바뀌었다.
        transform.eulerAngles = new Vector3(-rotateY, rotateX, 0f);
    }
}
