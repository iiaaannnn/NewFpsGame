using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AttackState : BaseState
{
    public float moveTimer;
    private float losePlayerTimer;
    private float shotTimer;
    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Perform()
    {
        if(enemy.CanSeePlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;

            enemy.transform.LookAt(enemy.Player.transform);

            if(shotTimer > enemy.fireRate)
            {
                Shoot();
            }

            if(moveTimer > Random.Range(3, 7))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                moveTimer = 0;
            }
            enemy.LastKnownPos = enemy.Player.transform.position;
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer > 8)
            {
                //set state to patrol state
                stateMachine.ChangeState(new SearchState());
            }
        }
    }

    public void Shoot()
    {
        Transform gun = enemy.gun;

        //load bullet from resources
        GameObject bullet = GameObject.Instantiate(Resources.Load("bullet") as GameObject, gun.position, enemy.gun.rotation);

        //get shoot direction
        Vector3 shootDirection = (enemy.Player.transform.position - gun.transform.position).normalized;

        //addd velocty in shoot direction
        bullet.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(Random.Range(-3f, 3f), Vector3.up) * shootDirection * 40;

        shotTimer = 0;
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
