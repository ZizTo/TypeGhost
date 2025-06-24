using UnityEngine;

public class ItemsInfo : MonoBehaviour
{
    public enum Rarity
    {
        COMMON,
        RARE,
        EPIC,
        ULTRAEPIC,
        LEGENDARY
    }
    public enum idEquiped
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

    public Sprite img;
    public Rarity rare;
    public int dmg;
    public int hp;
    public int pasdeff;
    public int deff;
    public int mana;
    public idEquiped[] canEquipOn;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = img;
    }
}
