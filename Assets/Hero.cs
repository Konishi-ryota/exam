using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class Hero : MonoBehaviour
{
    #region　変数
    public int AttackPower => H_attackPower;

    [SerializeField] GameObject bullets;
    [SerializeField] GameObject muzzle;
    [SerializeField] GameObject Pistle;
    [SerializeField] GameObject AR;
    [SerializeField] GameObject SMG;
    [SerializeField] GameObject SR;
    [SerializeField] GameObject goldWarningUI;
    [SerializeField] GameObject DuplicationWarningUI;
    [SerializeField] GameObject PauseUI;
    [SerializeField] GameObject PistleGameSceneUI;
    [SerializeField] GameObject ARGameSceneUI;
    [SerializeField] GameObject SMGGameSceneUI;
    [SerializeField] GameObject SRGameSceneUI;
    [SerializeField] int Pistle_gold;
    [SerializeField] int AR_gold;
    [SerializeField] int SMG_gold;
    [SerializeField] int SR_gold;
    private int Level;
    private int H_exp;
    private float _timer;
    private bool HouseEnter;
    [SerializeField] List<GameObject> weaponList = new();
    private int playerIndex;
    private Rigidbody2D _rig = null;
    //[SerializeField] float MaxChargetime;
    //[SerializeField] float MidChargetime;
    //[SerializeField] float IncreaseChargetime;
    [Header("HeroSettings")]
    [SerializeField] private int H_Gold;
    [SerializeField] private int H_hp;
    [SerializeField] private int H_speed;
    [SerializeField] private int H_attackPower = 3;
    [SerializeField] private float interval = 1f;
    private float leftHorizontalAxis;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        _rig = GetComponent<Rigidbody2D>();
        Enemy enemy =FindObjectOfType<Enemy>();
        _timer = interval;
        //H_exp = enemy.E_exp;
        //H_Gold = enemy.E_Gold;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            _rig.velocity = new Vector2(Input.GetAxis("Horizontal"), 0) * H_speed;

        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            playerIndex = (playerIndex - 1 + weaponList.Count) % weaponList.Count;
            Debug.Log($"{playerIndex}");
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            playerIndex = (playerIndex + 1) % weaponList.Count;
            Debug.Log($"{playerIndex}");
        }
        if (Input.GetKeyDown(KeyCode.Space) && interval < _timer)
        {
            _timer = 0;
            Instantiate(bullets,muzzle.transform.position,Quaternion.identity);
            bullets.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseUI.SetActive(true);
            Time.timeScale = 0;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            PauseUI.SetActive(false);
        }
        //if (Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.LeftShift))
        //{
        //    if (IncreaseChargetime <= MaxChargetime)
        //    {
        //        IncreaseChargetime += Time.deltaTime;
        //    }
        //}
        //else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.LeftShift))
        //{
        //    if (IncreaseChargetime >= MaxChargetime)
        //    {
                
        //    }
        //    if (IncreaseChargetime < MaxChargetime && IncreaseChargetime >= MidChargetime)
        //    {

        //    }
        //    if (IncreaseChargetime < MidChargetime)
        //    {

        //    }
        //}
        if (H_exp > 100)
        {
            H_exp = H_exp - 100;
            Level = Level + 1;
        }
        if (HouseEnter)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("1キーが押された");
                if (Checkgold(Pistle_gold,PistleGameSceneUI))
                {
                    DecreaseGold(Pistle_gold);
                    Debug.Log($"{H_Gold}");
                    Buyweapon(PistleGameSceneUI);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (Checkgold(AR_gold,ARGameSceneUI))
                {
                    DecreaseGold(AR_gold);
                    Debug.Log($"{H_Gold}");
                    Buyweapon(ARGameSceneUI);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (Checkgold(SMG_gold,SMGGameSceneUI))
                {
                    DecreaseGold(SMG_gold);
                    Debug.Log($"{H_Gold}");
                    Buyweapon(SMGGameSceneUI);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (Checkgold(SR_gold,SRGameSceneUI))
                {
                    Buyweapon(SRGameSceneUI);
                    DecreaseGold(SR_gold);
                    Debug.Log($"{H_Gold}");
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Respawn")
        {
            HouseEnter = true;
            Pistle.SetActive(true);
            AR.SetActive(true);
            SMG.SetActive(true);
            SR.SetActive(true);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Respawn")
        {
            HouseEnter = false;
            Pistle.SetActive(false);
            AR.SetActive(false);
            SMG.SetActive(false);
            SR.SetActive(false);
        }
    }
    private bool Checkgold(int gold,GameObject weapon)
    {
        if (H_Gold > gold &&!weaponList.Contains(weapon))
        {
            Debug.Log("購入可能");
            weaponList.Add(weapon);
            return true;
        }
        else if (H_Gold < gold)
        {
        Debug.Log("購入不可");
        goldWarningUI.SetActive(true);
        StartCoroutine(UIfalse());
        return false;
        }
        else if (weaponList.Contains(weapon))
        {
        Debug.Log("購入不可");
        DuplicationWarningUI.SetActive(true);
        StartCoroutine(UIfalse());
        return false;
        }
        return false;
    }

    public void AddGold(int value)
    {
        H_Gold += value;
    }

    public void DecreaseGold(int value)
    {
        H_Gold -= value;
        if (H_Gold <0)
        {
            H_Gold = 0;
            Debug.Log("お金がありません");
        }
    }
    public void Buyweapon(GameObject value)
    {
        value.gameObject.SetActive(true);
        //if (!weaponList.Contains(value))
        //{
        //    weaponList.Add(value);
        //}
    }
    IEnumerator UIfalse()
    {
        Debug.Log("UI非表示開始");
        yield return new WaitForSeconds(3);
        goldWarningUI.SetActive(false);
        DuplicationWarningUI.SetActive(false);
    }
}
