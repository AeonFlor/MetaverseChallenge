using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PowerpointLoader : MonoBehaviourPun
{
    public PhotonView PV;
    public GameObject LoadPPT;

    public GameObject screen;

    Texture2D current;

    MeshRenderer ScreenMesh;

    string FolderPath = "C:/PPT/";
    string CurrentPPT;

    List<string> ListOfPPT = new List<string>();
    string[] pptEXTs = new string[4];
    string currentEXT;

    int page = 1;

    void Start()
    {
        screen = GameObject.Find("Screen");
        ScreenMesh = screen.GetComponent<MeshRenderer>();
        pptEXTs = new string[]{ ".png", ".PNG", ".jpg", ".JPG"};
        PV = PhotonView.Get(this);
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

    public byte[] PPTLoader(string PPTname)
    {
        CurrentPPT = FolderPath + PPTname;
        page = 1;
        
        foreach(string ext in pptEXTs)
        {
            if (new FileInfo(CurrentPPT + "/슬라이드" + page.ToString() + ext).Exists)
            {
                CurrentPPT = CurrentPPT + "/슬라이드";
                current = LoadPNG(CurrentPPT + page.ToString() + ext);
                currentEXT = ext;
                break;
            }

            else if(new FileInfo(CurrentPPT + "/Slide" + page.ToString() + ext).Exists)
            {
                CurrentPPT = CurrentPPT + "/Slide";
                current = LoadPNG(CurrentPPT + page.ToString() + ext);
                currentEXT = ext;
                break;
            }
        }

        ScreenMesh.material.SetTexture("_MainTex", current);

        if (currentEXT == ".JPG" || currentEXT == ".jpg")
            return current.EncodeToJPG();

        else
            return current.EncodeToPNG();
    }

    public byte[] MoveNext()
    {
        if(File.Exists(CurrentPPT + (page+1).ToString() + currentEXT))
        {
            current = LoadPNG(CurrentPPT + (++page).ToString() + currentEXT);
            ScreenMesh.material.SetTexture("_MainTex", current);
        }

        else
        {
            Debug.Log("마지막 페이지입니다.");
        }

        if (currentEXT == ".JPG" || currentEXT == ".jpg")
            return current.EncodeToJPG();

        else
            return current.EncodeToPNG();
    }

    public byte[] MovePrevious()
    {
        if (page != 1)
        {
            current = LoadPNG(CurrentPPT + (--page).ToString() + currentEXT);
            ScreenMesh.material.SetTexture("_MainTex", current);
        }

        else
        {
            Debug.Log("첫번째 페이지입니다.");
        }

        if (currentEXT == ".JPG" || currentEXT == ".jpg")
            return current.EncodeToJPG();

        else
            return current.EncodeToPNG();
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
