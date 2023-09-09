using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAbilityState : MonoBehaviour
{
    public static PlayerAbilityState Instance { get; private set; }
    private Player player;
    private GameInput gameInput;

    private float dashCd = 0f;
    private float maxDashCd = 1.5f;
    private float dashTrailTimer = 0f;
    private float maxDashTrailTimer = 0.3f;
    private float jumpHeight = 500;

    public int throwingObjectsCount = 5;

    public event EventHandler OnPlayerDash;
    public event EventHandler OnPlayerThrowAttack;
    public event EventHandler OnPlayerInteract;
    public event EventHandler OnPlayerJump;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance;
        gameInput = player.gameInput;
        gameInput.OnPlayerDash += GameInput_OnPlayerDash;
        gameInput.OnPlayerInteract += GameInput_OnPlayerInteract;
        gameInput.OnPlayerRangedAttack += GameInput_OnPlayerRangedAttack;
        gameInput.OnPlayerJump += GameInput_OnPlayerJump;
    }

    // Update is called once per frame
    void Update()
    {
        CheckCds();
    }

    private void GameInput_OnPlayerDash(object sender, EventArgs e)
    {
        if (dashCd <= 0)
        {
            Vector2 dashDir = gameInput.GetMoveInputNormalized();
            if (dashDir.y > 0)
            {
                dashDir = new Vector2(dashDir.x, dashDir.y);
            }
            Debug.Log("dashDir= " + dashDir);
            player.playerRigidbody.velocity = Vector3.zero;
            player.playerRigidbody.angularVelocity = 0f;
            player.playerRigidbody.gravityScale = 0f;
            player.playerRigidbody.AddForce(dashDir * 700f);
            OnPlayerDash?.Invoke(this, EventArgs.Empty);
            player.dashTrailTransform.gameObject.SetActive(true);
            dashTrailTimer = maxDashTrailTimer;
            dashCd = maxDashCd;
            player.isDashing = true;
        }
    }

    private void GameInput_OnPlayerRangedAttack(object sender, EventArgs e)
    {
        if (!player.isAttacking)
        {
            if (throwingObjectsCount > 0)
            {
                throwingObjectsCount--;
                OnPlayerThrowAttack?.Invoke(this, e);
                Instantiate(player.rangedAttackObject, player.playerThrowPoint.transform.position, transform.rotation);
                GameUIManager.Instance.GetComponent<GameUIManager>().ChangeThrowingObjectCount(throwingObjectsCount);
            }
        }
    }

    private void GameInput_OnPlayerInteract(object sender, EventArgs e)
    {
        OnPlayerInteract?.Invoke(this, EventArgs.Empty);
    }
    private void GameInput_OnPlayerJump(object sender, EventArgs e)
    {
        if (!player.isJumping && player.isGrounded && !player.isFalling && !player.isDashing)
        {
            player.isJumping = true;
            player.playerRigidbody.AddForce(new Vector2(1, jumpHeight));
            OnPlayerJump?.Invoke(this, EventArgs.Empty);
            //transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y + jumpHeight, transform.position.z), jumpSpeed*Time.deltaTime ); 
        }
    }
    private void CheckCds()
    {
        if (dashCd > 0f)
        {
            dashCd = dashCd - 1 * Time.deltaTime;
        }
        if (dashTrailTimer > 0f)
        {

            dashTrailTimer = dashTrailTimer - 1 * Time.deltaTime;
        }
        if (dashTrailTimer <= 0f)
        {
            if (player.isDashing)
            {
                player.playerRigidbody.velocity = Vector2.zero;
                player.playerRigidbody.angularVelocity = 0f;
                player.playerRigidbody.gravityScale = 2.2f;
            }
            player.isDashing = false;
            player.dashTrailTransform.gameObject.SetActive(false);
        }
    }
}
