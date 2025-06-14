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
    private int Level;
    private int H_exp;
    private float _timer;
    private bool HouseEnter;
    [SerializeField] List<GameObject> weaponList = new();
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
            playerIndex = (playerIndex - 1 + weaponList.Count) % weaponList.Count;
            Debug.Log($"{playerIndex}");
        }
        if (Input.GetKeyDown(KeyCode.L) && Time.timeScale > 0)
        {
            playerIndex = (playerIndex + 1) % weaponList.Count;
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
            int pistle = 1;
            int AR = 1;
            int SMG = 1;
            int SR = 1;
            if (Input.GetKeyDown(KeyCode.Alpha1) && Time.timeScale > 0)
            {
                Debug.Log("1キーが押された");
                if (Checkgold(Pistle_gold,pistle))
                {
                    DecreaseGold(Pistle_gold);
                    Debug.Log($"{H_Gold}");
                    Buyweapon(PistleGameSceneUI);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && Time.timeScale > 0)
            {
                if (Checkgold(AR_gold,AR))
                {
                    DecreaseGold(AR_gold);
                    Debug.Log($"{H_Gold}");
                    Buyweapon(ARGameSceneUI);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && Time.timeScale > 0)
            {
                if (Checkgold(SMG_gold,SMG))
                {
                    DecreaseGold(SMG_gold);
                    Debug.Log($"{H_Gold}");
                    Buyweapon(SMGGameSceneUI);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) && Time.timeScale > 0)
            {
                if (Checkgold(SR_gold,SR))
                {
                    DecreaseGold(SR_gold);
                    Debug.Log($"{H_Gold}");
                    Buyweapon(SRGameSceneUI);
                }
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
    private bool Checkgold(int gold,int keyCount)
    {
        if (H_Gold > gold && keyCount < 2)
        {
            keyCount++;
            Debug.Log("購入可能");
            Debug.Log($"{keyCount}");
            return true;
        }
        else if(H_Gold < gold || keyCount > 1)
        {
            Debug.Log("購入不可");
            goldWarningUI.SetActive(true);
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
    }
    IEnumerator UIfalse()
    {
        Debug.Log("UI非表示開始");
        yield return new WaitForSeconds(3);
        goldWarningUI.SetActive(false);
        DupilicationWarningUI.SetActive(false);
    }
}
