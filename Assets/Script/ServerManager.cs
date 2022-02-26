using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ServerManager : MonoBehaviourPunCallbacks
{
    public string nickname, player;


    public void openLobby(string player, string nickname)
    {
        Connect();

        this.player = player;
        this.nickname = nickname;
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(nickname + "���� " + player + " �ƹ�Ÿ�� �κ� ������ ����Ǿ����ϴ�.");

        PhotonNetwork.AutomaticallySyncScene = true;

        var Player = PhotonNetwork.Instantiate(player, new Vector3(0, 10, 0), Quaternion.identity);
        Debug.Log(Player);
        PhotonNetwork.LocalPlayer.NickName = nickname;

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log(PhotonNetwork.NickName + "���� �κ� ����Ǿ����ϴ�.");
    }

    public void JoinClass()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinOrCreateRoom("���ǽ�", new RoomOptions { MaxPlayers = 5 }, null);
        }

        else
        {
            Connect();
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("���ǽ��� �����Ǿ����ϴ�.");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.NickName + "���� ���ǽǿ� �����߽��ϴ�.");
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("���� ���� ���� - ������ �õ� �� ...");
        Connect();
    }
}
