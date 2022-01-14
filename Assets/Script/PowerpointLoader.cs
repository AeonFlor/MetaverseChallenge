using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PowerpointLoader : MonoBehaviour
{
    public MeshRenderer Screen;

    string FolderPath = "C:/Users/AeonFlor/Desktop/project/metaverse_challenge/metaverse_challenge/Assets/PPT/";
    string CurrentPPT;
    int page = 1;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            ChoosePPT("220106_회의");
        }

        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveNext();
        }

        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MovePrevious();
        }
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
    void ChoosePPT(string PPTname)
    {
        CurrentPPT = FolderPath + PPTname + "/슬라이드";
        Screen.material.SetTexture("_MainTex", LoadPNG(CurrentPPT + page.ToString() + ".PNG"));
        Debug.Log(CurrentPPT + page.ToString() + ".PNG");
    }
    void MoveNext()
    {
        if(File.Exists(CurrentPPT + (page+1).ToString() + ".PNG"))
        {
            Screen.material.SetTexture("_MainTex", LoadPNG(CurrentPPT + (++page).ToString() + ".PNG"));
        }

        else
        {
            //마지막 페이지라는 안내 문구
        }
    }

    void MovePrevious()
    {
        if (page != 1)
        {
            Screen.material.SetTexture("_MainTex", LoadPNG(CurrentPPT + (--page).ToString() + ".PNG"));
        }

        else
        {
            //첫 페이지라는 안내 문구
        }
    }
}
