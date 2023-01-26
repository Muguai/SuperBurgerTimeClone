using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : State
{

    PlayerStateMachine playerStateMachine;

    private float translation;
    private float walkSpeed = 0.1f;
    private bool canJump = true;

    public PlayerRun(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        this.playerStateMachine = stateMachine;
    }


    public override IEnumerator Start()
    {
        playerStateMachine.playerInput.canMove = true;

        playerStateMachine.anim.Play("Run", 0, 0f);
        yield return null;
    }

    public override void Update()
    {
        if(Input.GetAxis("Horizontal") == 0)
        {
            playerStateMachine.SetState(new PlayerIdle(playerStateMachine));
        }else if (playerStateMachine.playerMovement.grounded == false)
        {
            playerStateMachine.SetState(new PlayerFall(playerStateMachine, false));
        }else if (playerStateMachine.playerInput.JumpInputDown && canJump)
        {
            playerStateMachine.playerVelocity.OnJumpInputDown();
            playerStateMachine.SetState(new PlayerJumpSquat(playerStateMachine));
        }
        else if (playerStateMachine.CheckForLadder(false) && playerStateMachine.canClimb)
        {
            playerStateMachine.SetState(new PlayerClimb(playerStateMachine));
        }
        else if (playerStateMachine.playerMovement.slidingDownMaxSlope == true)
        {
            canJump = false;
            if (Mathf.Abs(playerStateMachine.playerVelocity.velocity.y) > Mathf.Abs(playerStateMachine.playerVelocity.velocity.x))
            {
                playerStateMachine.SetState(new PlayerSlopeSlide(playerStateMachine));
            }

        }


    }

    public override void FixedUpdate()
    {
        
    }
}
