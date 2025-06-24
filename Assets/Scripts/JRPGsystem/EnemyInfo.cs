using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class EnemyInfo : MonoBehaviour
{
    public string enemyName;
    public int lvl;

    public int starthp;
    public int startdmg;
    public int startpasdeff;
    public int startdeff;
    public int startmana;

    int maxhp;
    int maxdmg;
    int maxpasdeff;
    int maxdeff;
    int maxmana;

    [SerializeField] int hp;
    [SerializeField] int dmg;
    [SerializeField] int pasdeff;
    [SerializeField] int deff;
    [SerializeField] int mana;

    int xp;
    public int xpwhenDie = 100;

    GameObject[] equiped = new GameObject[7];
    public GameObject[] equipedPrefabs = new GameObject[7];
    public GameObject[] equipPlaceholder = new GameObject[7];

    bool watchLeft = false;
    public void ChangeWatchLeft(bool isWatchLeft)
    {
        if (watchLeft == isWatchLeft) { return; }
        watchLeft = isWatchLeft;
        if (watchLeft)
        {
            foreach (GameObject go in equiped)
            {
                if (go != null) go.GetComponent<SpriteRenderer>().flipX = true;
            }

            if (equiped[0] != null) equiped[0].GetComponent<SpriteRenderer>().sortingOrder = 3;
            if (equiped[1] != null) equiped[1].GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
        else
        {
            foreach (GameObject go in equiped)
            {
                if (go != null) go.GetComponent<SpriteRenderer>().flipX = false;
            }

            if (equiped[0] != null) equiped[0].GetComponent<SpriteRenderer>().sortingOrder = 1;
            if (equiped[1] != null) equiped[1].GetComponent<SpriteRenderer>().sortingOrder = 3;
        }
    }

    enum idEquiped { 
        WEAPONLEFT,
        WEAPONRIGHT,
        HEAD,
        CHEST,
        BOOTS,
        DOPLEFT,
        DOPRIGHT
    }


    bool isDead = false;

    public TextMeshPro nickText;
    public Image hpSprite;
    [Header("Statistic")]
    public bool isYou;
    public GameObject hpT, dmgT, pasdeffT, deffT, manaT;
    public void UpdateText()
    {
        if (isYou)
        {
            hpT.GetComponent<TextMeshProUGUI>().text = "HP: " + hp + "/" + maxhp;
            dmgT.GetComponent<TextMeshProUGUI>().text = "DMG: " + dmg + "/" + maxdmg;
            deffT.GetComponent<TextMeshProUGUI>().text = "DEF: " + deff + "/" + maxdeff;
            pasdeffT.GetComponent<TextMeshProUGUI>().text = "PASDEF: " + pasdeff + "/" + maxpasdeff;
            manaT.GetComponent<TextMeshProUGUI>().text = "MANA: " + mana + "/" + maxmana;
        }
    }

    void UpdateAllStats(bool needHp)
    {
        if (needHp || hp > maxhp)
            hp = maxhp;
        dmg = maxdmg;
        deff = maxdeff;
        pasdeff = maxpasdeff;
        mana = maxmana;
        hpSprite.fillAmount = (float)hp / (float)maxhp;
        UpdateText();
    }

    private void Start()
    {
        nickText.text = "lvl " + lvl + " | " + enemyName;
        InventoryChanged();
        UpdateAllStats(true);
    }

    public void getAttacked(int changeOn, bool ispasdef, bool isdef) {
        int damageDealt = changeOn;
        damageDealt -= ispasdef ? pasdeff : 0;
        damageDealt -= isdef ? deff : 0;
        hp -= damageDealt > 0 ? damageDealt : 0;
        hpSprite.fillAmount = (float)hp / (float)maxhp;
        isDead = (hp <= 0);
        UpdateText();
    }

    public bool IsDead()
    {
        return isDead;
    }

    public int GetDmg()
    {
        return dmg;
    }

    public int GetPasdef()
    {
        return pasdeff;
    }
    public int GetDef()
    {
        return deff;
    }


    public void ChangeInventory(GameObject itemPrefab, int idOnChange)
    {
        equipedPrefabs[idOnChange] = itemPrefab;
    }

    public void InventoryChanged()
    {
        for (int i = 0; i < equiped.Length; i++)
        {
            if (equiped[i] != null) Destroy(equiped[i]);
        }
        for (int i = 0; i < equiped.Length; i++)
        {
            if (equipedPrefabs[i] != null)
            {
                equiped[i] = Instantiate(equipedPrefabs[i], equipPlaceholder[i].transform);
                equiped[i].GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                equiped[i].GetComponent<SpriteRenderer>().sortingOrder = i == 0 ? 1 : 3;
            }
        }

        maxhp = starthp;
        maxdmg = startdmg;
        maxdeff = startdeff;
        maxpasdeff = startpasdeff;
        maxmana = startmana;

        //Equiped pick
        foreach(GameObject item in equipedPrefabs)
        {
            if (item == null) continue;
            maxhp += item.GetComponent<ItemsInfo>().hp;
            maxdmg += item.GetComponent<ItemsInfo>().dmg;
            maxdeff += item.GetComponent<ItemsInfo>().deff;
            maxpasdeff += item.GetComponent<ItemsInfo>().pasdeff;
            maxmana += item.GetComponent<ItemsInfo>().mana;
        }

        if (watchLeft)
        {
            watchLeft = false;
            ChangeWatchLeft(true);
        }
        UpdateAllStats(false);
    }

    public void ChangeXp(int xpkol)
    {
        xp += xpkol;
        int xpneedtoup = 50 * lvl;
        while (xp >= xpneedtoup)
        {
            //LVL up
            xp -= xpneedtoup;
            lvl++;
            xpneedtoup = 50 * lvl;
            starthp += 10;
            startdmg += 1;
            startdeff += 3;
            startpasdeff += 1;
            startmana += 1;
            InventoryChanged();
            UpdateAllStats(true);
        }

        nickText.text = "lvl " + lvl + " | " + enemyName;
    }

    

    
}
