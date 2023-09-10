using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerAnimator : MonoBehaviour
{
    private Player player;
    private Animator animator;
    private const string IS_MOVING = "isMoving";
    private const string IS_ATTACKING = "isAttacking";
    private const string ATTACK_TRIGGER = "attackTrigger";
    private const string ATTACK_TRIGGER2 = "attackTrigger2";
    private const string ATTACK_TRIGGER3 = "attackTrigger3";
    private const string ATTACK_TRIGGER4 = "attackTrigger4";
    private const string RUNNINGATTACK_TRIGGER = "runningAttackTrigger";
    private const string JUMP_TRIGGER = "jumpTrigger";
    private const string RUNNING_ATTACK_CD = "runningAttackCD";
    private const string IS_FALLING = "isFalling";
    private const string IS_RUNNING = "isRunning";
    private const string THROW_ATTACK_TRIGGER = "throwAttackTrigger";
    private const string DASH_TRIGGER = "dashTrigger";
    private const string IS_GROUNDED = "isGrounded";

    

    private void Start()
    {
        player = Player.Instance;
        animator = GetComponent<Animator>();
        PlayerAbilityState.Instance.OnPlayerJump += Player_OnPlayerJump;
        PlayerAttackState.Instance.OnPlayerAttack += Player_OnPlayerAttack;
        PlayerAbilityState.Instance.OnPlayerThrowAttack += Player_OnPlayerThrowAttack;
        PlayerAbilityState.Instance.OnPlayerDash += Player_OnPlayerDash;
    }

    private void Player_OnPlayerDash(object sender, System.EventArgs e)
    {
        animator.SetTrigger(DASH_TRIGGER);
    }

    private void Player_OnPlayerThrowAttack(object sender, System.EventArgs e)
    {
        animator.SetTrigger(THROW_ATTACK_TRIGGER);
    }

    private void Player_OnPlayerAttack(object sender, System.EventArgs e)
    {
        if (PlayerMoveState.Instance.IsMoving())
        {
            if (PlayerMoveState.Instance.IsRunning() && player.isGrounded && PlayerAttackState.Instance.GetRunningAttackCD() <= 0)
            {
                animator.SetTrigger(RUNNINGATTACK_TRIGGER);
                PlayerAttackState.Instance.SetRunningAttackCD(1.5f);
            }
            else
            {
                animator.SetTrigger(ATTACK_TRIGGER);
            }
        }
        else
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("2HAttack"))
            {
                animator.SetTrigger(ATTACK_TRIGGER2);
                animator.ResetTrigger(ATTACK_TRIGGER);
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("2HAttackCombo2"))
            {
                animator.SetTrigger(ATTACK_TRIGGER3);
                animator.ResetTrigger(ATTACK_TRIGGER2);
                animator.ResetTrigger(ATTACK_TRIGGER);
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("2HAttackCombo3"))
            {
                animator.SetTrigger(ATTACK_TRIGGER4);
                animator.ResetTrigger(ATTACK_TRIGGER3);
                animator.ResetTrigger(ATTACK_TRIGGER2);
                animator.ResetTrigger(ATTACK_TRIGGER);
            }
            else
            {
                animator.SetTrigger(ATTACK_TRIGGER);
            }
        }
        
    }

    private void Player_OnPlayerJump(object sender, System.EventArgs e)
    {
        animator.SetTrigger(JUMP_TRIGGER);
    }

    private void Update()
    {
        animator.SetBool(IS_MOVING, PlayerMoveState.Instance.IsMoving());
        animator.SetBool(IS_RUNNING, PlayerMoveState.Instance.IsRunning());
        if (player.isGrounded)
        {
            animator.SetBool(IS_GROUNDED, true);
        }
        else
        {
            animator.SetBool(IS_GROUNDED, false);
        }
        animator.SetBool(IS_FALLING, player.isFalling);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("RunningAttack2H") || animator.GetCurrentAnimatorStateInfo(0).IsName("2HAttack") || animator.GetCurrentAnimatorStateInfo(0).IsName("2HAttackCombo2") || animator.GetCurrentAnimatorStateInfo(0).IsName("2HAttackCombo3") || animator.GetCurrentAnimatorStateInfo(0).IsName("2HAttackCombo4"))
        {
            player.EnableSlashTrailRenderer();
            player.isAttacking = true;
            animator.SetBool(IS_ATTACKING,true);
        }
        else
        {
            player.DisableSlashTrailRenderer();
            player.isAttacking=false;
            animator.SetBool(IS_ATTACKING, false);
        }
    }
}
