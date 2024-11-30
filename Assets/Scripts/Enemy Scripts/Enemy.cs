using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;


public class Enemy : MonoBehaviour
{
    public EnemyData data;
    public GameObject target;
    public GameObject mainBase;
    private NavMeshAgent agent;
    private InfoBar healthBar;

    public ModifierBase modifierDrop = null;
    public GameObject modifierPrefab = null;
    public float health = 0f;
    private float attackDamage;
    private float baseDamage;
    private int coinReward;
    public float activationDistance = 10f;
    private float attackRange;
    private float attackDelay;

    private bool trapped = false;

    void Start()
    {
        // Setup default variable
        health = data.baseHealth;
        attackDamage = data.attackDamage;
        baseDamage = data.baseDamage;
        coinReward = data.coinReward;
        attackRange = data.attackRange;
        attackDelay = data.attackDelay;

        // Setup NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
        data.SetupNavMeshAgent(agent);
        agent.SetDestination(target.transform.position);

        // Setup Health Bar
        healthBar = GetComponent<InfoBar>();
        healthBar.SetMaxValue((int)health);
        healthBar.SetValue((int)health);
    }

    void Update()
    {
        if (!trapped && agent.path.status != NavMeshPathStatus.PathComplete)
        {
            Collider[] nearby = Physics.OverlapSphere(transform.position, 10f);
            List<GameObject> walls = new List<GameObject>();
            foreach (Collider collider in nearby)
            {
                if (collider.gameObject.CompareTag("Wall") || collider.gameObject.CompareTag("Tower"))
                {
                    walls.Add(collider.gameObject);
                }
            }
            if (walls.Count > 0)
            {
                trapped = true;
                StartCoroutine(Escape(walls));
            }
            else
            {
                Debug.Log("Help me bro I'm stuck!");
            }
        }

        if (!target)
            return;

        if ((target.transform.position - transform.position).magnitude < activationDistance)
        {
            if (target.gameObject == mainBase)
            {
                TowerHealth health = mainBase.GetComponent<TowerHealth>();
                if (health)
                {
                    health.TowerDamage((int)baseDamage);
                    Destroy(gameObject);
                }
            }
        }
    }

    public void Hurt(float damage)
    {
        if (health <= 0)
            return;

        health = Mathf.Clamp(health - damage, 0, Mathf.Infinity);
        healthBar.SetValue((int)health);

        OnHurt(damage);
        if (health <= 0)
        {
            OnEnemyDie();

            GameManager.Instance.updateCoins(coinReward);

        }
    }

    void CreateModifierDrop()
    {
        var upgradePickup = Instantiate(modifierPrefab, new Vector3(transform.position.x, 2, transform.position.z), Quaternion.identity).GetComponent<UpgradePickup>();
        upgradePickup.modifier = modifierDrop;
    }

    public virtual void OnHurt(float dmg) { }

    public virtual void OnEnemyDie()
    {
        GetComponentInChildren<AnimationController>().isDead = true;
        StartCoroutine(PlayDeathAnimationAndDestroy());
    }

    private IEnumerator PlayDeathAnimationAndDestroy()
    {
        Animator animator = GetComponentInChildren<Animator>();
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        float deathAnimationLength = stateInfo.length;

        yield return new WaitForSeconds(deathAnimationLength - 0.3f);
        Destroy(gameObject);

        if (modifierDrop) CreateModifierDrop();
    }

    private IEnumerator Escape(List<GameObject> walls)
    {
        while (trapped)
        {
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(target.transform.position, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                trapped = false;
                agent.isStopped = false;
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                agent.SetDestination(target.transform.position);
                break;
            }

            Collider[] nearby = Physics.OverlapSphere(transform.position, 10f);
            walls = new List<GameObject>();
            foreach (Collider collider in nearby)
            {
                if (collider.gameObject.CompareTag("Wall") || collider.gameObject.CompareTag("Tower"))
                {
                    walls.Add(collider.gameObject);
                }
            }

            GameObject bestWall = walls[0];
            int leastHealth = walls[0].GetComponent<BuildingHealth>().health;
            foreach (GameObject wall in walls.GetRange(1, walls.Count - 1))
            {
                int wallHealth = wall.GetComponent<BuildingHealth>().health;
                if (wallHealth < leastHealth || (wallHealth == leastHealth && Vector3.Distance(mainBase.transform.position, wall.transform.position) < Vector3.Distance(mainBase.transform.position, bestWall.transform.position)))
                {
                    bestWall = wall;
                    leastHealth = wallHealth;
                }
            }
            agent.SetDestination(bestWall.transform.position);

            if (Vector3.Distance(transform.position, bestWall.transform.position) < attackRange)
            {
                agent.isStopped = true;
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                bestWall.GetComponent<BuildingHealth>().Hurt((int)attackDamage);
                yield return new WaitForSeconds(attackDelay);
            }
            else
            {
                agent.isStopped = false;
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                yield return null;
            }
        }
    }

}
