using UnityEngine;
using System.Collections;

public class Enemy_Patrol : MonoBehaviour
{
    public Vector2 [] patrolPoints;
    public float speed = 2;
    public float pauseDirection = 1.5f;
    public bool IsPatroling = true;

    private bool isPaused;
    private int currentPatrolIndex;
    private Vector2 target;

    private Rigidbody2D rb;

    public GameObject coliderToFind;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (patrolPoints.Length == 0) return;
        StartCoroutine(SetPatrolPoint());
        
    }


    void FixedUpdate()
    {
        if (patrolPoints.Length == 0) return;
        //StopsTheObject
        if (isPaused)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }


        Vector2 direction = ((Vector3)target - transform.position).normalized;

        //Flip
        if (IsPatroling)
        {
            if (direction.x < 0 && coliderToFind.GetComponent<Enemy_movement>().FacingDirection == 1 || direction.x > 0 && coliderToFind.GetComponent<Enemy_movement>().FacingDirection == -1)
            {
                coliderToFind.GetComponent<Enemy_movement>().Flip();
            }
        }

        //MoveTheObject
        if (IsPatroling)
        {
            rb.linearVelocity = direction * speed;
        }

    
        

        //SettingPatrolPoints
        if (Vector2.Distance(transform.position, target)< .1f)
        {
            StartCoroutine(SetPatrolPoint());
        }
    }

    
    IEnumerator SetPatrolPoint()
    {
        isPaused = true;

        yield return new WaitForSeconds(pauseDirection);

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        target = patrolPoints[currentPatrolIndex];
        isPaused = false;
    }
}
