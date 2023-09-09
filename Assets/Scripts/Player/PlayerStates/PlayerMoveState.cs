using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMoveState : MonoBehaviour
{
    public static PlayerMoveState Instance { get; private set; }
    private Player player;
    private List<Collider2D> wallColliderList;
    private static Vector2 throwDir;
    private GameInput gameInput;
    private float startingMoveSpeed = 0.06f;
    private float moveSpeed;
    private float acceleration = 1f;
    private float maxMoveSpeed = 0.16f;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance;
        wallColliderList = new List<Collider2D>();
        gameInput = player.gameInput;
    }
    // Update is called once per frame
    void Update()
    {
        if (!player.isDashing)
        {
            HandleMovement();
        }
    }
    public bool IsTouchingWall()
    {
        int a = player.wallCollider.OverlapCollider(new ContactFilter2D(), wallColliderList);
        foreach (Collider2D collider2D in wallColliderList)
        {
            //int b = wallCollider.OverlapCollider(new ContactFilter2D(), wallColliderList);
            if (collider2D.gameObject.GetComponent<GroundColider>() != null)
            {
                return true;
            }
        }
        return false;
    }
    public bool IsRunning()
    {
        if (moveSpeed >= maxMoveSpeed)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsMoving()
    {
        if (player.moveDir.x != 0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = player.gameInput.GetMoveInputNormalized();
        float moveX = 0;
        if (inputVector.x < 0)
        {
            moveX = -1;
        }
        else if (inputVector.x > 0)
        {
            moveX = 1;
        }
        player.moveDir = new Vector3(moveX, 0, 0);
        if (!player.IsTouchingWall())
        {
            if (!player.isJumping && !player.isFalling)
            {
                if (IsMoving() && !player.isAttacking)
                {
                    moveSpeed = moveSpeed * acceleration;
                    player.transform.position = transform.position + player.moveDir * moveSpeed;
                    player.transform.right = new Vector3(player.moveDir.x, 0f, 0f);
                    player.throwDir = new Vector2(player.moveDir.x, player.moveDir.y);
                    if (moveSpeed >= maxMoveSpeed)
                    {
                        moveSpeed = maxMoveSpeed;
                        acceleration = 1f;
                    }
                    else
                    {
                        acceleration = acceleration + 0.02f * Time.deltaTime;
                    }
                }
                else if (!IsMoving() || player.isAttacking)
                {

                    if (moveSpeed > startingMoveSpeed)
                    {
                        moveSpeed = moveSpeed - 0.11f * Time.deltaTime;
                    }
                    else
                    {
                        moveSpeed = startingMoveSpeed;
                    }
                }
            }
            else
            {
                Console.WriteLine("OLD POS: " + player.transform.position);
                player.transform.position = player.transform.position + player.moveDir * moveSpeed;
                Console.WriteLine("NEW POS: " + player.transform.position);
                if (player.moveDir != player.transform.right && player.moveDir.x != 0f)
                {
                    player.transform.right = new Vector3(player.moveDir.x, 0f, 0f);
                    player.throwDir = new Vector2(player.moveDir.x, player.moveDir.y);
                }
                else
                {
                    player.transform.right = new Vector3(player.transform.right.x, 0f, 0f);
                    player.throwDir = new Vector2(player.transform.right.x, player.moveDir.y);
                }
            }
        }
        else
        {
            player.transform.right = new Vector3(player.moveDir.x, 0f, 0f);
            player.throwDir = new Vector2(player.moveDir.x, player.moveDir.y);
            Debug.Log("is touching wall!");
        }
    }
    
}
