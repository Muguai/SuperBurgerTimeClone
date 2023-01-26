using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
   // protected StateMachine stateMachine;

    public State(StateMachine stateMachine)
    {
       // this.stateMachine = stateMachine;
    }

    public virtual IEnumerator Start()
    {
        yield break;
    }

    public virtual void Update()
    {

    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void Exit()
    {

    }

}
