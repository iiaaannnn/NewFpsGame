using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    public int waypointIndex;
    float waitTimer;
    public override void Enter()
    {
        
    }
    public override void Perform()
    {
        PatrolCycle();
        if(enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
    }
    public override void Exit()
    {

    }
    public void PatrolCycle()
    {
        if(enemy.Agent.remainingDistance < 0.2)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer > 0.0)
            {
                if (waypointIndex < enemy.path.waypoints.Count - 1)
                {
                    waypointIndex++;
                }
                else
                {
                    waypointIndex = 0;
                }

                enemy.Agent.SetDestination(enemy.path.waypoints[waypointIndex].position);

                waitTimer = 0;
            }
              
        }
    }
}
