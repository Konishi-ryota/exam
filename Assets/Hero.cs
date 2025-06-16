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
    [SerializeField] GameObject Pistle;
    [SerializeField] GameObject AR;
    [SerializeField] GameObject SMG;
    [SerializeField] GameObject SR;
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
    //[SerializeField] private float interval = 1f;

    private int _pistlecount = 0;
    private int _ARcount = 0;
    private int _SMGcount = 0;
    private int _SRcount = 0;
    public int playerIndex;
    private Rigidbody2D _rig = null;
    private int Level;
    private int H_exp;
    private float _timer = 0;
    private bool HouseEnter;
    private Enemy _enemy;
    private Bullet _bullet;
    public float interval = 1f;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        _rig = GetComponent<Rigidbody2D>();
        _enemy =FindObjectOfType<Enemy>();
        _bullet = FindObjectOfType<Bullet>();
        _timer =interval;
        //H_exp = enemy.E_exp;
        //H_Gold = enemy.E_Gold;
        
    }

    private void FixedUpdate()
    {
                _timer += Time.deltaTime;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            _rig.velocity = new Vector2(Input.GetAxis("Horizontal"), 0) * H_speed;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && Time.timeScale > 0)
        {
            playerIndex = (playerIndex - 1 + weaponList.Length) % weaponList.Length;
            Debug.Log($"{playerIndex}");
            HaveWeapon();
            WeaponController();
        }
        if (Input.GetKeyDown(KeyCode.L) && Time.timeScale > 0)
        {
            playerIndex = (playerIndex + 1) % weaponList.Length;
            Debug.Log($"{playerIndex}");
            HaveWeapon();
            WeaponController();
        }
        if (Input.GetKeyDown(KeyCode.Space) && /*_bullet.interval < _timer &&*/ Time.timeScale > 0) 
        {
            _timer = 0;
            Debug.Log("射撃");
            bulletshot();
        }
        if (HouseEnter)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && Checkbuy(Pistle_gold, ref _pistlecount))
            {
                    DecreaseGold(Pistle_gold);
                    Debug.Log($"{H_Gold}");
                    Buyweapon(PistleGameSceneUI);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && Checkbuy(AR_gold, ref _ARcount))
            {
                    DecreaseGold(AR_gold);
                    Debug.Log($"{H_Gold}");
                    Buyweapon(ARGameSceneUI);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && Checkbuy(SMG_gold,ref _SMGcount))
            {
                    DecreaseGold(SMG_gold);
                    Debug.Log($"{H_Gold}");
                    Buyweapon(SMGGameSceneUI);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) && Checkbuy(SR_gold,ref _SRcount))
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
    public void bulletshot()
    {
        if (playerIndex == 0 && interval > _timer)
        {
            Instantiate(PistleBullet,muzzle.transform.position,Quaternion.identity);
            PistleBullet.SetActive(true);
        }
        if (playerIndex == 1 && interval > _timer)
        {
            Instantiate(ARBullet, muzzle.transform.position, Quaternion.identity);
            ARBullet.SetActive(true);
        }
        if (playerIndex == 2 && interval > _timer)
        {
            Instantiate(SMGBullet, muzzle.transform.position, Quaternion.identity);
            SMGBullet.SetActive(true);
        }
        if (playerIndex == 3 && interval > _timer)
        {
            Instantiate(SRBullet, muzzle.transform.position, Quaternion.identity);
            SRBullet.SetActive(true);
        }
    }
    private void HaveWeapon()
    {
        if (playerIndex == 1 && _ARcount == 0)
        {
            NoWeaponWarningUI.SetActive(true);
            StartCoroutine(UIfadeout());
        }
        if (playerIndex == 2 && _SMGcount == 0)
        {
            NoWeaponWarningUI.SetActive(true);
            StartCoroutine(UIfadeout());
        }
        if (playerIndex == 3 && _SRcount== 0)
        {
            NoWeaponWarningUI.SetActive(true);
            StartCoroutine(UIfadeout());
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
        }
        if (playerIndex == 2)
        {
            PistleselectUI.SetActive(false);
            SMGselectUI.SetActive(true);
            ARselectUI.SetActive(false);
            SRSelectUI.SetActive(false);
        }
        if (playerIndex == 3)
        {
            PistleselectUI.SetActive(false);
            SRSelectUI.SetActive(true);
            ARselectUI.SetActive(false);
            SMGselectUI.SetActive(false);
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
