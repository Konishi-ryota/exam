using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemySpawnpoint : MonoBehaviour
{
    [SerializeField] GameObject[] spawnpoint;
    [SerializeField] GameObject[] enemy;

    private bool _isSpawning = true;
    public bool _isCleared;
    private int _spawnrnd;
    private int _secondSpawnrnd;
    private float _timer = 0;
    [NonSerialized] public int _remainTime;
    public int waveCount = 1;
    private int MaxWaveCount = StartScene.Maxwave;
    public int _stageTimer = 30;
    [SerializeField] Text timerText;
    [SerializeField] Text WaveText;
    [SerializeField] GameObject GameClearUI;
    private Hero _hero;

    // Start is called before the first frame update
    void Start()
    {
        _isCleared = false;
        _hero = FindAnyObjectByType<Hero>();
        Debug.Log($"{MaxWaveCount}+ 1");
        Debug.Log($"{Application.targetFrameRate}");
        WaveText.text = $"����{waveCount}�E�F�[�u��"; 
    }

    // Update is called once per frame
    void Update()
    {
        WaveSetting();
        if (_isSpawning && Time.frameCount % Application.targetFrameRate == 0)
        {
            EnemySpawn();
        }
        if (Time.timeScale == 0)
        {
            _isSpawning = false;
        }
        else
        {
            _isSpawning= true;
        }
    }
    /// <summary>
    /// �G�����̏���
    /// </summary>
    private void EnemySpawn()
    {
        //�{���͂P�`�S�A4�`7���炢�ł܂Ƃ߂����������ǒf�O
        if (waveCount == 1)
        {
            if (Time.frameCount % (Application.targetFrameRate * 6) == 0)//6�b���ƂɈ��
            {
                Debug.Log("�G����");
                Instantiate(enemy[0], spawnpoint[0].transform.position, Quaternion.identity);
                //enemy�z��̍ŏ��̓G���Aspawnpoint�̍ŏ��̏ꏊ����A��]�������ɐ���
            }
        }
        if (waveCount == 2)
        {
            if (Time.frameCount % (Application.targetFrameRate * 5) == 0)
            {
                Debug.Log("�G����");
                Instantiate(enemy[2], spawnpoint[1].transform.position, Quaternion.identity);
            }
        }
        if (waveCount == 3)
        {
            if (Time.frameCount % (Application.targetFrameRate * 5) == 0)
            {
                Instantiate(enemy[0], spawnpoint[0].transform.position, Quaternion.identity);
            }
            if (Time.frameCount %  (Application.targetFrameRate * 5) == 0)
            {
                Instantiate(enemy[2], spawnpoint[1].transform.position, Quaternion.identity);
            }
        }
        if (waveCount == 4)
        {
            if (Time.frameCount % (Application.targetFrameRate * 4) == 0)
            {
                _spawnrnd = Random.Range(0, 2);//�o�Ă���G�������_���Ō���
                Instantiate(enemy[_spawnrnd], spawnpoint[0].transform.position, Quaternion.identity);
            }
            if (Time.frameCount % (Application.targetFrameRate * 5) == 0)
            {
                _secondSpawnrnd = Random.Range(2, 4);
                Instantiate(enemy[2], spawnpoint[1].transform.position, Quaternion.identity);
            }
        }
        if (waveCount == 5)
        {
            if (Time.frameCount % (Application.targetFrameRate * 4) == 0)
            {
                _spawnrnd = Random.Range(0, 2);
                Instantiate(enemy[_spawnrnd], spawnpoint[0].transform.position, Quaternion.identity);
            }
            if (Time.frameCount % (Application.targetFrameRate * 5) == 0)
            {
                _secondSpawnrnd = Random.Range(2, 4);
                Instantiate(enemy[_secondSpawnrnd], spawnpoint[1].transform.position, Quaternion.identity);
            }
        }
        if (waveCount == 6)
        {
            if (Time.frameCount % (Application.targetFrameRate * 3) == 0)
            {
                _spawnrnd = Random.Range(0, 2);
                Instantiate(enemy[_spawnrnd], spawnpoint[0].transform.position, Quaternion.identity);
            }
            if (Time.frameCount % (Application.targetFrameRate * 3.5) == 0)
            {
                _secondSpawnrnd = Random.Range(2, 4);
                Instantiate(enemy[_secondSpawnrnd], spawnpoint[1].transform.position, Quaternion.identity);
            }
        }
        if (MaxWaveCount == waveCount && MaxWaveCount == 7)
        {
            if (Time.frameCount % (Application.targetFrameRate* 2) == 0)
            {
                _spawnrnd = Random.Range(0, 2);
                Instantiate(enemy[_spawnrnd], spawnpoint[0].transform.position, Quaternion.identity);
            }
            if (Time.frameCount % (Application.targetFrameRate * 2.5) == 0)
            {
                _secondSpawnrnd = Random.Range(2, 4);
                Instantiate(enemy[_secondSpawnrnd], spawnpoint[1].transform.position, Quaternion.identity);
            }
        }
    }
    /// <summary>
    /// �E�F�[�u�؂�ւ��@�\
    /// </summary>
    private void WaveSetting()
    {
        _timer += Time.deltaTime; //�^�C�}�[��1f���̕b���𑫂�
        _remainTime = _stageTimer - (int)_timer;//(int)��float��int�^��(�L���X�g)
        //1�E�F�[�u�̕b����������Ă���
        timerText.text = "�c�莞�� " + _remainTime.ToString("D2") + " �b";
        //�����\�����邱�ƂŃ^�C�}�[�ɂȂ�
        if (_remainTime <= 0 && waveCount != MaxWaveCount)//�c�莞�Ԃ�0�̎�
        {
                _hero._HouseEnter = true;//�V���b�v���J��
                Time.timeScale = 0;
                WaveText.text = $"����{waveCount + 1}�E�F�[�u��";
                timerText.text = "S��������\n�X�^�[�g";//\n�ŉ��s
            if (waveCount == MaxWaveCount - 1)
            {
                WaveText.text = "���͍ŏI�E�F�[�u";//�E�F�[�u�����ő������Ȃ������當���ύX
            }
            if (Input.GetKeyDown(KeyCode.S))//S���������烊�X�^�[�g
            {
                _hero._HouseEnter = false;
                _timer = 0;//������deltaTime��0�ɂ��A�܂��ŏ�����
                waveCount++;//�E�F�[�u��������₷
                Time.timeScale = 1;
                WaveText.text = $"����{waveCount}�E�F�[�u��";
                if (waveCount == MaxWaveCount)
                //S���������^�C�~���O�ŃE�F�[�u�����ő�E�F�[�u���Ɠ����ɂȂ�����
                {
                    WaveText.text = "���͍ŏI�E�F�[�u";
                }
            }
        }
        else if(_remainTime <= 0 && waveCount == MaxWaveCount)
        {
            GameClearUI.SetActive(true);//�Q�[���N���A�������s��
            _isCleared = true;
            Time.timeScale = 0;
        }
    }
}
