using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Rigidbody Player;
    public float Sensitivity = 1500f;
    float rotateX = 0f;
    float rotateY = 0f;

    public bool isShow, isSelect;

    GameObject[] players;

    void Start()
    {
        isShow = isSelect = false;
    }

    void Update()
    {
        if (isSelect && Player == null)
        {
            players = GameObject.FindGameObjectsWithTag("Player");

            foreach(var player in players)
            {
                if (player.GetComponent<AvatarController>().checkLocal())
                {
                    Player = player.GetComponent<Rigidbody>();
                }
            }
        }

        if (isSelect && !isShow)
        {
            TracePlayer();
            CameraControl();
        }
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

        //rotateX = Mathf.Clamp(rotateX, -180, 180);
        rotateY = Mathf.Clamp(rotateY, -30, 30);

        // X, Y 는 회전축을 나타내기에 위치가 바뀌었다.
        transform.eulerAngles = new Vector3(-rotateY, rotateX, 0f);
    }
    public void inUI()
    {
        isShow = true;
    }

    public void outUI()
    {
        isShow = false;
    }

    public void inSelect()
    {
        isSelect = true;
    }
}
