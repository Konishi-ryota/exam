using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    #region�@�ϐ�
    [SerializeField] GameObject muzzle;

    [SerializeField] GameObject[] ShopUI;
    [SerializeField] GameObject[] WarningUI;
    [SerializeField] GameObject PauseUI;
    [SerializeField] GameObject GameOverUI;
    [SerializeField] Text timerText;
    [SerializeField] GameObject[] WeaponGameSceneUI;

    [SerializeField,Header("�ォ��s�X�g���AAR�ASMG�ASR")] int[] WeaponGold;
    [SerializeField] GameObject[] WeaponSelectUI;
    [SerializeField] GameObject[] Bullet;
    [SerializeField] GameObject[] weaponList;

    [Header("�v���C���[�ݒ�")]
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
    [NonSerialized] public int _PlayerIndex = 0;
    private Rigidbody2D _rig = null;
    private float _PistleTimer;
    private float _ARTimer;  
    private float _SMGTimer;
    private float _SRTimer;
    private float _timer = 0;

    [NonSerialized] public bool _HouseEnter;
    [NonSerialized] public int _remainTime;
    public int _stageTimer = 30;
    private int waveCount = 1;
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
        if (Input.GetKeyDown(KeyCode.K) && Time.timeScale > 0)//����؂�ւ�
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
        if (_HouseEnter)//�V���b�v���J��
        {
            ShopUI[0].SetActive(true);
            ShopUI[1].SetActive(true);
            ShopUI[2].SetActive(true);
            ShopUI[3].SetActive(true);
            if (Input.GetKeyDown(KeyCode.Alpha1) && Checkbuy(WeaponGold[1], ref _ARcount))//�V���b�v�ŕ�����w�����邽�߂̂��
            {
                DecreaseGold(WeaponGold[1]);
                Debug.Log($"{H_Gold}");
                Buyweapon(WeaponGameSceneUI[0]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && Checkbuy(WeaponGold[2], ref _SMGcount))
            {
                DecreaseGold(WeaponGold[2]);
                Debug.Log($"{H_Gold}");
                Buyweapon(WeaponGameSceneUI[1]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && Checkbuy(WeaponGold[3], ref _SRcount))
            {
                DecreaseGold(WeaponGold[3]);
                Debug.Log($"{H_Gold}");
                Buyweapon(WeaponGameSceneUI[2]);
            }
        }
        else if (!_HouseEnter)//�V���b�v�����
        {
            ShopUI[0].SetActive(false);
            ShopUI[1].SetActive(false);
            ShopUI[2].SetActive(false);
            ShopUI[3].SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.P))//�|�[�Y
        {
            PauseUI.SetActive(true);
            Time.timeScale = 0;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            PauseUI.SetActive(false);
            Time.timeScale = 1;
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
    /// <summary>
    /// �E�F�[�u�؂�ւ��@�\
    /// </summary>
    private void WaveSetting()
    {
        _timer += Time.deltaTime;
        _remainTime = _stageTimer - (int)_timer;
        timerText.text = "�c�莞�� " + _remainTime.ToString("D2") + " �b";
        if (_remainTime <= 0)
        {
            _HouseEnter = true;
            _timer = _stageTimer;//S�������܂ł͎c�莞�Ԃ�0�b�ɂ���
            Time.timeScale = 0;
            timerText.text = "S�������ăX�^�[�g";
            if (Input.GetKeyDown(KeyCode.S))
            {
                _HouseEnter = false;
                _timer = 0;//S����������^�C�}�[�����ʂ�ɖ߂�
                waveCount++;
                Time.timeScale = 1;
            }
        }
    }
    /// <summary>
    /// �e�������ɌĂ΂�郁�\�b�h
    /// </summary>
    public void bulletshot()
    {
        if (_PlayerIndex == 0 && _PistleTimer < Time.time)//delta.time�𑫂����������time�ŕK�v�Ȏ������Ăяo���������y��
        {
            Instantiate(Bullet[0],muzzle.transform.position,Quaternion.identity);
            Bullet[0].SetActive(true);
            _PistleTimer = Time.time + PistleInterval;//if�����Ă΂ꂽ��Atime�ɃC���^�[�o���𑫂��ăs�X�g���^�C�}�[�ɑ��
            �@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@//�ꎞ�I��time�����^�C�}�[�̕����C���^�[�o�����ԕ��傫���Ȃ邽�߁A�����Ƌ@�\����
        }
        if (_PlayerIndex == 1 && _ARTimer < Time.time && _ARcount > 0)
        {
            Instantiate(Bullet[1], muzzle.transform.position, Quaternion.identity);
            Bullet[1].SetActive(true);
            _ARTimer = Time.time + ARInterval;
        }
        if (_PlayerIndex == 2 && _SMGTimer < Time.time && _SMGcount > 0)
        {
            Instantiate(Bullet[2], muzzle.transform.position, Quaternion.identity);
            Bullet[2].SetActive(true);
            _SMGTimer = Time.time + SMGInterval;
        }
        if (_PlayerIndex == 3 && _SRTimer < Time.time && _SRcount > 0)
        {
            Instantiate(Bullet[3], muzzle.transform.position, Quaternion.identity);
            Bullet[3].SetActive(true);
            _SRTimer = Time.time + SRInterval;
        }
    }
    /// <summary>
    /// �����邩�ǂ����̔��ʂ����邽�߂̃��\�b�h
    /// </summary>
    /// <param name="gold"></param>
    /// <param name="keycount">�������߂̃L�[�������ꂽ��</param>
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
    /// gold�𑝂₷���߂̃��\�b�h
    /// </summary>
    /// <param name="gold"></param>
    public void AddGold(int gold)
    {
        H_Gold += gold;
    }
    /// <summary>
    /// gold�����炷���߂̃��\�b�h
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
    /// UI���\���ɂ��邽�߂̃R���[�`��
    /// </summary>
    /// <param name="fadeoutUI">��\���ɂ�����UI</param>
    /// <returns></returns>
    IEnumerator UIfadeout(GameObject fadeoutUI)
    {
        yield return new WaitForSeconds(3);
        fadeoutUI.SetActive(false);
    }
}
