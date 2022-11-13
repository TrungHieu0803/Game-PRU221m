using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RangeEnemy2 : Range
{

    private bool isHit;
    [SerializeField]
    private GameObject mouth;
    private bool isActive;

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
        isActive = true;
        enemyType = EnemyType.MELEE;
        isHit = false;
        isDead = false;
        if (currentHealth == 0)
        {
            currentHealth = maxHealth;
        }
        animator = GetComponent<Animator>();
        InvokeRepeating("Hit", 1f, 3f);

    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            agent.destination = PlayerController.Instance.transform.position;
            animator.SetFloat("HorizontalMove", agent.destination.x - currentPosition.x);
            if (!isHit)
            {
                agent.speed = 2;
                animator.SetBool("IsMove", true);
            }
            currentPosition = transform.position;
        }else
        {
            agent.speed = 0;
            if (isActive)
            {
                gameObject.GetComponent<Collider2D>().enabled = false;
                currentHealth = -1;
                animator.SetTrigger("Death");
                SoundController.instance.playSound1();
                Destroy(gameObject, 0.5f);
                AmmoSpawner.Instance.SpawnRandomAmmo(currentPosition);
                ItemSpawner.Instance.SpawnRandomItem(currentPosition + new Vector3(0.5f, 0f));
                isActive = false;
            }
        }    
    }

    public new void Hit()
    {
        if (agent.remainingDistance < 6 && !isDead)
        {
            isHit = true;
            StartCoroutine(Spell());
        }
    }

    public IEnumerator Spell()
    {
        agent.speed = 0;
        animator.SetBool("IsHit", true);
        yield return new WaitForSeconds(1.1f);
        if (!isDead)
        {
            isHit = false;
            GameObject spell = Instantiate<GameObject>(spellPrefab, mouth.transform.position, Helper.GetRotation(mouth.transform.position, PlayerController.Instance.transform.position));
            spell.GetComponent<Fireball>().damage = damage;
            Rigidbody2D spell_body = spell.GetComponent<Rigidbody2D>();
            spell_body.AddForce((PlayerController.Instance.transform.position - mouth.transform.position).normalized * 10f, ForceMode2D.Impulse);  
        } else
        {
            yield return null;
        }
        animator.SetBool("IsHit", false);
    }
}
