using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using UnityEngine.UI;

public class RangeEnemy1 : MonoBehaviour, IRangeEnemy
{

    [SerializeField]
    private Image healthBarSprite;
    [SerializeField]
    private Canvas healthBarCanvas;
    [SerializeField]
    private GameObject spellPrefab;
    [SerializeField]
    private float damage;
    private NavMeshAgent agent;
    private Animator animator;
    private Vector3 currentPosition;
    [SerializeField]
    private float maxHealth;
    public float currentHealth;
    private bool isDead;
    private float spellDuration;
    private ObjectPool<GameObject> poolSpell;
    public EnemyType enemyType;

    // Start is called before the first frame update
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 2;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    void Start()
    {
        enemyType = EnemyType.RANGE;
        poolSpell = new ObjectPool<GameObject>(() =>
        {
            return Instantiate(spellPrefab);
        }, spell =>
        {
            spell.SetActive(true);
        }, spell =>
        {
            spell.SetActive(false);
        }, spell =>
        {
            Destroy(spell);
        }, false, 10, 20
       );
        spellDuration = 0;
        isDead = false;
        if(currentHealth == 0)
        {
            currentHealth = maxHealth;
        }
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = PlayerController.Instance.transform.position;
        animator.SetFloat("Fly", agent.nextPosition.x - currentPosition.x);
        currentPosition = transform.position;
        Hit();
        if (isDead)
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
            currentHealth = -1;
            agent.speed = 0;
            animator.SetTrigger("Death");
            SoundController.instance.playSound1();
            Destroy(gameObject, 1f);
            AmmoSpawner.Instance.SpawnRandomAmmo(currentPosition);
            isDead = false;
        }
    }

    public void UpdateHealthBar()
    {
        healthBarSprite.fillAmount = currentHealth / maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Bullet" && !isDead)
        {
            Destroy(collision.gameObject);
            currentHealth -= collision.gameObject.GetComponent<Bullet>().damage;
            UpdateHealthBar();
            agent.speed = 1;
        }
        if (collision.gameObject.tag == "Explosion" && !isDead)
        {
            
            currentHealth -= collision.gameObject.GetComponent<Explosion>().explosionDamage;
            UpdateHealthBar();
            agent.speed = 1;
        }
        if (currentHealth <= 0)
        {
            healthBarCanvas.enabled = false;
            isDead = true;
            GameManager.instance.UpdateKill();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet" && !isDead)
        {
            agent.speed = 2;
        }
    }

    public void Hit()
    {
       if(agent.remainingDistance < 5f)
        {
            spellDuration += Time.deltaTime;
            agent.speed = 0;
            if (spellDuration > 3f)
            {
                spellDuration = 0f;
                GameObject spell = poolSpell.Get();
                spell.transform.position = PlayerController.Instance.transform.position;
                spell.GetComponent<CircleCollider2D>().enabled = false;
                spell.GetComponent<SpellEnemyRange1>().damage = damage;
            }
        }
       else
        {
            agent.speed = 2;
        }
    }
}
