using UnityEngine;
using UnityEngine.UI;

public class FightPreparing : MonoBehaviour
{
    public bool fightActive = false;

    public Camera walkCam, fightCam;
    public float otdol = 1.24f;
    Vector2 ePosF;

    GameObject enemy;
    public GameObject jrpgUI;
    public Button AttackButton, DeffendButton;

    private void Start()
    {
        AttackButton.onClick.AddListener(AttackMoveButton);
        DeffendButton.onClick.AddListener(DeffendMoveButton);
    }

    public void FightStarting(GameObject en, Vector2 vec)
    {
        fightActive = true;
        ePosF = vec;
        gameObject.GetComponent<Movement>().isInFight = true;

        walkCam.gameObject.SetActive(false);
        fightCam.gameObject.SetActive(true);

        enemy = en;

        enemy.transform.position = new Vector2(fightCam.transform.position.x - otdol, fightCam.transform.position.y);

        GetComponent<SpriteRenderer>().flipX = true;
        GetComponent<EnemyInfo>().ChangeWatchLeft(true);

        jrpgUI.SetActive(true);

        nextMove();
    }

    float timer; float maxTime = 30f;
    public Image clockImage;
    int moveN = 0;
    enum moveStatus {
        NOTHING,
        ATTACK,
        DEFFEND
    }

    moveStatus uStatus, eStatus;
    void nextMove()
    {
        moveN++;
        timer = 0;
        uStatus = moveStatus.NOTHING;
        eStatus = moveStatus.NOTHING;
        BotLogic();
    }

    void BotLogic()
    {
        eStatus = moveStatus.ATTACK;
    }

    public void AttackMoveButton()
    {
        if (uStatus == moveStatus.NOTHING)
        {
            uStatus = moveStatus.ATTACK;
        }
    }

    public void DeffendMoveButton()
    {
        if (uStatus == moveStatus.NOTHING)
        {
            uStatus = moveStatus.DEFFEND;
        }
    }

    void ProcceedMove()
    {
        bool uDef = false, eDef = false;
        if(uStatus == moveStatus.DEFFEND)
        {
            uDef = true;
        }
        if (eStatus == moveStatus.DEFFEND)
        {
            eDef = true;
        }
        if (uStatus == moveStatus.ATTACK)
        {
            enemy.GetComponent<EnemyInfo>().getAttacked(GetComponent<EnemyInfo>().GetDmg(), true, eDef);
        }
        if (eStatus == moveStatus.ATTACK)
        {
            GetComponent<EnemyInfo>().getAttacked(enemy.GetComponent<EnemyInfo>().GetDmg(), true, uDef);
        }
        if (GetComponent<EnemyInfo>().IsDead() || enemy.GetComponent<EnemyInfo>().IsDead()) { 
            FightEnd(); 
            return; 
        }
        
        nextMove();
    }

    public void FightEnd()
    {
        if(!GetComponent<EnemyInfo>().IsDead() && enemy.GetComponent<EnemyInfo>().IsDead())
        {
            GetComponent<EnemyInfo>().ChangeXp(enemy.GetComponent<EnemyInfo>().xpwhenDie);
        }
        if(enemy.GetComponent<EnemyInfo>().IsDead())
        {
            Destroy(enemy);
        }
        fightActive = false;
        enemy.transform.position = ePosF;

        walkCam.gameObject.SetActive(true);
        fightCam.gameObject.SetActive(false);

        gameObject.GetComponent<Movement>().isInFight = false;

        jrpgUI.SetActive(false);
    }

    void Update()
    {
        if (!fightActive) { return; }
        if (eStatus == moveStatus.NOTHING || uStatus == moveStatus.NOTHING) timer += Time.deltaTime;
        clockImage.fillAmount = 1 - (timer / maxTime);

        if (timer >= maxTime && uStatus == moveStatus.NOTHING)
        {
            uStatus = moveStatus.DEFFEND;
        }
        if (timer >= maxTime && eStatus == moveStatus.NOTHING)
        {
            eStatus = moveStatus.DEFFEND;
        }
        if (uStatus != moveStatus.NOTHING && eStatus != moveStatus.NOTHING) { ProcceedMove(); }

        if (Input.GetKeyDown(KeyCode.E))
        {
            FightEnd();
        }
    }
}
