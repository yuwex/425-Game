using UnityEngine;
using UnityEngine.AI;
using System.Collections;


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

    void Start()
    {
        // Setup default variable
        health = data.baseHealth;
        attackDamage = data.attackDamage;
        baseDamage = data.baseDamage;
        coinReward = data.coinReward;

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

        health = Mathf.Clamp(health - damage, 0, Mathf.Infinity);
        healthBar.SetValue((int)health);

        OnHurt(damage);
        if (health <= 0)
        {
            OnEnemyDie();

            GameManager.Instance.updateCoins(coinReward);

            if (modifierDrop) CreateModifierDrop();

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
    }

}
