using UnityEngine;

public class Enemy_movement : MonoBehaviour
{
    public float speed;
    public bool isChasing;
    private Rigidbody2D rb;
    GameObject player;
    public int FacingDirection = 1;
    private SpriteRenderer sr;
    private FightPreparing FightPreparingObject;
    private Enemy_Patrol ep;
    CircleCollider2D cc;
    private Enemy_Wander ew;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
        sr = transform.parent.GetComponent<SpriteRenderer>();
        ep = transform.parent.GetComponent<Enemy_Patrol>();
        ew = transform.parent.GetComponent<Enemy_Wander>();
        cc = GetComponent<CircleCollider2D>();
    }
    
    // Update is called once per frame
    private void FixedUpdate() 
    {
        if (player == null) return;
        if (isChasing == true)
        {
            //Flip
            if (player.transform.position.x > rb.position.x && FacingDirection == -1 && FightPreparingObject.fightActive == false || player.transform.position.x < rb.position.x && FacingDirection == 1 && FightPreparingObject.fightActive == false)
            {
                Flip();
            }

            //walking
            if (FightPreparingObject.fightActive == false)
            {
                //IDontRememberThis
                if (FightPreparingObject.fightActive == false && FacingDirection == 1 && transform.localScale.x < 0)
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                }
                
                //ThiS here is walking
                Vector2 direction = (new Vector2(player.transform.position.x - rb.position.x, player.transform.position.y - rb.position.y)).normalized;
                rb.linearVelocity = direction * speed;
            }

            //SwitchingPositionInFight
            if (FightPreparingObject.fightActive == true)
            {
                if (FacingDirection == -1)
                {
                    Debug.Log("fd");
                    Flip();
                    
                }
                rb.linearVelocity = Vector2.zero;
            }
           
            
        }
        

    }


    public void Flip()
    {
        FacingDirection *= -1;


        //IfObjectLooksToTheLeftSide
        if (FacingDirection == 1)
        {
            sr.flipX = false;
            cc.offset = new Vector2(GetComponent<CircleCollider2D>().offset.x * -1, GetComponent<CircleCollider2D>().offset.y);
            rb.GetComponent<EnemyInfo>().ChangeWatchLeft(false);
        }


        //IfObjectLooksToTheRightSide
        if (FacingDirection == -1)
        {
            sr.flipX = true;
            cc.offset = new Vector2(GetComponent<CircleCollider2D>().offset.x * -1, GetComponent<CircleCollider2D>().offset.y);
            rb.GetComponent<EnemyInfo>().ChangeWatchLeft(true);
        }
    }

   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
            ep.IsPatroling = false;
            ew.IsWandering = false;
            isChasing = true;
            FightPreparingObject = player.GetComponent<FightPreparing>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ep.IsPatroling = true;
            ew.IsWandering = true;
            isChasing = false;
            rb.linearVelocity = Vector2.zero;
        }
    }
  
}
