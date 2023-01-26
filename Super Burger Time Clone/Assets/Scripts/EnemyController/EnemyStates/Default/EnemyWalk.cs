using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalk : State
{
    EnemyStateMachine enemyStateMachine;


    public EnemyWalk(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        this.enemyStateMachine = stateMachine;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
