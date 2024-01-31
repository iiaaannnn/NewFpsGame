using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using TMPro;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [Header("Nav Mesh")]
    private StateMachine stateMachine;
    private NavMeshAgent navMeshAgent;
    private GameObject player;

    private Vector3 lastKnownPos;

    public NavMeshAgent Agent { get => navMeshAgent; }
    public GameObject Player { get => player; }

    public Vector3 LastKnownPos { get => lastKnownPos; set => lastKnownPos = value; }
    [SerializeField] //debug
    private string currentState;

    public EnemyPath path;

    [Header("Enemy Vision")]
    public float sightDistance = 20f;
    public float fieldOfView = 80f;
    public float eyeHeight;

    [Header("Enemy Weapon")]
    public Transform gun;
    public float fireRate;

    [Header("Enemy Health")]
    private float enemyMaxHealth = 100f;
    private float enemyHealth;
    [SerializeField]
    FloatingHealthBar healthBar;

    public GameObject gameManager;

    [Header("Damage Numbers")]
    public GameObject dmgNumbersPrefab;
    public string dmgText;
    public Canvas canvas;
    float randomness = 20f;

    [Header("HitEffect")]
    public Material red;
    public Material white;

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        stateMachine.Initialise();
        player = GameObject.FindGameObjectWithTag("Player");

        enemyHealth = enemyMaxHealth;

        healthBar.UpdateHealthBar(enemyHealth, enemyMaxHealth);

    }

    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    // Update is called once per frame
    void Update()
    {
        CanSeePlayer();
        CheckIfDie();

        currentState = stateMachine.activeState.ToString();
    }

  

    public void TakeDamage(float damageTaken)
    {
        //update health
        enemyHealth -= damageTaken;
        healthBar.UpdateHealthBar(enemyHealth, enemyMaxHealth);

        //damage numbers
        GameObject dmgNumbersInstance = Instantiate(dmgNumbersPrefab, canvas.transform);
        float x = Random.Range(dmgNumbersInstance.transform.position.x - randomness, dmgNumbersInstance.transform.position.x + randomness);
        float y = Random.Range(dmgNumbersInstance.transform.position.y - randomness, dmgNumbersInstance.transform.position.y + randomness);

        dmgNumbersInstance.transform.position = new Vector3(x, y, 1);

        dmgText = damageTaken.ToString();
        dmgNumbersInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(dmgText);

        //hit effect
        gameObject.GetComponentInChildren<MeshRenderer>().material = red;
        StartCoroutine(ChangeBackColor(gameObject));
    }

    void CheckIfDie()
    {
        if(enemyHealth <= 0)
        {
            //spawn explosion when die
            GameObject explosion = GameObject.Instantiate(Resources.Load("Explosion") as GameObject, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(explosion, 2f);

            //add to kills
            gameManager.GetComponent<LevelInfo>().kills += 1;

            //destroy enemy go
            Destroy(gameObject);

            player.GetComponent<PlayerHealth>().RestoreHealth(10);
        }
    }

    public bool CanSeePlayer()
    {
        if(player != null)
        {
            //check if player is within sight distance
            if (Vector3.Distance(transform.position, player.transform.position) < sightDistance)
            {
                Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up * eyeHeight);

                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);

                //check if player is within fov
                if(angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);

                    RaycastHit hitInfo = new RaycastHit();
                    if(Physics.Raycast(ray, out hitInfo, sightDistance))
                    {
                        if(hitInfo.transform.gameObject == player)
                        {
                            //Debug.DrawRay(ray.origin, ray.direction * sightDistance);

                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }


    public IEnumerator ChangeBackColor(GameObject enemy)
    {
        yield return new WaitForSeconds(0.06f);
        enemy.GetComponentInChildren<MeshRenderer>().material = white;

    }

}
