using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    private float m_worldFriction = 0.05f;
    public Rigidbody2D s_spriteRB;
    public Vector3 m_velocity = Vector3.zero;

    private void Awake()
    {
        s_spriteRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        
    }

    public void MoveSprite(float move, bool isJumping)
    {
        Vector3 desiredVelocity = new Vector2(move * 10f, s_spriteRB.velocity.y);
        s_spriteRB.velocity = Vector3.SmoothDamp(s_spriteRB.velocity, desiredVelocity, ref m_velocity, m_worldFriction);
    }
}
