using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] GameObject bullets;
    [SerializeField] GameObject muzzle;
    [SerializeField] GameObject Pistle;
    [SerializeField] GameObject AR;
    [SerializeField] GameObject SMG;
    [SerializeField] GameObject SR;
    [SerializeField] GameObject goldWarningUI;
    [SerializeField] GameObject DupilicationWarningUI;
    [SerializeField] GameObject PauseUI;
    [SerializeField] GameObject PistleGameSceneUI;
    [SerializeField] GameObject ARGameSceneUI;
    [SerializeField] GameObject SMGGameSceneUI;
    [SerializeField] GameObject SRGameSceneUI;
    [SerializeField] int Pistle_gold;
    [SerializeField] int AR_gold;
    [SerializeField] int SMG_gold;
    [SerializeField] int SR_gold;
    int _pistlecount = 0;
    int _ARcount = 0;
    int _SMGcount = 0;
    int _SRcount = 0;
    private int Level;
    private int H_exp;
    private float _timer;
    private bool HouseEnter;
    [SerializeField] GameObject[] weaponList;
    private int playerIndex;
    private Rigidbody2D _rig = null;
    [Header("HeroSettings")]
    [SerializeField] private int H_Gold;
    [SerializeField] private int H_hp;
    [SerializeField] private int H_speed;
    [SerializeField] private int H_attackPower = 3;
    [SerializeField] private float interval = 1f;
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
        }
        if (Input.GetKeyDown(KeyCode.L) && Time.timeScale > 0)
        {
            playerIndex = (playerIndex + 1) % weaponList.Length;
            Debug.Log($"{playerIndex}");
        }
        if (Input.GetKeyDown(KeyCode.Space) && interval < _timer && Time.timeScale > 0)
        {
            _timer = 0;
            Instantiate(bullets, muzzle.transform.position, Quaternion.identity);
            bullets.SetActive(true);
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
            goldWarningUI.SetActive(true);
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
    public void weaponController()
    {
        if (playerIndex == 0)
        {
            
        }
    }
    IEnumerator UIfadeout()
    {
        Debug.Log("UI非表示開始");
        yield return new WaitForSeconds(3);
        goldWarningUI.SetActive(false);
        DupilicationWarningUI.SetActive(false);
    }
}
