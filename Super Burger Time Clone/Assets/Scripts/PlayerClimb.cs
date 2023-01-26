using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimb : State
{

    PlayerStateMachine playerStateMachine;

    private float originalGraivity;


    public PlayerClimb(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        this.playerStateMachine = stateMachine;
    }


    public override IEnumerator Start()
    {

        originalGraivity = playerStateMachine.playerVelocity.gravity;
        playerStateMachine.playerVelocity.velocity = Vector2.zero;
        playerStateMachine.playerVelocity.oldVelocity = Vector2.zero;

        playerStateMachine.playerVelocity.directionalInput.x = 0f;
        playerStateMachine.playerVelocity.gravity = 0f;
        playerStateMachine.playerVelocity.climbing = true;
        playerStateMachine.playerInput.cantMoveX = true;

        RaycastHit2D hit = Physics2D.Raycast(playerStateMachine.transform.position, Vector2.up, playerStateMachine.ladderDistance, playerStateMachine.ladderMask);
        if (hit.collider != null)
        {
            playerStateMachine.transform.position = new Vector2(hit.collider.bounds.center.x, playerStateMachine.transform.position.y);

        }


        playerStateMachine.anim.Play("Climb", 0, 0f);

        yield return null;
    }


    // Update is called once per frame
    public override void Update()
    {
        if (playerStateMachine.playerVelocity.directionalInput.y == 0f)
        {
            playerStateMachine.anim.Play("Climb", 0, 0f);
        }
        else
        {
            playerStateMachine.anim.StopPlayback();

        }


        if (playerStateMachine.playerInput.JumpInputDown)
        {
            if(playerStateMachine.playerVelocity.directionalInput.y < 0)
            {
                playerStateMachine.SetState(new PlayerIdle(playerStateMachine));
            }
            else
            {
                playerStateMachine.playerVelocity.OnJumpInputDown();
                playerStateMachine.SetState(new PlayerJumpSquat(playerStateMachine));
            }
        }
        else if (!playerStateMachine.CheckForLadder(true))
        {
            playerStateMachine.SetState(new PlayerIdle(playerStateMachine));
        }else if (playerStateMachine.playerMovement.grounded == true)
        {
            playerStateMachine.SetState(new PlayerIdle(playerStateMachine));
        }

    }

    public override void Exit()
    {
        playerStateMachine.StartCoroutine(playerStateMachine.LadderCooldown());
        playerStateMachine.playerVelocity.climbing = false;
        playerStateMachine.playerVelocity.gravity = originalGraivity;

        playerStateMachine.playerInput.cantMoveX = false;
    }


}
