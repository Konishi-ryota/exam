using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemySpawnpoint : MonoBehaviour
{
    [SerializeField] GameObject[] spawnpoint;
    [SerializeField] GameObject[] enemy;

    private bool _isSpawning1 = true;
    private bool _isSpawning2 = true;
    private bool _isSpawning3 = true;
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
        EnemySpawn();
        if (Time.timeScale == 0)
        {
            _isSpawning1= false;
            _isSpawning2= false;
            _isSpawning3 = false;
        }
        else
        {
            _isSpawning1= true;
            _isSpawning2= true;
            _isSpawning3 = true;
        }
    }
    private void EnemySpawn()
    {
        if (_hero.transform.position.x > spawnpoint[0].transform.position.x && Time.frameCount % (Application.targetFrameRate *3) == 0)
        {
            Instantiate(enemy[0],spawnpoint[0].transform.position,Quaternion.identity);
        }
        if (_hero.transform.position.x < spawnpoint[0].transform.position.x)
        {
            _isSpawning1 = false;
        }
        if (!_isSpawning1 && _hero.transform.position.x > spawnpoint[1].transform.position.x && Time.frameCount % (Application.targetFrameRate * 2) == 0)
        {
            Instantiate(enemy[1],spawnpoint[1].transform.position, Quaternion.identity);
        }
        if (_hero.transform.position.x < spawnpoint[1].transform.position.x)
        {
            _isSpawning2 = false;
        }
        if (!_isSpawning1 && !_isSpawning2 && _hero.transform.position.x > spawnpoint[2].transform.position.x && Time.frameCount % (Application.targetFrameRate * 2) == 0)
        {
            Instantiate(enemy[2], spawnpoint[2].transform.position, Quaternion.identity);
        }
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
            _timer = _stageTimer;//Sを押すまでは残り時間を0秒にする
            Time.timeScale = 0;
            timerText.text = "Sを押してスタート";
            if (Input.GetKeyDown(KeyCode.S))
            {
                _timer = 0;//Sを押したらタイマーを元通りに戻す
                waveCount++;
                Time.timeScale = 1;
            }
        }
    }
}
