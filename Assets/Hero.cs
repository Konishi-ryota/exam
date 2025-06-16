using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class Hero : MonoBehaviour
{
    #region　変数
    public int AttackPower => H_attackPower;

    [SerializeField] GameObject PistleBullet;
    [SerializeField] GameObject ARBullet;
    [SerializeField] GameObject SMGBullet;
    [SerializeField] GameObject SRBullet;
    [SerializeField] GameObject muzzle;
    [SerializeField] GameObject ShopUI;
    [SerializeField] GameObject ShopARUI;
    [SerializeField] GameObject ShopSMGUI;
    [SerializeField] GameObject ShopSRUI;
    [SerializeField] GameObject GoldWarningUI;
    [SerializeField] GameObject DupilicationWarningUI;
    [SerializeField] GameObject NoWeaponWarningUI;
    [SerializeField] GameObject PauseUI;
    [SerializeField] GameObject PistleGameSceneUI;
    [SerializeField] GameObject ARGameSceneUI;
    [SerializeField] GameObject SMGGameSceneUI;
    [SerializeField] GameObject SRGameSceneUI;
    [SerializeField] GameObject PistleselectUI;
    [SerializeField] GameObject ARselectUI;
    [SerializeField] GameObject SMGselectUI;
    [SerializeField] GameObject SRSelectUI;
    [SerializeField] int Pistle_gold;
    [SerializeField] int AR_gold;
    [SerializeField] int SMG_gold;
    [SerializeField] int SR_gold;
    [SerializeField] GameObject[] weaponList;
   
    [Header("HeroSettings")]
    [SerializeField] private int H_Gold;
    [SerializeField] private int H_hp;
    [SerializeField] private int H_speed;
    [SerializeField] private int H_attackPower = 3;
    [SerializeField] float _PistlInterval = 3f;
    [SerializeField] float _ARInterval = 2f;
    [SerializeField] float _SMGInterval = 0.5f;
    [SerializeField] float _SRInterval = 5f;

    //[SerializeField] private float interval = 1f;

    private int _ARcount = 0;
    private int _SMGcount = 0;
    private int _SRcount = 0;
    public int playerIndex = 0;
    private Rigidbody2D _rig = null;
    private int Level;
    private int H_exp;
    private float _PistleTimer;
    private float _ARTimer;  
    private float _SMGTimer;
    private float _SRTimer;
    private bool HouseEnter;
    private Enemy _enemy;
    private Bullet _bullet;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        _rig = GetComponent<Rigidbody2D>();
        _enemy =FindObjectOfType<Enemy>();
        _bullet = FindObjectOfType<Bullet>();
        _PistleTimer =_PistlInterval;
        _ARTimer = _ARInterval;
        _SMGTimer = _SMGInterval;
        _SMGTimer = _SRInterval;
        //H_exp = enemy.E_exp;
        //H_Gold = enemy.E_Gold;
        
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            _rig.velocity = new Vector2(Input.GetAxis("Horizontal"), 0) * H_speed;
        }
    }
    // Update is called once per frame
    void Update()
    {
        _PistleTimer += Time.deltaTime;
        _ARTimer += Time.deltaTime;
        _SMGTimer += Time.deltaTime;
        _SRTimer += Time.deltaTime;
        Debug.Log($"{_PistleTimer}");
        if (Input.GetKeyDown(KeyCode.K) && Time.timeScale > 0)
        {
            playerIndex = (playerIndex + 1) % weaponList.Length;
            Debug.Log($"{playerIndex}");
            WeaponController();
        }
        if (Input.GetKeyDown(KeyCode.L) && Time.timeScale > 0)
        {
            playerIndex = (playerIndex - 1 + weaponList.Length) % weaponList.Length;
            Debug.Log($"{playerIndex}");
            WeaponController();
        }
        if (Input.GetKey(KeyCode.Space) && /*_bullet.interval < _timer &&*/ Time.timeScale > 0) 
        {
            bulletshot();
        }
        if (HouseEnter)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && Checkbuy(AR_gold, ref _ARcount))
            {
                    DecreaseGold(AR_gold);
                    Debug.Log($"{H_Gold}");
                    Buyweapon(ARGameSceneUI);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && Checkbuy(SMG_gold,ref _SMGcount))
            {
                    DecreaseGold(SMG_gold);
                    Debug.Log($"{H_Gold}");
                    Buyweapon(SMGGameSceneUI);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && Checkbuy(SR_gold,ref _SRcount))
            {
                    DecreaseGold(SR_gold);
                    Debug.Log($"{H_Gold}");
                    Buyweapon(SRGameSceneUI);
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseUI.SetActive(true);
            Time.timeScale = 0;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            PauseUI.SetActive(false);
            Time.timeScale = 1;
        }   
        if (H_exp > 100)
        {
            H_exp = H_exp - 100;
            Level = Level + 1;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Respawn")
        {
            HouseEnter = true;
            ShopUI.SetActive(true);
            ShopARUI.SetActive(true);
            ShopSMGUI.SetActive(true);
            ShopSRUI.SetActive(true);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Respawn")
        {
            HouseEnter = false;
            ShopUI.SetActive(false);
            ShopARUI.SetActive(false);
            ShopSMGUI.SetActive(false);
            ShopSRUI.SetActive(false);
        }
    }
    public void bulletshot()
    {
        if (playerIndex == 0 && _PistlInterval < _PistleTimer)
        {
            Instantiate(PistleBullet,muzzle.transform.position,Quaternion.identity);
            PistleBullet.SetActive(true);
            _PistleTimer = 0;
        }
        if (playerIndex == 1 && _ARInterval < _ARTimer && _ARcount > 0)
        {
            Instantiate(ARBullet, muzzle.transform.position, Quaternion.identity);
            ARBullet.SetActive(true);
            _ARTimer = 0;
        }
        if (playerIndex == 2 && _SMGInterval < _SMGTimer && _SMGcount > 0)
        {
            Instantiate(SMGBullet, muzzle.transform.position, Quaternion.identity);
            SMGBullet.SetActive(true);
            _SMGTimer = 0;
        }
        if (playerIndex == 3 && _SRInterval < _SRTimer && _SRcount > 0)
        {
            Instantiate(SRBullet, muzzle.transform.position, Quaternion.identity);
            SRBullet.SetActive(true);
            _SRTimer = 0;
        }
    }
    private bool Checkbuy(int gold, ref int keycount)
    {
        if (H_Gold > gold && keycount == 0)
        {
            Debug.Log("購入可能");
            keycount++;
            return true;
        }
        else if(H_Gold < gold)
        {
            Debug.Log("購入不可");
            GoldWarningUI.SetActive(true);
            StartCoroutine(UIfadeout());
            return false;
        }
        else if (keycount > 0)
        {
            Debug.Log("購入不可");
            DupilicationWarningUI.SetActive(true);
            StartCoroutine(UIfadeout());
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
    }
    public void WeaponController()
    {
        if (playerIndex == 0)
        {
            PistleselectUI.SetActive(true);
            ARselectUI.SetActive(false);
            SMGselectUI.SetActive(false);
            SRSelectUI.SetActive(false);
        }
        if (playerIndex == 1)
        {
            PistleselectUI.SetActive(false);
            ARselectUI.SetActive(true);
            SMGselectUI.SetActive(false);
            SRSelectUI.SetActive(false);
            if (_ARcount == 0)
            {
                NoWeaponWarningUI.SetActive(true);
                StartCoroutine(UIfadeout());
            }
        }
        if (playerIndex == 2)
        {
            PistleselectUI.SetActive(false);
            SMGselectUI.SetActive(true);
            ARselectUI.SetActive(false);
            SRSelectUI.SetActive(false);
            if (_SMGcount == 0)
            {
                NoWeaponWarningUI.SetActive(true);
                StartCoroutine(UIfadeout());
            }
        }
        if (playerIndex == 3)
        {
            PistleselectUI.SetActive(false);
            SRSelectUI.SetActive(true);
            ARselectUI.SetActive(false);
            SMGselectUI.SetActive(false);
            if (_SRcount == 0)
            {
                NoWeaponWarningUI.SetActive(true);
                StartCoroutine(UIfadeout());
            }
        }
    }
    IEnumerator UIfadeout()
    {
        Debug.Log("UI非表示開始");
        yield return new WaitForSeconds(3);
        GoldWarningUI.SetActive(false);
        DupilicationWarningUI.SetActive(false);
        NoWeaponWarningUI.SetActive(false);
    }
}
