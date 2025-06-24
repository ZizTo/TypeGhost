using UnityEngine;
using System.Collections.Generic;

public class AttackEnemy : MonoBehaviour
{
    public GameObject attackPrefab;
    bool attacking = false;
    public bool fightActive = false;

    float timer = 0;
    float timeForAttack = 0.2f;
    [SerializeField] float cooldown = 0.5f;
    GameObject attackObject;

    private void Update()
    {
        if (fightActive) return;
        if (timer > -cooldown) { timer -= Time.deltaTime; }
        if (attacking)
        {
            List<Collider2D> hitColl = new List<Collider2D>();
            attackObject.GetComponent<Collider2D>().Overlap(hitColl);

            foreach (Collider2D collider in hitColl)
            {
                if (collider.gameObject.CompareTag("hitRange"))
                {
                    Debug.Log("Попал по врагу");
                    if (GetComponent<FightPreparing>().isActiveAndEnabled)
                    {
                        GetComponent<FightPreparing>().FightStarting(collider.gameObject, collider.transform.position);
                    }
                    else if (GetComponent<NewFight>().isActiveAndEnabled)
                    {
                        GetComponent<NewFight>().FightStarting(collider.transform.parent.gameObject);
                    }

                    Vector2 direction = (collider.transform.position - transform.position).normalized;
                    collider.transform.parent.GetComponent<RBTests>().GiveForce(direction.x, direction.y, GetComponent<EnemyInfo>().GetDmg());
                    attacking = false;
                    break;
                }
            }
            if (timer < 0)
            {
                attacking = false;
            }
        }
        if (!attacking && attackObject != null) { Destroy(attackObject); }
        if (Input.GetMouseButtonDown(0) && timer <= -cooldown)
        {
            if (!GetComponent<FightPreparing>().fightActive)
            {
                StartAttacking();
            }
        }
    }

    private void StartAttacking()
    {
        timer = timeForAttack;
        attackObject = Instantiate(attackPrefab, transform);
        attacking = true;
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector2 direc = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
        attackObject.transform.right = direc;
    }
}