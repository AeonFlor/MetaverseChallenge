using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_Manager_Class : MonoBehaviour
{
    public Canvas UI;
    public GameObject[] avatar = new GameObject[3];

    public Button BtnCheckPPT, BtnManageStudent, BtnExit01, BtnExit02, BtnBack, BtnPrevious, BtnNext, BtnSelect, BtnExit03, BtnConnect;
    public TMP_InputField InputNickname;

    private GameObject DeskUI, PPTUI, SelectUI, ConnectUI;

    CameraManager player;

    public Camera MainCamera;

    GameObject userAvatar;

    GameObject ServerManager;

    // Start is called before the first frame update
    void Start()
    {
        player = MainCamera.GetComponent<CameraManager>();

        BtnPrevious.onClick.AddListener(previous);
        BtnNext.onClick.AddListener(next);
        BtnSelect.onClick.AddListener(select);
        BtnCheckPPT.onClick.AddListener(checkPPT);
        BtnManageStudent.onClick.AddListener(manageStudent);
        BtnExit01.onClick.AddListener(exit);
        BtnExit02.onClick.AddListener(exit);
        BtnExit03.onClick.AddListener(exit);
        BtnBack.onClick.AddListener(back);
        BtnConnect.onClick.AddListener(connect);

        DeskUI = GameObject.Find("Desk_UI");
        PPTUI = GameObject.Find("PPT_UI");
        SelectUI = GameObject.Find("Select_UI");
        ConnectUI = GameObject.Find("Connect_UI");

        DeskUI.SetActive(false);
        PPTUI.SetActive(false);
        ConnectUI.SetActive(false);
        SelectUI.SetActive(true);
    }

    // Update is called once per frame
    void manageStudent()
    {
        
    }

    void checkPPT()
    {
        DeskUI.SetActive(false);
        PPTUI.SetActive(true);
        PPTUI.GetComponent<PowerpointLoader>().ShowList();
    }

    void back()
    {
        DeskUI.SetActive(true);
        PPTUI.SetActive(false);
    }

    public void exit()
    {
        DeskUI.SetActive(false);
        PPTUI.SetActive(false);
        ConnectUI.SetActive(false);
        player.outUI();
    }

    public void enableDeskUI()
    {
        player.inUI();
        DeskUI.SetActive(true);
    }

    public void enableConnectUI()
    {
        player.inUI();
        ConnectUI.SetActive(true);
    }

    public bool isActiveUI()
    {
        return (PPTUI.activeSelf || DeskUI.activeSelf);
    }

    void previous()
    {
        if (MainCamera.transform.position.x > 0)
            MainCamera.transform.position = MainCamera.transform.position + new Vector3(-10, 0, 0);

        else
            Debug.Log("첫 번째 아바타입니다.");
    }

    void next()
    {
        if (MainCamera.transform.position.x < 20)
            MainCamera.transform.position = MainCamera.transform.position + new Vector3(10, 0, 0);

        else
            Debug.Log("마지막 아바타입니다.");
    }

    void select()
    {
        userAvatar = Instantiate(avatar[(int)MainCamera.transform.position.x / 10]);
        userAvatar.transform.position = new Vector3(0, 10, 0);

        userAvatar.GetComponent<AvatarController>().nickname = InputNickname.text;
        userAvatar.GetComponent<AvatarController>().ActiveName();

        DontDestroyOnLoad(userAvatar);
        DontDestroyOnLoad(UI);
        DontDestroyOnLoad(MainCamera);

        SelectUI.SetActive(false);
        SceneManager.LoadScene("Lobby");

        MainCamera.GetComponent<CameraManager>().inSelect();
    }

    void connect()
    {
        ServerManager = GameObject.Find("ServerManager");
        ServerManager.GetComponent<ServerManager>().JoinClass();

        exit();
        ConnectUI.SetActive(false);

        userAvatar.transform.position = new Vector3(0, 10, 0);

        DontDestroyOnLoad(userAvatar);
        DontDestroyOnLoad(UI);
        DontDestroyOnLoad(MainCamera);
        DontDestroyOnLoad(ServerManager);

        SceneManager.LoadScene("Class");
    }
}
