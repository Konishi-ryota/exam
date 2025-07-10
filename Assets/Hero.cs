using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Hero : MonoBehaviour
    //God Class������A����
{
    #region�@�ϐ�
    [SerializeField] GameObject muzzle;

    [SerializeField] AudioClip[] ShotSE;

    [SerializeField] GameObject ShopUI;
    [SerializeField] GameObject[] WarningUI;
    [SerializeField] Text GoldText;
    [SerializeField] Text HPText;
    [SerializeField] Text GameOverWaveText;
    [SerializeField] GameObject GameOverUI;
    [SerializeField] GameObject GameClearUI;

    [SerializeField,Header("�ォ��ASpark�AWaveform�APulse")] int[] WeaponGold;
    [SerializeField] GameObject[] WeaponGameSceneUI;
    [SerializeField] GameObject[] WeaponSelectUI;
    [SerializeField] GameObject[] Bullet;
    [Header("�ォ��Bolt�ASpark�AWaveform�APulse")]
    [SerializeField] GameObject[] bulletList;

    [Header("�v���C���[�ݒ�")]
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
        Time.timeScale = 1;//�ēǂݍ��݂������Ɏ��Ԃ������悤��
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
            //�ړ��p�̃L�[��������ĂȂ�������
        {
            _animator.SetBool("Player_anim",false);//�ړ�����A�j���[�V������~
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(H_speed * Time.deltaTime, 0);
            _animator.SetBool("Player_anim", true);//�ړ�����A�j���[�V�������s
            transform.rotation = Quaternion.Euler(0,180,0);//180�x��]
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(H_speed * Time.deltaTime, 0);
            _animator.SetBool("Player_anim", true);
            transform.rotation = Quaternion.Euler(0,0,0);//D�L�[�������ĉ�]���Ă����Ɍ��ɖ߂�
        }
        if (Input.GetKeyDown(KeyCode.W) && _isGround && Time.timeScale != 0)
        {
            _animator.SetTrigger("Jump");//�W�����v�A�j���[�V�������s
            //bool�ɗႦ��Ȃ�trigger�͏��true�Ȃ��
            //�ړ�����A�j���[�V�����͕��u�̃A�j���[�V�����ƍs�����藈���肷�邩��bool
            //�W�����v��any state���炵���q�����ĂȂ�����ʍs�Ȃ̂ŁAtrigger
            _rig.AddForce(new Vector2(0, 350));
        }
        if (Input.GetKeyDown(KeyCode.K) && Time.timeScale > 0)//����؂�ւ�
        {
            _bulletIndex = (_bulletIndex + 1) % bulletList.Length;
            //%�ŗ]����o�����ƂŁA0�`3�����邮�����Ă����
            Debug.Log($"{_bulletIndex}");
            WeaponSelecter();
        }
        if (Input.GetKeyDown(KeyCode.L) && Time.timeScale > 0)
        {
            _bulletIndex = (_bulletIndex - 1 + bulletList.Length) % bulletList.Length;
            //���̂܂܈����Z����ƃ}�C�i�X�ɂȂ��Ă��܂��̂ŁA
            //�e�̎�ނ̐�(_bulletIndex)����-1�������ƁA
            //�e�̃��X�g�̐�(bulletList.Length)�𑫂��Ă����]�c
            //�ϐ����Ⴄ�����łǂ�����ő�l�͓��������琬�藧��
            Debug.Log($"{_bulletIndex}");
            WeaponSelecter();
        }
        if (Input.GetKey(KeyCode.Space) && Time.timeScale > 0)
            //�X�y�[�X���������Ƃ��Ɏ��Ԃ��~�܂��ĂȂ�������
        {
            BulletShot();//�e������
        }
        if (_HouseEnter)//�V���b�v���J��
        {
            ShopUI.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Alpha1) && Checkbuy(WeaponGold[0], ref _ARcount))
            {
                DecreaseGold(WeaponGold[0]);
                Debug.Log($"{H_Gold}");
                Buyweapon(WeaponGameSceneUI[0]);
                //�e���w������
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
        else if (!_HouseEnter)//�V���b�v�����
        {
            ShopUI.SetActive(false);
        }
        if (H_HP <= 0 || _house.HouseHP == _house._enemyIn)
            //�v���C���[��HP�A�܂��͑ϋv�l��0�ɂȂ�����
        {
            Destroy(this.gameObject);
            GameOverUI.SetActive(true);
            Time.timeScale = 0;
            GameOverWaveText.text = $"{_enemySpawnpoint.waveCount - 1}" + "�E�F�[�u�N���A�I�I";
            //����1�E�F�[�u�O���N���A���ɕ\��
            _isDead = true;
        }

        if (_enemySpawnpoint._isCleared)
        //�N���A���Ă���
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SceneManager.LoadScene("StartScene");
                //�X�^�[�g�V�[���Ɉڍs
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                //�ă��[�h
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isGround = true;
            //��i�W�����v�ł��Ȃ��悤��
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
    /// �e�������ɌĂ΂�郁�\�b�h
    /// </summary>
    public void BulletShot()
    {
        if (_bulletIndex == 0 && _PistleTimer < Time.time)
            //delta.time�𑫂����������time�ŕK�v�Ȏ������Ăяo���������y��
        {
            Instantiate(Bullet[0],muzzle.transform.position,transform.rotation);
            _audio.PlayOneShot(ShotSE[0]);
            Bullet[0].SetActive(true);
            _PistleTimer = Time.time + PistleInterval;
            //if�����Ă΂ꂽ��Atime�ɃC���^�[�o���𑫂��ăs�X�g���^�C�}�[�ɑ��
            //�ꎞ�I��time�����^�C�}�[�̕����C���^�[�o�����ԕ��傫���Ȃ邽�߁A�����Ƌ@�\����
        }
        if (_bulletIndex == 1 && _ARTimer < Time.time && _ARcount > 0)
        {
            Instantiate(Bullet[1], muzzle.transform.position, transform.rotation);
            //�L�����������Ă�������ɒe�𔭎�
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
    /// ���ݒ�p���\�b�h
    /// </summary>
    private void SetGold()
    { 
        GoldText.text ="������: " + H_Gold.ToString();
    }
    /// <summary>
    /// HP�ݒ�p���\�b�h
    /// </summary>
    public void SetHP()
    {
        HPText.text = "�c��HP:" + H_HP.ToString();
    }
    /// <summary>
    /// �����邩�ǂ����̔��ʂ����邽�߂̃��\�b�h
    /// </summary>
    /// <param name="gold"></param>
    /// <param name="keycount">�������߂̃L�[�������ꂽ��</param>
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
        {//1�񔃂��Ă��甃���Ȃ��悤�ɂ���
            WarningUI[1].SetActive(true);
            StartCoroutine(UIfadeout(WarningUI[1]));
            return false;
        }
            return false;
    }
    /// <summary>
    /// gold�𑝂₷���߂̃��\�b�h
    /// </summary>
    /// <param name="gold"></param>
    public void AddGold(int gold)
    {
        H_Gold += gold;
        Debug.Log($"���̏�����:{H_Gold}");
        SetGold();
    }
    /// <summary>
    /// gold�����炷���߂̃��\�b�h
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
    /// ���킪�������ɌĂяo�������
    /// </summary>
    /// <param name="weapon"></param>
    public void Buyweapon(GameObject weapon)
    {
        weapon.SetActive(true);
    }
    /// <summary>
    /// �����I�����邽�߂̃��\�b�h
    /// </summary>
    public void WeaponSelecter()
    {
        if (_bulletIndex == 0)//0�̎�
        {
            WeaponSelectUI[0].SetActive(true);//��ڂ̑I��UI��\���A����ȊO���\��
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
            if (_ARcount == 0)//�w�����Ă��Ȃ�������
            {
                WarningUI[2].SetActive(true);
                StartCoroutine(UIfadeout(WarningUI[2]));//�x��UI��\���A2�b��ɔ�\��
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
    /// UI���\���ɂ��邽�߂̃R���[�`��
    /// </summary>
    /// <param name="fadeoutUI">��\���ɂ�����UI</param>
    /// <returns></returns>
    IEnumerator UIfadeout(GameObject fadeoutUI)
    {
        yield return new WaitForSecondsRealtime(2);
        //���Ԃ��~�܂��Ă�Ƃ��ɏo������Realtime�œ�����
        fadeoutUI.SetActive(false);
    }
}
