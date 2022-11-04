using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MeleeEnemy2 : MonoBehaviour
{
    [SerializeField]
    private Image healthBarSprite;
    [SerializeField]
    private Canvas healthBarCanvas;
    [SerializeField]
    private float damage;
    private NavMeshAgent agent;
    private Animator animator;
    private Vector3 currentPosition;
    [SerializeField]
    private float maxHealth;
    public float currentHealth;
    private bool isDead;
    private bool isSpell;
    private float spellDuration;
    private bool isHit;
    [SerializeField]
    private GameObject spellPrefab;
    [SerializeField] 
    private float spellDamage;
    public EnemyType enemyType;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 2;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyType = EnemyType.MELEE;
        isHit = false;
        spellDuration = 0f;
        isSpell = false;
        isDead = false;
        if(currentHealth == 0)
        {
            currentHealth = maxHealth;
        }
        animator = GetComponent<Animator>();
        InvokeRepeating("ActiveSpell", 5f, 5f);

    }

    // Update is called once per frame
    void Update()
    {

        if (isDead)
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
            currentHealth = -1;
            agent.speed = 0;
            animator.SetTrigger("Death");
            SoundController.instance.playSound1();
            Destroy(gameObject, 1.1f);
            AmmoSpawner.Instance.SpawnRandomAmmo(currentPosition);
            isDead = false;
        }
        else
        {
            agent.destination = PlayerController.Instance.transform.position;
            animator.SetFloat("HorizontalMove", agent.destination.x - currentPosition.x);
            if (!isSpell)
            {
                animator.SetBool("IsMove", true);
                if (isHit)
                {
                    Hit();
                }
            }
            currentPosition = transform.position;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isHit = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isHit = false;
            animator.SetBool("IsJump", false);
            agent.speed = 2;
        }
    }

    public void Hit()
    {
        if (spellDuration == 0f)
        {
            PlayerController.Instance.currentHealth -= damage;
        }
        else if (spellDuration >= 2f)
        {
            PlayerController.Instance.currentHealth -= damage;
            spellDuration = 0f;
        }
        spellDuration += Time.deltaTime;
        animator.SetBool("IsJump", true);
        agent.speed = 0.5f;

    }

    public void ActiveSpell()
    {
        isSpell = true;
        agent.speed = 4;
        StartCoroutine(Spell());

    }

    public IEnumerator Spell()
    {
        animator.SetBool("IsJump", true);
        yield return new WaitForSeconds(1f);
        isSpell = false;
        GameObject spell = Instantiate<GameObject>(spellPrefab, transform.position, Quaternion.identity);
        spell.GetComponent<Smoke>().SetSpellDamage(spellDamage);
        animator.SetBool("IsJump", false);
    }

}
