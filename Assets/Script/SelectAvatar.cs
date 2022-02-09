using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectAvatar : MonoBehaviour
{
    public GameObject[] avatar = new GameObject[3];
    public Button BtnPrevious, BtnNext, BtnSelect;
    public Camera MainCamera;

    void Start()
    {
        BtnPrevious.onClick.AddListener(previous);
        BtnNext.onClick.AddListener(next);
        BtnSelect.onClick.AddListener(select);
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
        GameObject userAvatar = Instantiate(avatar[(int)MainCamera.transform.position.x / 10]);
        userAvatar.transform.position = userAvatar.transform.position + new Vector3(0, 10, 0);
        DontDestroyOnLoad(userAvatar);
        SceneManager.LoadScene("Lobby");
    }
}
