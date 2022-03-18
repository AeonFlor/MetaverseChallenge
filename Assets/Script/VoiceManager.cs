using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice;
using Photon.Voice.Unity;
using Photon.Voice.PUN;

public class VoiceManager : MonoBehaviour
{
    Recorder recorder;
    PhotonVoiceView voiceView;

    // Start is called before the first frame update
    void Start()
    {
        recorder = GetComponent<Recorder>();
        voiceView = GetComponent<PhotonVoiceView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
