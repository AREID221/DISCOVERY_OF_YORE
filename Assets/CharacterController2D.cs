using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    private float m_worldFriction = 0.05f;
    public Rigidbody2D s_spriteRB;
    private Vector3 m_velocity = Vector3.zero;
    public LayerMask m_groundLayers;

    private float s_jumpForce = 700f;
    public bool m_midAirControl = false;
    //Grab ceiling and ground check transforms through hard code.
    public Transform s_groundCheck;
    const float c_groundDetectionRadius = 0.2f;
    public bool m_isGrounded;

    public Transform s_ceilingCheck;
    const float c_ceilingDetectionRadius = 0.2f;

    public UnityEvent OnGround;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        s_spriteRB = GetComponent<Rigidbody2D>();

        if (OnGround == null)
        {
            OnGround = new UnityEvent();
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        bool wasGrounded = m_isGrounded;
        m_isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(s_groundCheck.position, c_ceilingDetectionRadius, m_groundLayers);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_isGrounded = true;

                if (!wasGrounded)
                    OnGround.Invoke();

                //if (!wasGrounded)
                //{
                //    OnGround.Invoke();
                //}
            }
        }
    }

    public void MoveSprite(float move, bool isJumping)
    {
        if (m_isGrounded || m_midAirControl)
        {
            Vector3 desiredVelocity = new Vector2(move * 10f, s_spriteRB.velocity.y);
            s_spriteRB.velocity = Vector3.SmoothDamp(s_spriteRB.velocity, desiredVelocity, ref m_velocity, m_worldFriction);

            // Flip the player depending on current axis direction (left-facing for A, right-facing for D).
            // TO-DO: Flip function!

            
        }

        if (m_isGrounded && isJumping)
        {
            m_isGrounded = false;
            s_spriteRB.AddForce(new Vector2(0f, s_jumpForce));
        }
    }
}
