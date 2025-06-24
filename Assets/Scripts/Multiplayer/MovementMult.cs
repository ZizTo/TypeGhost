using UnityEngine;
using Unity.Netcode;

public class MovementMult : NetworkBehaviour
{
    public float speed = 5f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        transform.position = new Vector2(2f, 0f);
        Camera camera = GetComponentInChildren<Camera>();

        if(!IsOwner)
        {
            camera.enabled = false;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsOwner) { return; }


        if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector2(1f, 0f) * Time.deltaTime * speed);
            GetComponent<SpriteRenderer>().flipX = false;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector2(-1f, 0f) * Time.deltaTime * speed);
            GetComponent<SpriteRenderer>().flipX = true;
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector2(0f, 1f) * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector2(0f, -1f) * Time.deltaTime * speed);
        }
    }
}
