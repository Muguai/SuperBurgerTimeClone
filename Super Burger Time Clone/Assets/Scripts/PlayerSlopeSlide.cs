using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlopeSlide : State
{

    PlayerStateMachine playerStateMachine;

    public PlayerSlopeSlide(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        this.playerStateMachine = stateMachine;

        playerStateMachine.playerInput.holdX = true;
        playerStateMachine.playerVelocity.gravityIncrease = playerStateMachine.SlideGravityIncrease;
        playerStateMachine.playerInput.holdXValue = playerStateMachine.playerMovement.SlopeHitLeft ? 1f : -1f;
        playerStateMachine.playerInput.holdXValue *= playerStateMachine.slideFinishBoost;
        playerStateMachine.SpriteObject.transform.localPosition = new Vector2(0, -0.1f);
    }

    public override IEnumerator Start()
    {
        playerStateMachine.anim.Play("SlopeSlideStart", 0, 0f);
        Debug.Log("SLOPE");
        yield return null;
    }

    // Update is called once per frame
    public override void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerStateMachine.SpriteObject.transform.position, -Vector2.up, 2f, playerStateMachine.playerMovement.collisionMask);
        if (hit.collider != null)
        {
            Vector2 newUp = hit.normal;
           // playerStateMachine.SpriteObject.transform.up = newUp;
        }
        if (playerStateMachine.playerMovement.slidingDownMaxSlope == false)
        {
            playerStateMachine.SetState(new PlayerIdle(playerStateMachine));

        }
        else if (playerStateMachine.playerInput.JumpInputDown)
        {
           // playerStateMachine.playerVelocity.OnJumpInputDown();
           // playerStateMachine.SetState(new PlayerJumpSquat(playerStateMachine));
        }
    }

    public override void Exit()
    {
       // playerStateMachine.playerVelocity.velocity = new Vector2(5f, 0f);
        playerStateMachine.playerVelocity.gravityIncrease = 1;
        playerStateMachine.SpriteObject.transform.localPosition = Vector2.zero;
        playerStateMachine.SpriteObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        playerStateMachine.playerInput.holdX = false;
    }
}
