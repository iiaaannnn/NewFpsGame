using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState activeState;

    public PatrolState patrolState;
    public void Initialise()
    {
        ChangeState(new PatrolState());
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame                                                                                                                                                                                                                                                              
    void Update()
    {
        if(activeState != null)
        {
            activeState.Perform();
        }
    }

    public void ChangeState(BaseState newState)
    {
        //check if there is ann active state
        if(activeState != null)
        {
            activeState.Exit();
        }

        //set new state
        activeState = newState;

        //make sure the new state is not null
        if(activeState != null)
        {
            //set up new state
            activeState.stateMachine = this;
            activeState.enemy = GetComponent<Enemy>();
            activeState.Enter();
        }           
    }
}
                                                                                                                                                                                                                                                                                    