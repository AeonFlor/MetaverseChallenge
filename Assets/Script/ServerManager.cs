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
        Debug.Log("서버 연결 완료");
        isConnect = true;

        if(model != 10)
        {
            JoinClass();
        }
    }

    public void JoinLobby(string nickname, int model)
    {
        PhotonNetwork.JoinOrCreateRoom("로비", new RoomOptions { MaxPlayers = 10 }, null);
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
        PhotonNetwork.JoinOrCreateRoom("강의실", new RoomOptions { MaxPlayers = 5 }, null);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.Name == "로비")
        {
            user = PhotonNetwork.Instantiate(avatar[model], new Vector3(-45f, 2f, 7.5f), Quaternion.identity, 0);
            Debug.Log(PhotonNetwork.NickName + " 님의 캐릭터 생성 완료");
            Debug.Log(PhotonNetwork.NickName + " 님이 로비에 참가했습니다.");
        }

        else if (PhotonNetwork.CurrentRoom.Name == "강의실")
        {
            user = PhotonNetwork.Instantiate(avatar[model], new Vector3(-13f, 2f, -7f), Quaternion.identity, 0);
            Debug.Log(PhotonNetwork.NickName + " 님의 캐릭터 생성 완료");
            Debug.Log(PhotonNetwork.NickName + " 님이 강의실에 참가했습니다.");
        }

        MainCamera.GetComponent<CameraManager>().canFollow = true;
        user.GetComponent<AvatarController>().nickname = this.nickname;
        user.GetComponent<AvatarController>().ActiveName();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("방 만들기 실패");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("방 참가 실패");
        Debug.Log(message);
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("서버 연결 끊김");
        isConnect = false;
    }
}
