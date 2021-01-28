using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // 2D character controller,
    // contains move and jump.
    public CharacterController2D controller;

    // Left/right movement, jumping velocity.
    private float m_xMovement = 0f;
   //private float m_yMovement = 0f;
    public bool jumping = false;
    // Left/right move speed.
    private float s_moveSpeed = 100f;
    
    private void Awake()
    {

    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        m_xMovement = Input.GetAxisRaw("Horizontal") * s_moveSpeed;

        if (Input.GetButton("Jump"))
        {
            jumping = true;
        }
        //else if (Input.GetButtonUp("Jump"))
        //{

        //}
    }

    private void FixedUpdate()
    {
        controller.MoveSprite(m_xMovement * Time.fixedDeltaTime, jumping);
        jumping = false;
    }
}
