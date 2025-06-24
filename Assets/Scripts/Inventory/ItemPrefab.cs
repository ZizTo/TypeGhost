using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ItemPrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public GameObject itemPrefab;
    public GameObject placeholderPrefab;
    public GameObject imgView, rarityView;
    GameObject placholderFound;

    public enum idOfPlaceholder
    {
        WEAPONLEFT,
        WEAPONRIGHT,
        HEAD,
        CHEST,
        BOOTS,
        DOPLEFT,
        DOPRIGHT,
        INVENTORY
    }
    public idOfPlaceholder itemOn;

    bool isOnMouse = false, isDragged = false, therePlaceholder = false, placed = false;
    Vector2 startPos, placeholderPos = new Vector2(0f,0f);

    bool needMoving = false;

    private void Start()
    {
        startPos = GetComponent<RectTransform>().position;
        imgView.GetComponent<Image>().sprite = itemPrefab.GetComponent<ItemsInfo>().img;
        switch(itemPrefab.GetComponent<ItemsInfo>().rare)
        {
            case ItemsInfo.Rarity.COMMON:
                rarityView.GetComponent<Image>().color = new Color(255/255, 255/255, 255/255, 0.9f);
                break;
            case ItemsInfo.Rarity.RARE:
                rarityView.GetComponent<Image>().color = new Color(38f / 255f, 162f / 255f, 255 / 255, 0.9f);
                break;
            case ItemsInfo.Rarity.EPIC:
                rarityView.GetComponent<Image>().color = new Color(143f / 255f, 37f / 255f, 255 / 255, 0.9f);
                break;
            case ItemsInfo.Rarity.ULTRAEPIC:
                rarityView.GetComponent<Image>().color = new Color(255 / 255, 37f / 255f, 40f / 255f, 0.9f);
                break;
            case ItemsInfo.Rarity.LEGENDARY:
                rarityView.GetComponent<Image>().color = new Color(255 / 255, 250f / 255f, 37f / 255f, 0.9f);
                break;
            default:
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isOnMouse = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOnMouse = false;
    }

    GameObject startPlaceholder;
    bool comeBack = true;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isOnMouse)
        {
            if (startPlaceholder == null)
            {
                startPos = GetComponent<RectTransform>().position;
                startPlaceholder = Instantiate(placeholderPrefab, transform.parent);
                startPlaceholder.GetComponent<RectTransform>().position = startPos;
                startPlaceholder.GetComponent<PlaceholderInfo>().placholderIs = (PlaceholderInfo.idOfPlaceholder)(int)itemOn;
                transform.SetAsLastSibling();
            }
            isDragged = true;
            needMoving = true;
            comeBack = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragged = false;
        if (therePlaceholder)
        {
            placed = true;
            comeBack = false;
        }
    }

    private void Update()
    { 
        if (!needMoving) { return; }
        Vector2 nextPos;
        if (placed)
        {
            nextPos = placeholderPos;
        }
        else if (isDragged)
        {
            therePlaceholder = false;
            nextPos = Input.mousePosition;
            Collider2D[] colliders = Physics2D.OverlapPointAll(Input.mousePosition);
            foreach (Collider2D col in colliders)
            {
                if (col.gameObject.tag == "placeholder" && Array.IndexOf(itemPrefab.GetComponent<ItemsInfo>().canEquipOn, (ItemsInfo.idEquiped)(int)col.GetComponent<PlaceholderInfo>().placholderIs) != -1)
                {
                    therePlaceholder = true;
                    placeholderPos = col.transform.position;
                    placholderFound = col.gameObject;
                }
            }
            if (therePlaceholder) nextPos = placeholderPos;
        }
        else
        {
            nextPos = startPos;
        }

        needMoving = isDragged || Mathf.Sqrt((GetComponent<RectTransform>().position.x - nextPos.x) * (GetComponent<RectTransform>().position.x - nextPos.x)
                + (GetComponent<RectTransform>().position.y - nextPos.y) * (GetComponent<RectTransform>().position.y - nextPos.y)) > 1f;

        float velocity = 10f;

        float rast = Mathf.Sqrt((GetComponent<RectTransform>().position.x - nextPos.x) * (GetComponent<RectTransform>().position.x - nextPos.x) + (GetComponent<RectTransform>().position.y - nextPos.y) * (GetComponent<RectTransform>().position.y - nextPos.y));
        GetComponent<RectTransform>().position = Vector2.MoveTowards(GetComponent<RectTransform>().position, nextPos, Time.deltaTime * velocity * rast);

        if (!needMoving && comeBack)
        {
            Destroy(startPlaceholder);
            GetComponent<RectTransform>().position = startPos;
        }
        if (!needMoving && placed)
        {
            startPos = placeholderPos;
            itemOn = (idOfPlaceholder)(int)placholderFound.GetComponent<PlaceholderInfo>().placholderIs;

            if (startPlaceholder.GetComponent<PlaceholderInfo>().placholderIs != PlaceholderInfo.idOfPlaceholder.INVENTORY)
            {
                GetComponentInParent<EnemyInfo>().ChangeInventory(null, (int)startPlaceholder.GetComponent<PlaceholderInfo>().placholderIs);
            }
            if (itemOn != idOfPlaceholder.INVENTORY)
            {
                GetComponentInParent<EnemyInfo>().ChangeInventory(itemPrefab, (int)itemOn);
            }
            GetComponentInParent<EnemyInfo>().InventoryChanged();
            startPlaceholder = null;
            placed = false;
            Destroy(placholderFound);
        }
    }
}
