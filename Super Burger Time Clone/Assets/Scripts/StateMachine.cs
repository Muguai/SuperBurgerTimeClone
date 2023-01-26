using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class StateMachine : MonoBehaviour
{
    protected State state;
    public TextMeshPro debugText;
    public int ignoreBeginingOfDebugText;

    public void SetState(State state)
    {
        if(this.state != null)
        {
            this.state.Exit();
        }
        this.state = state;
        StartCoroutine(state.Start());
        if(debugText != null)
        {
            string str = state.GetType().ToString();
            debugText.text = str.Substring(ignoreBeginingOfDebugText, str.Length - ignoreBeginingOfDebugText);
        }

    }

    public State GetCurrentState()
    {
        return state;
    }


}
