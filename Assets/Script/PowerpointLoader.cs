using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class PowerpointLoader : MonoBehaviour
{
    public GameObject LoadPPT;

    public GameObject screen;
    MeshRenderer ScreenMesh;

    string FolderPath = "Assets/PPT/";
    string CurrentPPT;

    List<string> ListOfPPT = new List<string>();

    int page = 1;

    void Start()
    {
        screen = GameObject.Find("Screen");
        ScreenMesh = screen.GetComponent<MeshRenderer>();
    }

    Texture2D LoadPNG(string Path)
    {
        Texture2D tex = null;
        byte[] FileData;
        
        if(File.Exists(Path))
        {
            FileData = File.ReadAllBytes(Path);
            tex = new Texture2D(2,2);
            tex.LoadImage(FileData);
        }

        return tex;
    }

    public void PPTLoader(string PPTname)
    {
        CurrentPPT = FolderPath + PPTname + "/슬라이드";
        ScreenMesh.material.SetTexture("_MainTex", LoadPNG(CurrentPPT + page.ToString() + ".PNG"));
        Debug.Log(CurrentPPT + page.ToString() + ".PNG");
    }

    public void MoveNext()
    {
        if(File.Exists(CurrentPPT + (page+1).ToString() + ".PNG"))
        {
            ScreenMesh.material.SetTexture("_MainTex", LoadPNG(CurrentPPT + (++page).ToString() + ".PNG"));
        }

        else
        {
            Debug.Log("마지막 페이지입니다.");
        }
    }

    public void MovePrevious()
    {
        if (page != 1)
        {
            ScreenMesh.material.SetTexture("_MainTex", LoadPNG(CurrentPPT + (--page).ToString() + ".PNG"));
        }

        else
        {
            Debug.Log("첫번째 페이지입니다.");
        }
    }

    public void ShowList()
    {
        DirectoryInfo list = new DirectoryInfo(FolderPath);

        foreach(var ppt in list.GetDirectories())
        {
            if (!ListOfPPT.Contains(ppt.Name))
                ListOfPPT.Add(ppt.Name);
        }

        int yValue = 0;

        foreach (string ppt in ListOfPPT)
        {
            GameObject index = Instantiate(LoadPPT, GameObject.Find("Content").transform);
            index.transform.position = index.transform.position + new Vector3(0, yValue, 0);
            index.GetComponentInChildren<TMP_Text>().text = ppt;
            yValue -= 200;
        }
    }
}
