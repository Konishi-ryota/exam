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
    [SerializeField] Text timerText;
    [SerializeField] int Pistle_gold;
    [SerializeField] int AR_gold;
    [SerializeField] int SMG_gold;
    [SerializeField] int SR_gold;
    [SerializeField] int _StageTimer = 30;
    [SerializeField] GameObject[] weaponList;
   
    [Header("HeroSettings")]
    [SerializeField] private int H_Gold;
    [SerializeField] private int H_hp;
    [SerializeField] private int H_speed;
    [SerializeField] float PistlInterval = 3f;
    [SerializeField] float ARInterval = 2f;
    [SerializeField] float SMGInterval = 0.5f;
    [SerializeField] float SRInterval = 5f;

    private int _ARcount = 0;
    private int _SMGcount = 0;
    private int _SRcount = 0;
    public int _PlayerIndex = 0;
    private Rigidbody2D _rig = null;
    private int _Level;
    private int _HeroExp;
    private float _PistleTimer;
    private float _ARTimer;  
    private float _SMGTimer;
    private float _SRTimer;
    private float _timer = 0;
    private bool _HouseEnter;
    private Enemy _enemy;
    private Bullet _bullet;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        _rig = GetComponent<Rigidbody2D>();
        _enemy =FindObjectOfType<Enemy>();
        _bullet = FindObjectOfType<Bullet>();
        _PistleTimer =PistlInterval;
        _ARTimer = ARInterval;
        _SMGTimer = SMGInterval;
        _SMGTimer = SRInterval;
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

        _timer += Time.deltaTime;
        int remaining = _StageTimer - (int)_timer;
        timerText.text = remaining.ToString("D2");
        if (Input.GetKeyDown(KeyCode.K) && Time.timeScale > 0)
        {
            _PlayerIndex = (_PlayerIndex + 1) % weaponList.Length;
            Debug.Log($"{_PlayerIndex}");
            WeaponController();
        }
        if (Input.GetKeyDown(KeyCode.L) && Time.timeScale > 0)
        {
            _PlayerIndex = (_PlayerIndex - 1 + weaponList.Length) % weaponList.Length;
            Debug.Log($"{_PlayerIndex}");
            WeaponController();
        }
        if (Input.GetKey(KeyCode.Space) && Time.timeScale > 0) 
        {
            bulletshot();
        }
        if (_HouseEnter)
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
        if (_HeroExp > 100)
        {
            _HeroExp = _HeroExp - 100;
           _Level =_Level + 1;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Respawn")
        {
            _HouseEnter = true;
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
            _HouseEnter = false;
            ShopUI.SetActive(false);
            ShopARUI.SetActive(false);
            ShopSMGUI.SetActive(false);
            ShopSRUI.SetActive(false);
        }
    }
    public void bulletshot()
    {
        if (_PlayerIndex == 0 && PistlInterval < _PistleTimer)
        {
            Instantiate(PistleBullet,muzzle.transform.position,Quaternion.identity);
            PistleBullet.SetActive(true);
            _PistleTimer = 0;
        }
        if (_PlayerIndex == 1 && ARInterval < _ARTimer && _ARcount > 0)
        {
            Instantiate(ARBullet, muzzle.transform.position, Quaternion.identity);
            ARBullet.SetActive(true);
            _ARTimer = 0;
        }
        if (_PlayerIndex == 2 && SMGInterval < _SMGTimer && _SMGcount > 0)
        {
            Instantiate(SMGBullet, muzzle.transform.position, Quaternion.identity);
            SMGBullet.SetActive(true);
            _SMGTimer = 0;
        }
        if (_PlayerIndex == 3 && SRInterval < _SRTimer && _SRcount > 0)
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
        if (_PlayerIndex == 0)
        {
            PistleselectUI.SetActive(true);
            ARselectUI.SetActive(false);
            SMGselectUI.SetActive(false);
            SRSelectUI.SetActive(false);
        }
        if (_PlayerIndex == 1)
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
        if (_PlayerIndex == 2)
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
        if (_PlayerIndex == 3)
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
