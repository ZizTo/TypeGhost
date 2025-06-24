using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NewFight : MonoBehaviour
{
    public bool fightActive = false;

    public Camera walkCam, fightCam;

    GameObject enemy;
    public GameObject jrpgUI;
    public Button AttackButton, DeffendButton;


    float timer; float maxTime = 30f;
    public Image clockImage;
    int moveN = 0;

    public GameObject DamageEffect;
    enum moveStatus
    {
        NOTHING,
        PREPARE_ATTACK,
        PREPARE_DEFFEND,
        ATTACK,
        DEFFEND
    }
    bool IsStatusReady(moveStatus st)
    {
        return st == moveStatus.ATTACK || st == moveStatus.DEFFEND;
    }

    public GameObject attackPrefab, shieldPrefab;
    moveStatus uStatus, eStatus;
    Vector2 uNapr, eNapr;
    bool moveInProccess = false;

    Vector2 directionToEnemy()
    {
        return new Vector2(enemy.transform.position.x - transform.position.x, enemy.transform.position.y - transform.position.y).normalized;
    }

    Vector2 directionToMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        return new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y).normalized;
    }

    private void Start()
    {
        AttackButton.onClick.AddListener(AttackMoveButton);
        DeffendButton.onClick.AddListener(DeffendMoveButton);
    }

    public void FightStarting(GameObject en)
    {
        fightActive = true;
        gameObject.GetComponent<Movement>().isInFight = true;
        gameObject.GetComponent<AttackEnemy>().fightActive = true;

        walkCam.gameObject.SetActive(false);
        fightCam.gameObject.SetActive(true);

        enemy = en;

        jrpgUI.SetActive(true);


        nextMove();
    }

    void nextMove()
    {
        moveN++;
        timer = 0;
        uStatus = moveStatus.NOTHING;
        eStatus = moveStatus.NOTHING;
        fightActive = true;
        BotLogic();
    }

    void BotLogic()
    {
        eNapr = -directionToEnemy();
        eStatus = moveStatus.ATTACK;
    }

    public void AttackMoveButton()
    {
        if (!IsStatusReady(uStatus))
        {
            uStatus = moveStatus.PREPARE_ATTACK;
        }
    }

    public void DeffendMoveButton()
    {
        if (!IsStatusReady(uStatus))
        {
            uStatus = moveStatus.PREPARE_DEFFEND;
        }
    }

    GameObject uObj = null, eObj = null;

    void ProcceedMove()
    {
        fightActive = false;
        
        if (uStatus == moveStatus.DEFFEND)
        {
            uObj = Instantiate(shieldPrefab, transform);
            uObj.transform.right = uNapr;
        }
        if (eStatus == moveStatus.DEFFEND)
        {
            eObj = Instantiate(shieldPrefab, enemy.transform);
            eObj.transform.right = eNapr;
        }
        if (uStatus == moveStatus.ATTACK)
        {
            uObj = Instantiate(attackPrefab, transform);
            uObj.transform.right = uNapr;
            GetComponent<RBTests>().GiveForce(uNapr, 2 * GetComponent<EnemyInfo>().GetDmg());
            //enemy.GetComponent<EnemyInfo>().getAttacked(GetComponent<EnemyInfo>().GetDmg(), true, eDef);
        }
        if (eStatus == moveStatus.ATTACK)
        {
            eObj = Instantiate(attackPrefab, enemy.transform);
            eObj.transform.right = eNapr;
            enemy.GetComponent<RBTests>().GiveForce(eNapr, 2 * enemy.GetComponent<EnemyInfo>().GetDmg());
            //GetComponent<EnemyInfo>().getAttacked(enemy.GetComponent<EnemyInfo>().GetDmg(), true, uDef);
        }
        moveInProccess = true;

        
    }

    public void FightEnd()
    {
        if (!GetComponent<EnemyInfo>().IsDead() && enemy.GetComponent<EnemyInfo>().IsDead())
        {
            GetComponent<EnemyInfo>().ChangeXp(enemy.GetComponent<EnemyInfo>().xpwhenDie);
        }
        if (enemy.GetComponent<EnemyInfo>().IsDead())
        {
            Destroy(enemy);
        }
        fightActive = false;

        walkCam.gameObject.SetActive(true);
        fightCam.gameObject.SetActive(false);

        gameObject.GetComponent<Movement>().isInFight = false;
        gameObject.GetComponent<AttackEnemy>().fightActive = false;

        jrpgUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Instantiate(DamageEffect, transform);
        }
        if (fightCam.isActiveAndEnabled && enemy)
        {
            //------------------Cum logic---------------------------
            Vector2 rastVect = enemy.transform.position - transform.position;
            float rast = Mathf.Sqrt(rastVect.x * rastVect.x + rastVect.y * rastVect.y);
            Vector2 delta = new Vector2(transform.position.x, transform.position.y) + rastVect.normalized * rast / 2f;
            fightCam.transform.position = new Vector3(delta.x, delta.y, fightCam.transform.position.z);
            float camSize = rast;
            if (camSize < 2f) { camSize = 2f; }
            if (camSize > 15f) { camSize = 25f; }
            fightCam.GetComponent<Camera>().orthographicSize = camSize;
            //-------------------------------------------------------
        }

        if (fightActive)
        {
            //------------------Clock--------------------------------
            timer += Time.deltaTime;
            clockImage.fillAmount = 1 - (timer / maxTime);
            //-------------------------------------------------------

            //-----------------timer ends----------------------------
            if (timer >= maxTime && !IsStatusReady(uStatus))
            {
                uNapr = directionToEnemy();
                uStatus = moveStatus.DEFFEND;
            }
            if (timer >= maxTime && eStatus == moveStatus.NOTHING)
            {
                eNapr = -directionToEnemy();
                eStatus = moveStatus.DEFFEND;
            }
            //-------------------------------------------------------

            //------------------show napr----------------------------
            if (uStatus == moveStatus.PREPARE_ATTACK)
            {
                Vector2 napr = directionToMouse();
                Debug.DrawRay(transform.position, napr, Color.red);
                if (Input.GetMouseButtonDown(0))
                {
                    uNapr = napr;
                    uStatus = moveStatus.ATTACK;
                }
            }
            else if (uStatus == moveStatus.PREPARE_DEFFEND)
            {
                Vector2 napr = directionToMouse();
                Debug.DrawRay(transform.position, napr, Color.blue);
                if (Input.GetMouseButtonDown(0))
                {
                    uNapr = napr;
                    uStatus = moveStatus.DEFFEND;
                }
            }
            //------------------------------------------------------

            if (IsStatusReady(uStatus) && IsStatusReady(eStatus)) { ProcceedMove(); }

            if (Input.GetKeyDown(KeyCode.E))
            {
                FightEnd();
            }
        }

        if (moveInProccess)
        {

            if (uStatus == moveStatus.ATTACK)
            {
                List<Collider2D> hitColl = new List<Collider2D>();
                if (uObj) uObj.GetComponent<PolygonCollider2D>().Overlap(hitColl);

                foreach (Collider2D collider in hitColl)
                {
                    if ((collider.gameObject.CompareTag("Shield") || collider.gameObject.CompareTag("hitRange")) && ReferenceEquals(collider.transform.parent.gameObject, enemy))
                    {
                        enemy.GetComponent<EnemyInfo>().getAttacked(GetComponent<EnemyInfo>().GetDmg(), true, collider.gameObject.CompareTag("Shield"));
                        int damage = GetComponent<EnemyInfo>().GetDmg() - enemy.GetComponent<EnemyInfo>().GetPasdef();
                        if (collider.gameObject.CompareTag("Shield")) damage -= enemy.GetComponent<EnemyInfo>().GetDef();
                        if (damage > 0) { 
                            enemy.GetComponent<RBTests>().GiveForce(uNapr, damage);
                            Instantiate(DamageEffect, enemy.transform).transform.up = uNapr;
                        }
                        else if (damage < 0) { 
                            GetComponent<RBTests>().GiveForce(-uNapr, -damage);
                            Instantiate(DamageEffect, transform).transform.up = -uNapr;
                        }
                        Destroy(uObj);
                        break;
                    }
                }
            }
            if (eStatus == moveStatus.ATTACK)
            {
                List<Collider2D> hitColl = new List<Collider2D>();
                if (eObj) eObj.GetComponent<PolygonCollider2D>().Overlap(hitColl);

                foreach (Collider2D collider in hitColl)
                {
                    if ((collider.gameObject.CompareTag("Shield") || collider.gameObject.CompareTag("hitRange")) && ReferenceEquals(collider.transform.parent.gameObject, transform.gameObject))
                    {
                        if (collider.gameObject.CompareTag("Shield")) Debug.Log("enemy attacked your shield");
                        GetComponent<EnemyInfo>().getAttacked(enemy.GetComponent<EnemyInfo>().GetDmg(), true, collider.gameObject.CompareTag("Shield"));
                        int damage = enemy.GetComponent<EnemyInfo>().GetDmg() - GetComponent<EnemyInfo>().GetPasdef();
                        if (collider.gameObject.CompareTag("Shield")) damage -= GetComponent<EnemyInfo>().GetDef();
                        if (damage > 0) { GetComponent<RBTests>().GiveForce(eNapr, damage);
                            Instantiate(DamageEffect, transform).transform.up = eNapr;
                        }
                        else if (damage < 0) { enemy.GetComponent<RBTests>().GiveForce(-eNapr, -damage);
                            Instantiate(DamageEffect, enemy.transform).transform.up = -eNapr;
                        }
                        Destroy(eObj);
                        break;
                    }
                }
            }
            moveInProccess = !(GetComponent<Rigidbody2D>().linearVelocity == new Vector2(0f, 0f) && enemy.GetComponent<Rigidbody2D>().linearVelocity == new Vector2(0f, 0f));
            if (moveInProccess) { return; }

            if (uObj != null) Destroy(uObj);
            if (eObj != null) Destroy(eObj);

            if (GetComponent<EnemyInfo>().IsDead() || enemy.GetComponent<EnemyInfo>().IsDead())
            {
                FightEnd();
                return;
            }

            nextMove();
        }
    }

    
}
