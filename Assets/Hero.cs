using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Hero : MonoBehaviour
{
    #region　変数
    [SerializeField] GameObject muzzle;

    [SerializeField] AudioClip[] ShotSE;

    [SerializeField] GameObject[] ShopUI;
    [SerializeField] GameObject[] WarningUI;
    [SerializeField] Text GoldText;
    [SerializeField] Text HPText;
    [SerializeField] Text GameOverWaveText;
    [SerializeField] GameObject PauseUI;
    [SerializeField] GameObject GameOverUI;
    [SerializeField] GameObject GameClearUI;

    [SerializeField,Header("上から、Spark、Waveform、Pulse")] int[] WeaponGold;
    [SerializeField] GameObject[] WeaponGameSceneUI;
    [SerializeField] GameObject[] WeaponSelectUI;
    [SerializeField] GameObject[] Bullet;
    [Header("上からBolt、Spark、Waveform、Pulse")]
    [SerializeField] GameObject[] weaponList;

    [Header("プレイヤー設定")]
    public int H_HP;
    [SerializeField] private int H_Gold;
    [SerializeField] private int H_speed;
    [SerializeField] float PistleInterval;
    [SerializeField] float ARInterval;
    [SerializeField] float SMGInterval;
    [SerializeField] float SRInterval;

    private int _ARcount = 0;
    private int _SMGcount = 0;
    private int _SRcount = 0;
    [NonSerialized] public int _PlayerIndex = 0;
    private float _PistleTimer;
    private float _ARTimer;  
    private float _SMGTimer;
    private float _SRTimer;

    private Rigidbody2D _rig = null;
    private Animator _animator;
    private AudioSource _audio;
    private House _house;
    private EnemySpawnpoint _enemySpawnpoint;
    [NonSerialized] public bool _HouseEnter;
    private bool _isGround;
    private bool _isPause;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        _rig = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        _house = FindAnyObjectByType<House>();
        _enemySpawnpoint = FindAnyObjectByType<EnemySpawnpoint>();
        _PistleTimer =PistleInterval;
        _ARTimer = ARInterval;
        _SMGTimer = SMGInterval;
        _SMGTimer = SRInterval;
        SetGold(); 
        SetHP();
    }
    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKey(KeyCode.D) || !Input.GetKey(KeyCode.A) || !Input.GetKey(KeyCode.W))
        {
            _animator.SetBool("Player_anim",false);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(H_speed * Time.deltaTime, 0);
            _animator.SetBool("Player_anim", true);
            transform.rotation = Quaternion.Euler(0,180,0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(H_speed * Time.deltaTime, 0);
            _animator.SetBool("Player_anim", true);
            transform.rotation = Quaternion.Euler(0,0,0);
        }
        if (Input.GetKeyDown(KeyCode.W) && _isGround && Time.timeScale != 0)
        {
            _animator.SetTrigger("Jump");
            _rig.AddForce(new Vector2(0, 350));
        }
        if (Input.GetKeyDown(KeyCode.K) && Time.timeScale > 0)//武器切り替え
        {
            _PlayerIndex = (_PlayerIndex + 1) % weaponList.Length;
            Debug.Log($"{_PlayerIndex}");
            WeaponSelecter();
        }
        if (Input.GetKeyDown(KeyCode.L) && Time.timeScale > 0)
        {
            _PlayerIndex = (_PlayerIndex - 1 + weaponList.Length) % weaponList.Length;
            Debug.Log($"{_PlayerIndex}");
            WeaponSelecter();
        }
        if (Input.GetKey(KeyCode.Space) && Time.timeScale > 0)
        {
            bulletshot();
        }
        if (_HouseEnter)//ショップを開く
        {
            GoldText.gameObject.SetActive(true);
            ShopUI[0].SetActive(true);
            ShopUI[1].SetActive(true);
            ShopUI[2].SetActive(true);
            ShopUI[3].SetActive(true);
            if (Input.GetKeyDown(KeyCode.Alpha1) && Checkbuy(WeaponGold[0], ref _ARcount))//ショップで武器を購入するためのやつ
            {
                DecreaseGold(WeaponGold[0]);
                Debug.Log($"{H_Gold}");
                Buyweapon(WeaponGameSceneUI[0]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && Checkbuy(WeaponGold[1], ref _SMGcount))
            {
                DecreaseGold(WeaponGold[1]);
                Debug.Log($"{H_Gold}");
                Buyweapon(WeaponGameSceneUI[1]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && Checkbuy(WeaponGold[2], ref _SRcount))
            {
                DecreaseGold(WeaponGold[2]);
                Debug.Log($"{H_Gold}");
                Buyweapon(WeaponGameSceneUI[2]);
            }
        }
        else if (!_HouseEnter)//ショップを閉じる
        {
            GoldText.gameObject.SetActive(false);
            ShopUI[0].SetActive(false);
            ShopUI[1].SetActive(false);
            ShopUI[2].SetActive(false);
            ShopUI[3].SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.P))//ポーズ
        {
            PauseUI.SetActive(true);
            Time.timeScale = 0;
            _isPause = true;
        }
        if (Input.GetKeyDown(KeyCode.S) && _isPause)
        {
            PauseUI.SetActive(false);
            Time.timeScale = 1;
            _isPause = false;
        }
        if (H_HP <= 0 || _house.HouseHP == _house._enemyIn)
        {
            Destroy(this.gameObject);
            GameOverUI.SetActive(true);
            Time.timeScale = 0;
            GameOverWaveText.text = $"{_enemySpawnpoint.waveCount}" + "ウェーブクリア！！";
        }
        if (_isPause)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("StartScene");
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isGround = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isGround = false;
            _animator.SetBool("Player_Jump", true);
        }
    }
    /// <summary>
    /// 弾を撃つ時に呼ばれるメソッド
    /// </summary>
    public void bulletshot()
    {
        if (_PlayerIndex == 0 && _PistleTimer < Time.time)//delta.timeを足し続けるよりもtimeで必要な時だけ呼び出した方が軽い
        {
            Instantiate(Bullet[0],muzzle.transform.position,transform.rotation);
            _audio.PlayOneShot(ShotSE[0]);
            Bullet[0].SetActive(true);
            _PistleTimer = Time.time + PistleInterval;//if文が呼ばれたら、timeにインターバルを足してピストルタイマーに代入
            　　　　　　　　　　　　　　　　　　　　　//一時的にtimeよりもタイマーの方がインターバル時間分大きくなるため、ちゃんと機能する
        }
        if (_PlayerIndex == 1 && _ARTimer < Time.time && _ARcount > 0)
        {
            Instantiate(Bullet[1], muzzle.transform.position, transform.rotation);//キャラが向いている方向に弾を発射
            _audio.PlayOneShot(ShotSE[1]);
            Bullet[1].SetActive(true);
            _ARTimer = Time.time + ARInterval;
        }
        if (_PlayerIndex == 2 && _SMGTimer < Time.time && _SMGcount > 0)
        {
            Instantiate(Bullet[2], muzzle.transform.position, transform.rotation);
            _audio.PlayOneShot(ShotSE[2]);
            Bullet[2].SetActive(true);
            _SMGTimer = Time.time + SMGInterval;
        }
        if (_PlayerIndex == 3 && _SRTimer < Time.time && _SRcount > 0)
        {
            Instantiate(Bullet[3], muzzle.transform.position, transform.rotation);
            _audio.PlayOneShot(ShotSE[3]);
            Bullet[3].SetActive(true);
            _SRTimer = Time.time + SRInterval;
        }
    }
    private void SetGold()
    { 
        GoldText.text ="所持金: " + H_Gold.ToString();
    }
    public void SetHP()
    {
        HPText.text = "残りHP:" + H_HP.ToString();
    }
    /// <summary>
    /// 買えるかどうかの判別をするためのメソッド
    /// </summary>
    /// <param name="gold"></param>
    /// <param name="keycount">買うためのキーが押された回数</param>
    /// <returns></returns>
    private bool Checkbuy(int gold, ref int keycount)
    {
        if (H_Gold >= gold && keycount == 0)
        {
            keycount++;
            return true;
        }
        else if(H_Gold < gold)
        {
            WarningUI[0].SetActive(true);
            StartCoroutine(UIfadeout(WarningUI[0]));
            return false;
        }
        else if (keycount > 0)
        {
            WarningUI[1].SetActive(true);
            StartCoroutine(UIfadeout(WarningUI[1]));
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
        Debug.Log($"{H_Gold}");
        SetGold();
    }
    /// <summary>
    /// goldを減らすためのメソッド
    /// </summary>
    /// <param name="gold"></param>
    public void DecreaseGold(int gold)
    {
        H_Gold -= gold;
        SetGold();
        if (H_Gold <0)
        {
            H_Gold = 0;
        }
    }
    /// <summary>
    /// 武器が買う時に呼び出されるやつ
    /// </summary>
    /// <param name="weapon"></param>
    public void Buyweapon(GameObject weapon)
    {
        weapon.SetActive(true);
    }
    /// <summary>
    /// 武器を選択するためのメソッド
    /// </summary>
    public void WeaponSelecter()
    {
        if (_PlayerIndex == 0)
        {
            WeaponSelectUI[0].SetActive(true);
            WeaponSelectUI[1].SetActive(false);
            WeaponSelectUI[2].SetActive(false);
            WeaponSelectUI[3].SetActive(false);
        }
        if (_PlayerIndex == 1)
        {
            WeaponSelectUI[0].SetActive(false);
            WeaponSelectUI[1].SetActive(true);
            WeaponSelectUI[2].SetActive(false);
            WeaponSelectUI[3].SetActive(false);
            if (_ARcount == 0)
            {
                WarningUI[2].SetActive(true);
                StartCoroutine(UIfadeout(WarningUI[2]));
            }
        }
        if (_PlayerIndex == 2)
        {
            WeaponSelectUI[0].SetActive(false);
            WeaponSelectUI[1].SetActive(false);
            WeaponSelectUI[2].SetActive(true);
            WeaponSelectUI[3].SetActive(false);
            if (_SMGcount == 0)
            {
                WarningUI[2].SetActive(true);
                StartCoroutine(UIfadeout(WarningUI[2]));
            }
        }
        if (_PlayerIndex == 3)
        {
            WeaponSelectUI[0].SetActive(false);
            WeaponSelectUI[1].SetActive(false);
            WeaponSelectUI[2].SetActive(false);
            WeaponSelectUI[3].SetActive(true);
            if (_SRcount == 0)
            {
                WarningUI[2].SetActive(true);
                StartCoroutine(UIfadeout(WarningUI[2]));
            }
        }
    }
    /// <summary>
    /// UIを非表示にするためのコルーチン
    /// </summary>
    /// <param name="fadeoutUI">非表示にしたいUI</param>
    /// <returns></returns>
    IEnumerator UIfadeout(GameObject fadeoutUI)
    {
        yield return new WaitForSecondsRealtime(3);
        fadeoutUI.SetActive(false);
    }
}
