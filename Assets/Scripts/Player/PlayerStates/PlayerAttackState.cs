using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttackState : MonoBehaviour
{
    public static PlayerAttackState Instance { get; private set; }
    private Player player;
    private float runningAttackCD = 0f;

    public event EventHandler OnPlayerAttack;
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance;
        player.gameInput.OnPlayerAttack += GameInput_OnPlayerAttack;
    }

    // Update is called once per frame
    void Update()
    {
        CheckCds();
    }

    private void GameInput_OnPlayerAttack(object sender, EventArgs e)
    {
        OnPlayerAttack?.Invoke(this, EventArgs.Empty);
        if (PlayerMoveState.Instance.IsRunning()&& player.isGrounded && runningAttackCD <= 0)
        {
            player.playerRigidbody.AddForce(new Vector2(-200f * player.moveDir.x, 0f));
        }
    }

    public void EnableSlashTrailRenderer()
    {
        player.slashPointTrailRenderer.enabled = true;
    }
    public void DisableSlashTrailRenderer()
    {
        player.slashPointTrailRenderer.enabled = false;
    }
    public float GetRunningAttackCD()
    {
        return runningAttackCD;
    }
    public void SetRunningAttackCD(float runningAttackCD)
    {
        this.runningAttackCD = runningAttackCD;
    }

    private void CheckCds()
    {
        if (runningAttackCD > 0f)
        {
            runningAttackCD = runningAttackCD - 1 * Time.deltaTime;
        }
    }
}
