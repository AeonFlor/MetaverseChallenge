using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class AvatarController : MonoBehaviourPun
{
    PhotonView pv;
    public Animator anim;

    GameObject UI;
    GraphicRaycaster rayUI;
    EventSystem eventUI;
    PointerEventData pointer;

    public Rigidbody avatarRigidbody;
    private GameObject mouseTarget;

    public string nickname;
    public float speed = 10f;
    public float jumpPower = 5f;

    bool isJump, isSit, authPPT, onName, isLocal;

    GameObject PowerpointLoader;
    PowerpointLoader controlPPT;

    GameObject UserCanvas;
    Transform nicknameUI;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();

        UI = GameObject.Find("Canvas");

        avatarRigidbody = GetComponent<Rigidbody>();
        rayUI = UI.GetComponent<GraphicRaycaster>();
        eventUI = GetComponent<EventSystem>();

        PowerpointLoader = UI.transform.Find("PPT_UI").gameObject;

        authPPT = isSit = onName = false;

        controlPPT = PowerpointLoader.GetComponent<PowerpointLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!pv.IsMine)
        {
            Debug.Log("-");
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Camera.main.GetComponent<CameraManager>().isShow)
                mouseTarget = GetClickedUI();
            else
                mouseTarget = GetClickedObject();

            if (mouseTarget != null)
            {
                Debug.Log(mouseTarget.tag);

                if (mouseTarget.tag == "ProfessorDesk")
                {
                    Debug.Log("Connect to Desk");
                    UI.GetComponent<UI_Manager_Class>().enableDeskUI();
                }

                else if (mouseTarget.tag == "PPT_Button")
                {
                    authPPT = true;
                    Debug.Log("Select PPT");
                    controlPPT.PPTLoader(mouseTarget.GetComponentInChildren<TMP_Text>().text);
                    UI.GetComponent<UI_Manager_Class>().exit();
                }

                else if(mouseTarget.tag == "ConnectClass")
                {
                    Debug.Log("Connect to Class");
                    UI.GetComponent<UI_Manager_Class>().enableConnectUI();
                }

                else if(mouseTarget.tag == "Seat")
                {
                    if(!isSit)
                    {
                        transform.position = mouseTarget.transform.position;
                        anim.SetBool("sit", true);
                        isSit = true;
                    }
                }
            }
        }

        if(isSit)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {

            }
        }

        if(authPPT)
        {
            if (Input.GetKeyDown(KeyCode.Z))
                controlPPT.MovePrevious();

            else if (Input.GetKeyDown(KeyCode.X))
                controlPPT.MoveNext();
        }

        if (Camera.main.GetComponent<CameraManager>().isSelect)
        {
            MouseTrace();

            if (onName)
            {
                NameTracePlayer();
            }

            Move();
            Jump();
        }
    }

    void NameTracePlayer()
    {
        nicknameUI.transform.position = gameObject.transform.position + new Vector3(0f, 10f, 0f);
    }

    void MouseTrace()
    {
        transform.rotation = new Quaternion(transform.rotation.x, Camera.main.transform.rotation.y, transform.rotation.z, transform.rotation.w);
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

        foreach(RaycastResult test in results)
        {
            Debug.Log(test.gameObject.tag);
        }

        // 0은 다른 오브젝트로 탐색된다.
        if (results.Count > 1)
            return results[1].gameObject;

        else
            return null;
    }

    public void ActiveName()
    {
        onName = true;

        UserCanvas = GameObject.Find("UserCanvas");

        // 자식만 탐색
        nicknameUI = UserCanvas.transform.Find("nickname");

        Debug.Log(nicknameUI);

        nicknameUI.GetComponent<TMP_Text>().text = nickname;
    }

    public bool checkLocal()
    {
        return isLocal;
    }
}
