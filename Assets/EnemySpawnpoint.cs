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
    [SerializeField] private int waveCount = 1;
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
        if (waveCount == 1)
        {
            if (Time.frameCount % (Application.targetFrameRate * 2) == 0)
            {
                Debug.Log("敵生成");
                Instantiate(enemy[0], spawnpoint[0].transform.position, Quaternion.identity);
            }
        }
        if (waveCount == 2)
        {
            if (Time.frameCount % (Application.targetFrameRate * 2) == 0)
            {
                Debug.Log("敵生成");
                Instantiate(enemy[1], spawnpoint[1].transform.position, Quaternion.identity);
            }
        }
        if (waveCount == 3)
        {
            _enemySpawnFrequency = Random.Range(40,60);
            Debug.Log($"{_enemySpawnFrequency}");
            if (Time.frameCount % (Application.targetFrameRate /1.5) == 0)
            {
                Instantiate(enemy[0], spawnpoint[0].transform.position, Quaternion.identity);
            }
            if (Time.frameCount %  Application.targetFrameRate == 0)
            {
                Instantiate(enemy[1], spawnpoint[1].transform.position, Quaternion.identity);
            }
        }
        if (waveCount >= 4 && waveCount != MaxWaveCount)
        {
            if (Time.frameCount * _spawnrnd % Application.targetFrameRate == 0)
            {
                _enemySpawnFrequency = Random.Range(1, 5);
                _spawnrnd = Random.Range(0, 2);
                for (int i = 0; i < _enemySpawnFrequency; i++)
                {
                    Instantiate(enemy[_spawnrnd], spawnpoint[0].transform.position, Quaternion.identity);
                }
            }
            if ((Time.frameCount + 30) % Application.targetFrameRate == 0)
            {
                _spawnrnd = Random.Range(0, 3);
                for (int i = 0; i < _spawnrnd; i++)
                {
                    Instantiate(enemy[1], spawnpoint[1].transform.position, Quaternion.identity);
                }
            }
        }
        if (MaxWaveCount == waveCount)
        {

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
    /// ウェーブ切り替え機能
    /// </summary>
    private void WaveSetting()
    {
        _timer += Time.deltaTime;
        _remainTime = _stageTimer - (int)_timer;
        timerText.text = "残り時間 " + _remainTime.ToString("D2") + " 秒";
        if (_remainTime <= 0)
        {
            _hero._HouseEnter = true;
            _timer = _stageTimer;//Sを押すまでは残り時間を0秒にする
            Time.timeScale = 0;
            timerText.text = "Sを押して\nスタート";
            if (Input.GetKeyDown(KeyCode.S))
            {
                _hero._HouseEnter = false;
                _timer = 0;//Sを押したらタイマーを元通りに戻す
                waveCount++;
                Time.timeScale = 1;
            }
        }
    }
}
