using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ServerManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private string[] avatar;
    private GameObject user;
    private int model;

    public string nickname;
    public bool isConnect;
    public GameObject MainCamera;

    string gameVersion = "2";

    private void Awake()
    {
        model = 10;
        isConnect = false;
        avatar = new string[] { "hanyang_professor", "hanyang_boy", "hanyang_girl" };
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("���� ���� �Ϸ�");
        isConnect = true;

        if(model != 10)
        {
            JoinClass();
        }
    }

    public void JoinLobby(string nickname, int model)
    {
        PhotonNetwork.JoinOrCreateRoom("�κ�", new RoomOptions { MaxPlayers = 10 }, null);
        PhotonNetwork.NickName = nickname;
        this.nickname = nickname;

        this.model = model;
    }

    public void outRoom()
    {
        PhotonNetwork.LeaveRoom();
        MainCamera.GetComponent<CameraManager>().canFollow = false;
    }

    public void JoinClass()
    {
        PhotonNetwork.JoinOrCreateRoom("���ǽ�", new RoomOptions { MaxPlayers = 5 }, null);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.Name == "�κ�")
        {
            user = PhotonNetwork.Instantiate(avatar[model], new Vector3(-45f, 2f, 7.5f), Quaternion.identity, 0);
            Debug.Log(PhotonNetwork.NickName + " ���� ĳ���� ���� �Ϸ�");
            Debug.Log(PhotonNetwork.NickName + " ���� �κ� �����߽��ϴ�.");
        }

        else if (PhotonNetwork.CurrentRoom.Name == "���ǽ�")
        {
            user = PhotonNetwork.Instantiate(avatar[model], new Vector3(-13f, 2f, -7f), Quaternion.identity, 0);
            Debug.Log(PhotonNetwork.NickName + " ���� ĳ���� ���� �Ϸ�");
            Debug.Log(PhotonNetwork.NickName + " ���� ���ǽǿ� �����߽��ϴ�.");
        }

        MainCamera.GetComponent<CameraManager>().canFollow = true;
        user.GetComponent<AvatarController>().nickname = this.nickname;
        user.GetComponent<AvatarController>().ActiveName();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("�� ����� ����");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("�� ���� ����");
        Debug.Log(message);
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("���� ���� ����");
        isConnect = false;
    }
}
