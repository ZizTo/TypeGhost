using UnityEngine;
using System.Collections;

public class Enemy_Wander : MonoBehaviour
{
    [Header("Wander Area")]
    public float startwanderWidth = 40;
    public float startwanderHeight = 40;
    public Vector2 startingPosition;

    public bool IsWandering = true;
    public int pauseDuration = 1;
    public float speed = 2;
    public Vector2 target;
    public GameObject coliderToFind;
    public bool IsStucked;
    public float gizmosDistance = 1f;
    public float gizmosNumber;
    public Vector2 gizmosPos;

    private Rigidbody2D rb;
    private bool isPaused2;
    private float wanderWidth;
    private float wanderHeight;
    private float dividedwanderWidth;
    private float dividedwanderHeight;
    



    void Start()
    {
        wanderHeight = startwanderHeight;
        wanderWidth = startwanderWidth;
        dividedwanderHeight = startwanderHeight / 4;
        dividedwanderWidth = startwanderWidth / 4;
        
    }

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void OnEnable() 
    {
        target = GetRandomTarget();
        
    }

    private void FixedUpdate()
    {
        if (IsWandering)
        {
            if (isPaused2)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }
            
            if (IsStucked)
            {
                Vector2 gizmosPos = transform.position + transform.forward * gizmosDistance;
                gizmosNumber = 0.5f;
                wanderHeight = dividedwanderHeight;
                wanderWidth = dividedwanderWidth;
                rb.linearVelocity = Vector2.zero;
                StartCoroutine(PauseAndPickNewDestination());

            }
            else
            {
                Vector2 gizmosPos = startingPosition;
                gizmosNumber = 0;
                wanderHeight = startwanderHeight;
                wanderWidth = startwanderWidth;

            }
            if(Vector2.Distance(transform.position, target) < .1f && !IsStucked)
            {
                StartCoroutine(PauseAndPickNewDestination());
            }

            Move();
            

            
        }
    }

    
    private void Move()
    {
        if (IsWandering)
        {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        if (direction.x < 0 && coliderToFind.GetComponent<Enemy_movement>().FacingDirection == 1 && !IsStucked|| direction.x > 0 && coliderToFind.GetComponent<Enemy_movement>().FacingDirection == -1 && !IsStucked)
            {
                coliderToFind.GetComponent<Enemy_movement>().Flip();
            }
        rb.linearVelocity = direction * speed;
        }
    }
    


    IEnumerator PauseAndPickNewDestination()
    {
        isPaused2 = true;
        yield return new WaitForSeconds(pauseDuration);

        target = GetRandomTarget();
        isPaused2 = false;

    }



    private Vector2 GetRandomTarget()
    {
        float halfwidth = wanderWidth / 2;
        float halfHeight = wanderHeight / 2;
        int edge = Random.Range(0, 4);

        return edge switch 
        {
            0 => new Vector2(gizmosPos.x - halfwidth, Random.Range(gizmosPos.y - halfHeight, gizmosPos.y + halfHeight)),//Left
            1 => new Vector2(gizmosPos.x + halfwidth, Random.Range(gizmosPos.y - halfHeight, gizmosPos.y + halfHeight)),//Right
            2 => new Vector2(Random.Range(gizmosPos.x - halfwidth, gizmosPos.x + halfwidth), gizmosPos.y - halfHeight),//Bottom
            _ => new Vector2(Random.Range(gizmosPos.x - halfwidth, gizmosPos.x + halfwidth), gizmosPos.y + halfHeight),//Top

        };
    }
    
    
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "wall")
        {
            IsStucked = true;
        }
    }


    private void OnCollisionExit2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "wall")
        {
            IsStucked = false;
        }
    }



    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(gizmosPos, new Vector3(wanderWidth, wanderHeight, gizmosNumber));
        
    }


    
    
}
