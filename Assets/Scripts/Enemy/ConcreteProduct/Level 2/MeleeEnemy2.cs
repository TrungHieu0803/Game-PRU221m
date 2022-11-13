using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MeleeEnemy2 : Melee
{

    private bool isSpell;
    [SerializeField]
    private GameObject spellPrefab;
    [SerializeField] 
    private float spellDamage;

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
        hitDuration = 0f;
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
            Destroy(gameObject, 0.5f);
            AmmoSpawner.Instance.SpawnRandomAmmo(currentPosition);
            ItemSpawner.Instance.SpawnRandomItem(currentPosition + new Vector3(0.5f, 0f));
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

    public new void Hit()
    {
        if (hitDuration == 0f)
        {
            PlayerController.Instance.currentHealth -= damage;
        }
        else if (hitDuration >= 2f)
        {
            PlayerController.Instance.currentHealth -= damage;
            hitDuration = 0f;
        }
        hitDuration += Time.deltaTime;
        animator.SetBool("IsJump", true);
        agent.speed = 0.5f;
        Vibration.Vibrate(10);
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
