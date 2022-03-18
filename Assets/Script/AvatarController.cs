using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Voice;
using Photon.Voice.Unity;
using Photon.Voice.PUN;

public class AvatarController : MonoBehaviourPun
{
    public PhotonView PV;

    public Animator anim;

    GameObject UI, Screen;
    GraphicRaycaster rayUI;
    EventSystem eventUI;
    PointerEventData pointer;

    public Rigidbody avatarRigidbody;
    private GameObject mouseTarget;

    public string nickname;
    public float speed = 10f;
    public float jumpPower = 5f;

    Speaker speaker;

    bool isJump, onName;
    
    [SerializeField]
    private bool authPPT;

    GameObject PowerpointLoader;
    PowerpointLoader controlPPT;

    byte[] currentImg;

    GameObject UserCanvas;
    Transform nicknameUI;

    // Start is called before the first frame update
    void Awake()
    {
        UI = GameObject.Find("Canvas");

        PowerpointLoader = UI.transform.Find("PPT_UI").gameObject;

        avatarRigidbody = GetComponent<Rigidbody>();
        rayUI = UI.GetComponent<GraphicRaycaster>();
        eventUI = GetComponent<EventSystem>();

        speaker = GetComponent<Speaker>();

        authPPT = onName = false;

        controlPPT = PowerpointLoader.GetComponent<PowerpointLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }

        // 조작부
        if (Input.GetMouseButtonDown(0))
        {
            if (Camera.main.GetComponent<CameraManager>().isShow)
                mouseTarget = GetClickedUI();
            else
                mouseTarget = GetClickedObject();

            if (mouseTarget != null)
            {
                if (mouseTarget.tag == "ProfessorDesk")
                {
                    Debug.Log("Connect to Desk");
                    UI.GetComponent<UI_Manager_Class>().enableDeskUI();
                }

                else if (mouseTarget.tag == "PPT_Button")
                {
                    authPPT = true;
                    Debug.Log("Select PPT");
                    currentImg = controlPPT.PPTLoader(mouseTarget.GetComponentInChildren<TMP_Text>().text);
                    PV.RPC("changeScreen", RpcTarget.Others, currentImg);
                    UI.GetComponent<UI_Manager_Class>().exit();
                }

                else if (mouseTarget.tag == "ConnectClass")
                {
                    Debug.Log("Connect to Class");
                    UI.GetComponent<UI_Manager_Class>().enableConnectUI();
                }

                else if (mouseTarget.tag == "Seat")
                {
                    if (SceneManager.GetActiveScene().name == "Class")
                    {
                        Screen = GameObject.Find("Screen");

                        // LookAt 쓸거면 위치 바꾸고 회전해야 됨.
                        transform.position = mouseTarget.transform.position;
                        transform.LookAt(Screen.transform);

                        Camera.main.transform.position = mouseTarget.transform.position + new Vector3(0, 5, 0);
                        Camera.main.transform.LookAt(Screen.transform);

                        // 걷는 중 앉을 경우, 애니메이션이 계속 재생되는 문제 해결을 위한 코드
                        anim.SetFloat("Walk", 0);

                        Camera.main.GetComponent<CameraManager>().inUI();
                    }
                }

                else if (mouseTarget.tag == "LightSwitch")
                {
                    if (GameObject.Find("Directional Light").GetComponent<Light>().color == Color.black)
                        GameObject.Find("Directional Light").GetComponent<Light>().color = new Color(255/255f, 244/255f, 214/255f);

                    else
                        GameObject.Find("Directional Light").GetComponent<Light>().color = Color.black;
                }
            }
        }

        if (authPPT)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                currentImg = controlPPT.MovePrevious();
                PV.RPC("changeScreen", RpcTarget.Others, currentImg);
            }

            else if (Input.GetKeyDown(KeyCode.X))
            {
                currentImg = controlPPT.MoveNext();
                PV.RPC("changeScreen", RpcTarget.Others, currentImg);
            }


        }

        if (Camera.main.GetComponent<CameraManager>().canFollow && !Camera.main.GetComponent<CameraManager>().isShow)
        {
            MouseTrace();

            /*
            if (onName)
            {
                NameTracePlayer();
            }
            */

            Move();
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Camera.main.GetComponent<CameraManager>().outUI();
            UI.GetComponent<UI_Manager_Class>().exit();
        }
        
        if (speaker != null)
        {
            if (speaker.IsPlaying)
            {
                Debug.Log("Talking");
                //nicknameUI.GetComponent<TMP_Text>().color = Color.green;
            }

            else
            {
                //nicknameUI.GetComponent<TMP_Text>().color = Color.white;
            }
        }
    }
    void NameTracePlayer()
    {
        nicknameUI.transform.position = gameObject.transform.position + new Vector3(0f, 5.5f, 0f);
        nicknameUI.transform.rotation = gameObject.transform.rotation * Quaternion.AngleAxis(180, Vector3.up);
    }

    void MouseTrace()
    {
        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);
    }

    void Move()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        anim.SetFloat("Walk", Mathf.Abs(inputX) + Mathf.Abs(inputZ));

        transform.Translate((new Vector3(inputX, 0, inputZ) * speed) * Time.deltaTime);
    }

    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(!isJump)
            {
                isJump = true;
                anim.SetTrigger("Jump");
                avatarRigidbody.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isJump = false;
        }
    }

    private GameObject GetClickedObject()
    {
        RaycastHit hit;
        GameObject mouseTarget = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            mouseTarget = hit.collider.gameObject;
        }

        return mouseTarget;
    }

    private GameObject GetClickedUI()
    {
        pointer = new PointerEventData(eventUI);
        pointer.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        rayUI.Raycast(pointer, results);

        // 0은 다른 오브젝트로 탐색된다.
        if (results.Count > 1)
            return results[1].gameObject;

        else
            return null;
    }

    public void ActiveName()
    {
        /*
        onName = true;

        UserCanvas = GameObject.Find("UserCanvas");

        // 자식만 탐색
        nicknameUI = UserCanvas.transform.Find("nickname");

        nicknameUI.GetComponent<TMP_Text>().text = nickname;
        */
    }

    [PunRPC]
    void changeScreen(byte[] img)
    {
        Texture2D tex = new Texture2D(2,2);
        tex.LoadImage(img);

        Screen = GameObject.Find("Screen");
        Screen.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", tex);

        authPPT = false;
    }
}
