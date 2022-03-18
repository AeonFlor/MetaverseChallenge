using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class CameraManager : MonoBehaviour
{
    public Rigidbody userAvatar;
    public float Sensitivity = 1500f;
    float rotateX = 0f;
    float rotateY = 0f;

    public bool isShow, canFollow;

    void Start()
    {
        isShow = canFollow = false;
    }

    void Update()
    {
        if (canFollow && userAvatar == null)
        {
            findAvatar();
        }

        if (userAvatar != null && canFollow && !isShow)
        {
            TracePlayer();
            CameraControl();
        }
    }

    void TracePlayer()
    {
        transform.position = userAvatar.transform.position + new Vector3(0f, 5f, 0f);
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

    public void outview()
    {
        transform.rotation = userAvatar.transform.rotation;
    }

    public void inUI()
    {
        isShow = true;
    }

    public void outUI()
    {
        isShow = false;
        outview();
    }

    public void inSelect()
    {
        canFollow = true;
    }

    public void findAvatar()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                userAvatar = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
                break;
            }
        }
    }
}
