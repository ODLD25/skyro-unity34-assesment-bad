using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //Settings
    [SerializeField]private float speed = 2.4f;
    [SerializeField]private int maxHealth = 5;
    [SerializeField]private int scoreToAdd = 1;
    
    //Stats
    [SerializeField]private float curHealth;
    
    //Attack
    [SerializeField]private float attackDst;
    [SerializeField]private float dmg;
    [SerializeField]private float attackCooldown;
    private bool canAttack;
    
    //References
    [SerializeField]private GameObject hitParticle;
    private Transform player;

    void Start()
    {
        player = GameManager.instance.GetPlayer().transform;

        canAttack = true;

        curHealth = maxHealth;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    
        if (canAttack && Time.frameCount % 10 == 0)
        {
            if (Vector2.Distance(transform.position, player.position) <= attackDst && canAttack)
            {
                Attack();
            }
        }
    }

    public void DealDamage(float amount)
    {
        curHealth -= amount;

        if (curHealth <= 0)
        {
            GameManager.instance.AddScore(scoreToAdd);
            Destroy(gameObject);
        }
    }

    private void Attack()
    {
        player.GetComponent<PlayerController>().DealDmg(dmg);
        canAttack = false;

        Invoke(nameof(ResetAttack), attackCooldown);
    }

    private void ResetAttack()
    {
        canAttack = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ammo"))
        {
            Destroy(collision.gameObject);
            SpawnHitParticle();
            DealDamage(1);
        }
    }

    private void SpawnHitParticle()
    {
        GameObject spawnedHitParticle = Instantiate(hitParticle, transform.position + (player.position - transform.position).normalized * 0.5f, Quaternion.identity);
        Destroy(spawnedHitParticle, 2f);
    }
}
