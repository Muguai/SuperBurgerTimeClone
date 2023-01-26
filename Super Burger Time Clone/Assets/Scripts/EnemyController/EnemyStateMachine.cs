using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{


    [HideInInspector] public Movement enemyMovement;
    [HideInInspector] public EnemyVelocity enemyVelocity;


    // Start is called before the first frame update
    void Start()
    {
        enemyVelocity = GetComponent<EnemyVelocity>();
        enemyMovement = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        state.Update();

    }

    void FixedUpdate()
    {
        state.FixedUpdate();

    }
}
