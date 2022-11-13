using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using UnityEngine.UI;

public class RangeEnemy1 : Range
{


    private ObjectPool<GameObject> poolSpell;


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
        if (currentHealth == 0)
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
            Destroy(gameObject, 0.5f);
            AmmoSpawner.Instance.SpawnRandomAmmo(currentPosition);
            ItemSpawner.Instance.SpawnRandomItem(currentPosition + new Vector3(0.5f, 0f));
            isDead = false;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {

        agent.speed = 2;

    }

    public new void Hit()
    {
        if (agent.remainingDistance < 5f)
        {
            spellDuration += Time.deltaTime;
            agent.speed = 0;
            if (spellDuration > 2f)
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
