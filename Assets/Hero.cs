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
    [SerializeField] GameObject GameOverUI;
    
    [SerializeField] Text timerText;
    [SerializeField] int Pistle_gold;
    [SerializeField] int AR_gold;
    [SerializeField] int SMG_gold;
    [SerializeField] int SR_gold;
    [SerializeField] public int _StageTimer = 30;
    [SerializeField] GameObject[] weaponList;

    [Header("プレイヤー設定")]
    [SerializeField] private int H_Gold;
    [SerializeField] public int H_hp;
    [SerializeField] private int H_speed;
    [SerializeField] float PistleInterval;
    [SerializeField] float ARInterval;
    [SerializeField] float SMGInterval;
    [SerializeField] float SRInterval;

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
    private float _Timer = 0;
    public int _RemainTime;

    private bool _HouseEnter;
    private Enemy _enemy;
    private Bullet _bullet;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        _rig = GetComponent<Rigidbody2D>();
        _enemy =FindAnyObjectByType<Enemy>();
        _bullet = FindAnyObjectByType<Bullet>();
        _PistleTimer =PistleInterval;
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
        GamesceneTimer();
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
            ShopUI.SetActive(true);
            ShopARUI.SetActive(true);
            ShopSMGUI.SetActive(true);
            ShopSRUI.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Alpha1) && Checkbuy(AR_gold, ref _ARcount))
            {
                DecreaseGold(AR_gold);
                Debug.Log($"{H_Gold}");
                Buyweapon(ARGameSceneUI);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && Checkbuy(SMG_gold, ref _SMGcount))
            {
                DecreaseGold(SMG_gold);
                Debug.Log($"{H_Gold}");
                Buyweapon(SMGGameSceneUI);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && Checkbuy(SR_gold, ref _SRcount))
            {
                DecreaseGold(SR_gold);
                Debug.Log($"{H_Gold}");
                Buyweapon(SRGameSceneUI);
            }
        } else if (!_HouseEnter)
        {
            ShopUI.SetActive(false);
            ShopARUI.SetActive(false);
            ShopSMGUI.SetActive(false);
            ShopSRUI.SetActive(false);
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
        if (H_hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnDestroy()
    {
        GameOverUI.SetActive(true);
        Time.timeScale = 0;
    }
    private void GamesceneTimer()
    {
        _Timer += Time.deltaTime;
        _RemainTime = _StageTimer - (int)_Timer;
        timerText.text = "残り時間 " + _RemainTime.ToString("D2") + " 秒";
        if (_RemainTime <= 0)
        {
            _HouseEnter = true;
            _StageTimer = 0;
            _Timer = 0;
            Time.timeScale = 0;
            timerText.text = "Sを押してスタート";
            if (Input.GetKeyDown(KeyCode.S))
            {
                _HouseEnter = false;
                _StageTimer = 3;
                Time.timeScale = 1;
                return;
            }
        }
    }
    /// <summary>
    /// 弾を撃つ時に呼ばれるメソッド
    /// </summary>
    public void bulletshot()
    {
        if (_PlayerIndex == 0 && _PistleTimer < Time.time)//delta.timeを足し続けるよりもtimeで必要な時だけ呼び出した方が軽い
        {
            Instantiate(PistleBullet,muzzle.transform.position,Quaternion.identity);
            PistleBullet.SetActive(true);
            _PistleTimer = Time.time + PistleInterval;//if文が呼ばれたら、timeにインターバルを足してピストルタイマーに代入
            　　　　　　　　　　　　　　　　　　　　　//一時的にtimeよりもタイマーの方がインターバル時間分大きくなるため、ちゃんと機能する
        }
        if (_PlayerIndex == 1 && _ARTimer < Time.time && _ARcount > 0)
        {
            Instantiate(ARBullet, muzzle.transform.position, Quaternion.identity);
            ARBullet.SetActive(true);
            _ARTimer = Time.time + ARInterval;
        }
        if (_PlayerIndex == 2 && _SMGTimer < Time.time && _SMGcount > 0)
        {
            Instantiate(SMGBullet, muzzle.transform.position, Quaternion.identity);
            SMGBullet.SetActive(true);
            _SMGTimer = Time.time + SMGInterval;
        }
        if (_PlayerIndex == 3 && _SRTimer < Time.time && _SRcount > 0)
        {
            Instantiate(SRBullet, muzzle.transform.position, Quaternion.identity);
            SRBullet.SetActive(true);
            _SRTimer = Time.time + SRInterval;
        }
    }
    /// <summary>
    /// 買えるかどうかの判別をするためのメソッド
    /// </summary>
    /// <param name="gold"></param>
    /// <param name="keycount">買うためのキーが押された回数</param>
    /// <returns></returns>
    private bool Checkbuy(int gold, ref int keycount)
    {
        if (H_Gold > gold && keycount == 0)
        {
            keycount++;
            return true;
        }
        else if(H_Gold < gold)
        {
            GoldWarningUI.SetActive(true);
            StartCoroutine(UIfadeout());
            return false;
        }
        else if (keycount > 0)
        {
            DupilicationWarningUI.SetActive(true);
            StartCoroutine(UIfadeout());
            return false;
        }
            return false;
    }
    /// <summary>
    /// goldを増やすためのメソッド
    /// </summary>
    /// <param name="gold"></param>
    public void AddGold(int gold)
    {
        H_Gold += gold;
    }
    /// <summary>
    /// goldを減らすためのメソッド
    /// </summary>
    /// <param name="gold"></param>
    public void DecreaseGold(int gold)
    {
        H_Gold -= gold;
        if (H_Gold <0)
        {
            H_Gold = 0;
        }
    }
    /// <summary>
    /// 武器が買えた時に呼び出されるやつ
    /// </summary>
    /// <param name="weapon"></param>
    public void Buyweapon(GameObject weapon)
    {
        weapon.SetActive(true);
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
        yield return new WaitForSeconds(3);
        GoldWarningUI.SetActive(false);
        DupilicationWarningUI.SetActive(false);
        NoWeaponWarningUI.SetActive(false);
    }
}
