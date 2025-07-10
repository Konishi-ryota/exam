using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Hero : MonoBehaviour
    //God Classすぎる、反省
{
    #region　変数
    [SerializeField] GameObject muzzle;

    [SerializeField] AudioClip[] ShotSE;

    [SerializeField] GameObject ShopUI;
    [SerializeField] GameObject[] WarningUI;
    [SerializeField] Text GoldText;
    [SerializeField] Text HPText;
    [SerializeField] Text GameOverWaveText;
    [SerializeField] GameObject GameOverUI;
    [SerializeField] GameObject GameClearUI;

    [SerializeField,Header("上から、Spark、Waveform、Pulse")] int[] WeaponGold;
    [SerializeField] GameObject[] WeaponGameSceneUI;
    [SerializeField] GameObject[] WeaponSelectUI;
    [SerializeField] GameObject[] Bullet;
    [Header("上からBolt、Spark、Waveform、Pulse")]
    [SerializeField] GameObject[] bulletList;

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
    [NonSerialized] public int _bulletIndex = 0;
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
    public bool _isDead;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _rig = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        _house = FindAnyObjectByType<House>();
        _enemySpawnpoint = FindAnyObjectByType<EnemySpawnpoint>();
        Time.timeScale = 1;//再読み込みした時に時間が動くように
        _PistleTimer =PistleInterval;//
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
            //移動用のキーが押されてなかったら
        {
            _animator.SetBool("Player_anim",false);//移動するアニメーション停止
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(H_speed * Time.deltaTime, 0);
            _animator.SetBool("Player_anim", true);//移動するアニメーション実行
            transform.rotation = Quaternion.Euler(0,180,0);//180度回転
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(H_speed * Time.deltaTime, 0);
            _animator.SetBool("Player_anim", true);
            transform.rotation = Quaternion.Euler(0,0,0);//Dキーを押して回転してた時に元に戻す
        }
        if (Input.GetKeyDown(KeyCode.W) && _isGround && Time.timeScale != 0)
        {
            _animator.SetTrigger("Jump");//ジャンプアニメーション実行
            //boolに例えるならtriggerは常にtrueなやつ
            //移動するアニメーションは放置のアニメーションと行ったり来たりするからbool
            //ジャンプはany stateからしか繋がってない一方通行なので、trigger
            _rig.AddForce(new Vector2(0, 350));
        }
        if (Input.GetKeyDown(KeyCode.K) && Time.timeScale > 0)//武器切り替え
        {
            _bulletIndex = (_bulletIndex + 1) % bulletList.Length;
            //%で余りを出すことで、0～3をぐるぐる回ってくれる
            Debug.Log($"{_bulletIndex}");
            WeaponSelecter();
        }
        if (Input.GetKeyDown(KeyCode.L) && Time.timeScale > 0)
        {
            _bulletIndex = (_bulletIndex - 1 + bulletList.Length) % bulletList.Length;
            //そのまま引き算するとマイナスになってしまうので、
            //弾の種類の数(_bulletIndex)から-1したあと、
            //弾のリストの数(bulletList.Length)を足してから剰余残
            //変数が違うだけでどちらも最大値は同じだから成り立つ
            Debug.Log($"{_bulletIndex}");
            WeaponSelecter();
        }
        if (Input.GetKey(KeyCode.Space) && Time.timeScale > 0)
            //スペースを押したときに時間が止まってなかったら
        {
            BulletShot();//弾を撃つ
        }
        if (_HouseEnter)//ショップを開く
        {
            ShopUI.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Alpha1) && Checkbuy(WeaponGold[0], ref _ARcount))
            {
                DecreaseGold(WeaponGold[0]);
                Debug.Log($"{H_Gold}");
                Buyweapon(WeaponGameSceneUI[0]);
                //弾を購入する
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
            ShopUI.SetActive(false);
        }
        if (H_HP <= 0 || _house.HouseHP == _house._enemyIn)
            //プレイヤーのHP、または耐久値が0になったら
        {
            Destroy(this.gameObject);
            GameOverUI.SetActive(true);
            Time.timeScale = 0;
            GameOverWaveText.text = $"{_enemySpawnpoint.waveCount - 1}" + "ウェーブクリア！！";
            //死んだ1ウェーブ前をクリア数に表示
            _isDead = true;
        }

        if (_enemySpawnpoint._isCleared)
        //クリアしてたら
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SceneManager.LoadScene("StartScene");
                //スタートシーンに移行
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                //再ロード
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isGround = true;
            //二段ジャンプできないように
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isGround = false;
        }
    }
    /// <summary>
    /// 弾を撃つ時に呼ばれるメソッド
    /// </summary>
    public void BulletShot()
    {
        if (_bulletIndex == 0 && _PistleTimer < Time.time)
            //delta.timeを足し続けるよりもtimeで必要な時だけ呼び出した方が軽い
        {
            Instantiate(Bullet[0],muzzle.transform.position,transform.rotation);
            _audio.PlayOneShot(ShotSE[0]);
            Bullet[0].SetActive(true);
            _PistleTimer = Time.time + PistleInterval;
            //if文が呼ばれたら、timeにインターバルを足してピストルタイマーに代入
            //一時的にtimeよりもタイマーの方がインターバル時間分大きくなるため、ちゃんと機能する
        }
        if (_bulletIndex == 1 && _ARTimer < Time.time && _ARcount > 0)
        {
            Instantiate(Bullet[1], muzzle.transform.position, transform.rotation);
            //キャラが向いている方向に弾を発射
            _audio.PlayOneShot(ShotSE[1]);
            Bullet[1].SetActive(true);
            _ARTimer = Time.time + ARInterval;
        }
        if (_bulletIndex == 2 && _SMGTimer < Time.time && _SMGcount > 0)
        {
            Instantiate(Bullet[2], muzzle.transform.position, transform.rotation);
            _audio.PlayOneShot(ShotSE[2]);
            Bullet[2].SetActive(true);
            _SMGTimer = Time.time + SMGInterval;
        }
        if (_bulletIndex == 3 && _SRTimer < Time.time && _SRcount > 0)
        {
            Instantiate(Bullet[3], muzzle.transform.position, transform.rotation);
            _audio.PlayOneShot(ShotSE[3]);
            Bullet[3].SetActive(true);
            _SRTimer = Time.time + SRInterval;
        }
    }
    /// <summary>
    /// 金設定用メソッド
    /// </summary>
    private void SetGold()
    { 
        GoldText.text ="所持金: " + H_Gold.ToString();
    }
    /// <summary>
    /// HP設定用メソッド
    /// </summary>
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
    public bool Checkbuy(int gold, ref int keycount)
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
        {//1回買われてたら買えないようにする
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
        Debug.Log($"今の所持金:{H_Gold}");
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
        if (_bulletIndex == 0)//0の時
        {
            WeaponSelectUI[0].SetActive(true);//一個目の選択UIを表示、それ以外を非表示
            WeaponSelectUI[1].SetActive(false);
            WeaponSelectUI[2].SetActive(false);
            WeaponSelectUI[3].SetActive(false);
        }
        if (_bulletIndex == 1)
        {
            WeaponSelectUI[0].SetActive(false);
            WeaponSelectUI[1].SetActive(true);
            WeaponSelectUI[2].SetActive(false);
            WeaponSelectUI[3].SetActive(false);
            if (_ARcount == 0)//購入していなかったら
            {
                WarningUI[2].SetActive(true);
                StartCoroutine(UIfadeout(WarningUI[2]));//警告UIを表示、2秒後に非表示
            }
        }
        if (_bulletIndex == 2)
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
        if (_bulletIndex == 3)
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
        yield return new WaitForSecondsRealtime(2);
        //時間が止まってるときに出すからRealtimeで動かす
        fadeoutUI.SetActive(false);
    }
}
