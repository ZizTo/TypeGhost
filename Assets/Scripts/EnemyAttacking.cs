using UnityEngine;
using System.Collections;

public class EnemyAttacking : MonoBehaviour
{
    public float attackRange = 2f;
    GameObject player;
    public float delay = 2f;
    private bool IsInRange;
  
    void Start()
    {
        
    }

    
    void Update()
    {
       
        
        
    }


    private void CheckHit()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.parent.position, attackRange);

        foreach (Collider2D collider in hitColliders)
        {
           
            if (collider.gameObject.CompareTag("enemy"))
            {
                StartCoroutine(DelayedAction());
                Debug.Log("Враг вас аттаковал");
                player.GetComponent<FightPreparing>().FightStarting(collider.gameObject, collider.transform.position);
                break;
            }
        }
    }


    private IEnumerator DelayedAction()
    {
        Debug.Log("Задержка началась");
        yield return new WaitForSeconds(delay);
        if (IsInRange)
        {
            CheckHit();
        }
        Debug.Log("Задержка прошла");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {   
        if (collision.gameObject.tag == "Player")
        {
            IsInRange = true;
            player = collision.gameObject;
            if (player.GetComponent<FightPreparing>().fightActive == false)
            {
                StartCoroutine(DelayedAction());
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            IsInRange = false;
        }
    }
}
