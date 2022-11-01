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
    public bool showPath;
    public bool showAhead;
    private Animator animator;
    private Vector3 currentPosition;
    private float maxHealth;
    private float currentHealth;
    private bool isDead;
    private float spellDuration;
    private ObjectPool<GameObject> poolSpell;

    // Start is called before the first frame update
    void Start()
    {
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
        maxHealth = 100;
        currentHealth = 100;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 2;
        agent.updateRotation = false;
        agent.updateUpAxis = false;

    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = PlayerController.Instance.transform.position;
        animator.SetFloat("Fly", agent.nextPosition.x - currentPosition.x);
        currentPosition = transform.position;
        Hit();
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
            if (currentHealth < 0)
            {
                healthBarCanvas.enabled = false;    
                isDead = true;
            }
        }
        if (isDead && currentHealth != -1)
        {
            currentHealth = -1;
            agent.speed = 0;
            animator.SetTrigger("Death");
            SoundController.instance.playSound1();
            Destroy(gameObject, 1f);
            AmmoSpawner.Instance.SpawnRandomAmmo(currentPosition);
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
