using UnityEngine;

public class RBTests : MonoBehaviour
{
    public float x = 10f, y = 10f;
    public float RbForce = 5f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Pushed");
            GiveForce(x, y, 1);
        }
    }

    public void GiveForce(float xn, float yn, int dmg) {
        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0f, 0f);
        float dmgn = RbForce * Mathf.Log(dmg, 1.3f);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(xn * dmgn, yn * dmgn), ForceMode2D.Impulse);
    }
    public void GiveForce(Vector2 napr, int dmg)
    {
        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0f, 0f);
        float dmgn = RbForce * Mathf.Log(dmg, 1.3f);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(napr.x * dmgn, napr.y * dmgn), ForceMode2D.Impulse);
    }
}
