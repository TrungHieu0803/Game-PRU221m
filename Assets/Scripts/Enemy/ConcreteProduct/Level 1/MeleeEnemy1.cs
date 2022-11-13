using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MeleeEnemy1 : Melee
{
    //[SerializeField]
    //private Image healthBarSprite;
    //[SerializeField]
    //private Canvas healthBarCanvas;
    //[SerializeField]
    //private float damage;
    //private NavMeshAgent agent;
    //private Animator animator;
    //private Vector3 currentPosition;
    //[SerializeField]
    //private float maxHealth;
    //public float currentHealth;
    //private bool isDead;
    //private bool isHit;
    //private float spellDuration;
    //public EnemyType enemyType;


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
        hitDuration = 0f;
        isHit = false;
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
        animator.SetFloat("Walk", agent.nextPosition.x - currentPosition.x);
        animator.SetBool("IsStopped", agent.nextPosition.x == currentPosition.x);
        if (isHit)
        {
            Hit();
        }
        currentPosition = transform.position;
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
            animator.SetBool("IsHit", false);
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
        agent.speed = 0.5f;
        animator.SetBool("IsHit", true);
        animator.SetFloat("Horizontal", agent.nextPosition.x - currentPosition.x > 0 ? 1 : -1);
        Vibration.Vibrate(10);

    }
}
