using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour
{
    public Rigidbody avatarRigidbody;

    public float speed = 10f;
    public float jumpPower = 5f;

    bool isJump;

    // Start is called before the first frame update
    void Start()
    {
        avatarRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MouseTrace();
        Move();
        Jump();
    }

    void MouseTrace()
    {
        transform.rotation = Camera.main.transform.rotation;
    }

    void Move()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        transform.Translate((new Vector3(inputX, 0, inputZ) * speed) * Time.deltaTime);
    }

    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(!isJump)
            {
                isJump = true;
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
}
