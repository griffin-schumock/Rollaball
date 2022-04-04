using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpSpeed;
    public int defaultJumpAllowed;
    int jumpAllowed;

    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    private Rigidbody rb;
    bool isGrounded;
    private int count;
    private float movementX;
    private float movementY;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpAllowed = defaultJumpAllowed;
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnJump()
    {
        if(isGrounded)
        {
            Jump();
            jumpAllowed--;
            isGrounded = false;
        }

        else if(!isGrounded && (jumpAllowed > 0))
        {
            Jump();
            jumpAllowed--;
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if(count >= 12)
        {
            winTextObject.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);
    }

    void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            jumpAllowed = defaultJumpAllowed-1;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    private void Jump()
    {
        Vector3 jumpHeight = new Vector3(0.0f, 20.0f * jumpSpeed, 0.0f);
        rb.AddForce(jumpHeight);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp")) { 
        other.gameObject.SetActive(false);
            count += 1;

            SetCountText();
        }
    }
}
