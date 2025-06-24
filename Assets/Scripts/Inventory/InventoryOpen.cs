using UnityEngine;

public class InventoryOpen : MonoBehaviour
{
    public GameObject inventoryAtAll, leftUp, rightUp;
    Vector2 leftUpf, leftUps, rightUpf, rightUps;

    /*bool open, needMove;
    void Start()
    {
        leftUpf = leftUp.GetComponent<RectTransform>().position;
        leftUps = leftUp.GetComponent<RectTransform>().position - new Vector3(leftUp.GetComponent<RectTransform>().rect.width + 100f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            open = !open;
            needMove = true;
            if (open)
            {
                inventoryAtAll.SetActive(true);
            }
        }
        if (!needMove) { return; }
        Vector2 nextPos = open ? leftUpf : leftUps;

        float velocity = 8f;
        float rast = Mathf.Sqrt((leftUp.GetComponent<RectTransform>().position.x - nextPos.x) * (leftUp.GetComponent<RectTransform>().position.x - nextPos.x) + (leftUp.GetComponent<RectTransform>().position.y - nextPos.y) * (leftUp.GetComponent<RectTransform>().position.y - nextPos.y));
        leftUp.GetComponent<RectTransform>().position = Vector2.MoveTowards(leftUp.GetComponent<RectTransform>().position, nextPos, Time.deltaTime * velocity * rast);

        if (rast < 1f)
        {
            needMove = false;
            if (!open) { inventoryAtAll.SetActive(false); }
        }
    }*/
    bool open = false;
    private void Start()
    {
        inventoryAtAll.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            open = !open;
            if (open) inventoryAtAll.SetActive(true);
            else inventoryAtAll.SetActive(false);
        }
    }
}
