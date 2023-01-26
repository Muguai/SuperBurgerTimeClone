using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlide : State
{
    PlayerStateMachine playerStateMachine;

    public PlayerWallSlide(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        this.playerStateMachine = stateMachine;

    }

    public override IEnumerator Start()
    {
        playerStateMachine.playerInput.canMove = true;
        playerStateMachine.anim.Play("WallSlide", 0, 0f);

        //    playerStateMachine.spriteRenderer.flipX = false;
        yield return null;
    }

    // Update is called once per frame
    public override void Update()
    {

        if (playerStateMachine.playerVelocity.holdWallContact == false)
        {
            playerStateMachine.anim.StopPlayback();

        }
        else
        {
            playerStateMachine.anim.Play("WallSlide", 0, 0f);

        }

        if (playerStateMachine.playerInput.JumpInputDown)
        {
            playerStateMachine.playerVelocity.OnJumpInputDown();
            playerStateMachine.SetState(new PlayerJumpSquat(playerStateMachine));

        }else if (playerStateMachine.playerMovement.grounded == true || playerStateMachine.playerVelocity.wallContact == false)
        {

            playerStateMachine.SetState(new PlayerFall(playerStateMachine, false));
        }

    }
}
