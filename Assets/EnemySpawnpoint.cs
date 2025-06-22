using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemySpawnpoint : MonoBehaviour
{
    [SerializeField] GameObject[] spawnpoint;
    [SerializeField] GameObject[] enemy;

    private bool _isSpawning = true;
    private int _spawnrnd;
    private float _timer = 0;
    [NonSerialized] public int _remainTime;
    private int waveCount = 1;
    private int RemainWaveCount;
    [SerializeField] int MaxWaveCount = 7;
    public int _stageTimer = 30;
    [SerializeField] Text timerText;
    private Hero _hero;
    private int _enemySpawnFrequency;

    // Start is called before the first frame update
    void Start()
    {
        _hero = FindAnyObjectByType<Hero>();
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
    private void EnemySpawn()
    {
        if (waveCount == 1 || waveCount == 2)
        {
            if (Time.frameCount % (Application.targetFrameRate * 2) == 0)
            {
                Debug.Log("�G����");
                Instantiate(enemy[0], spawnpoint[0].transform.position, Quaternion.identity);
            }
        }
        if (waveCount == 3)
        {
            _enemySpawnFrequency = Random.Range(2, 5);
            Debug.Log($"{_enemySpawnFrequency}");
            if (Time.frameCount % (Application.targetFrameRate * _enemySpawnFrequency) == 0)
            { 
                Instantiate(enemy[1], spawnpoint[1].transform.position, Quaternion.identity);
            }
        }
        if (waveCount > 3 && MaxWaveCount != waveCount)
        {
            _enemySpawnFrequency = Random.Range(2, 5);
            Debug.Log($"{_enemySpawnFrequency}");
            if (Time.frameCount % (Application.targetFrameRate * _enemySpawnFrequency) == 0)
            {
                Instantiate(enemy[0], spawnpoint[0].transform.position, Quaternion.identity);
            }
            if (Time.frameCount % (Application.targetFrameRate * _enemySpawnFrequency + 1) == 0)
            {
                Instantiate(enemy[1], spawnpoint[1].transform.position, Quaternion.identity);
            }
        }
        if (MaxWaveCount == waveCount)
        {
            if (Time.frameCount % (Application.targetFrameRate * 0.5f) == 0)
            {
                Instantiate(enemy[0], spawnpoint[0].transform.position,Quaternion.identity);
            }
            if ((Time.frameCount + 1) % Application.targetFrameRate == 0)
            {
                Instantiate(enemy[1], spawnpoint[1].transform.position, Quaternion.identity);
            }
        }
        //if(waveCount == 1)
        //{
        //    _enemySpawnFrequency = Random.Range(2,5);
        //    Debug.Log($"{_enemySpawnFrequency}");
        //    if (Time.frameCount % (Application.targetFrameRate * _enemySpawnFrequency) == 0)
        //    {
        //      Instantiate(enemy[0], spawnpoint[0].transform.position, Quaternion.identity);
        //    }
        //}
        //if (waveCount == 2)
        //{
        //    _enemySpawnFrequency= Random.Range(2,4);
        //    Debug.Log($"{_enemySpawnFrequency}");
        //    if (Time.frameCount % (Application.targetFrameRate * _enemySpawnFrequency) == 0)
        //    {
        //        Instantiate(enemy[0], spawnpoint[0].transform.position,Quaternion.identity);
        //    }
        //}
        //if (waveCount == 3)
        //{

        //}
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
            _hero._HouseEnter = true;
            _timer = _stageTimer;//S�������܂ł͎c�莞�Ԃ�0�b�ɂ���
            Time.timeScale = 0;
            timerText.text = "S�������ăX�^�[�g";
            if (Input.GetKeyDown(KeyCode.S))
            {
                _hero._HouseEnter = false;
                _timer = 0;//S����������^�C�}�[�����ʂ�ɖ߂�
                waveCount++;
                Time.timeScale = 1;
            }
        }
    }
}
