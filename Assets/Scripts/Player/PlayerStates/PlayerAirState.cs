using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : MonoBehaviour
{
    private Player player;
    private float lasty;
    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance;
        lasty = player.transform.position.y;
        player.isJumping = false;
        player.isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isGrounded)
        {
            player.isJumping = false;
            player.isFalling = false;
        }
        else
        {
            if (lasty > transform.position.y)
            {
                player.isFalling = true;
            }
        }
        lasty = transform.position.y;
    }
}
