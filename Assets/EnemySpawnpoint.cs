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
    private int _spawnrnd;
    private int _secondSpawnrnd;
    private float _timer = 0;
    [NonSerialized] public int _remainTime;
    public int waveCount = 1;
    public int MaxWaveCount = StartScene.Maxwave;
    public int _stageTimer = 30;
    [SerializeField] Text timerText;
    [SerializeField] Text WaveText;
    [SerializeField] GameObject GameClearUI;
    private Hero _hero;

    // Start is called before the first frame update
    void Start()
    {
        _hero = FindAnyObjectByType<Hero>();
        Debug.Log($"{MaxWaveCount}");
        Debug.Log($"{Application.targetFrameRate}");
        WaveText.text = $"今は{waveCount}ウェーブ目"; 
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
            if (Time.frameCount % (Application.targetFrameRate *5) == 0)
            {
                Debug.Log("敵生成");
                Debug.Log($"{Time.frameCount}");
                Instantiate(enemy[0], spawnpoint[0].transform.position, Quaternion.identity);
            }
        }
        if (waveCount == 2)
        {
            if (Time.frameCount % (Application.targetFrameRate * 5) == 0)
            {
                Debug.Log("敵生成");
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
        if (waveCount >= 4 && waveCount != MaxWaveCount)
        {
            if (Time.frameCount % (Application.targetFrameRate * 2) == 0)
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
        if (MaxWaveCount == waveCount && MaxWaveCount == 7)
        {
            if (Time.frameCount % Application.targetFrameRate == 0)
            {
                _spawnrnd = Random.Range(0, 2);
                Instantiate(enemy[_spawnrnd], spawnpoint[0].transform.position, Quaternion.identity);
            }
            if (Time.frameCount % (Application.targetFrameRate * 2) == 0)
            {
                _secondSpawnrnd = Random.Range(2, 4);
                Instantiate(enemy[_secondSpawnrnd], spawnpoint[1].transform.position, Quaternion.identity);
            }
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
        if (_remainTime <= 0 && waveCount != MaxWaveCount)
        {
            _hero._HouseEnter = true;
            _timer = _stageTimer;//Sを押すまでは残り時間を0秒にする
            Time.timeScale = 0;
            WaveText.text = $"次は{waveCount + 1}ウェーブ目";
            timerText.text = "Sを押して\nスタート";
            if (waveCount == MaxWaveCount - 1)
            {
                WaveText.text = "次は最終ウェーブ";
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                _hero._HouseEnter = false;
                _timer = 0;//Sを押したらタイマーを元通りに戻す
                waveCount++;
                WaveText.text = $"今は{waveCount}ウェーブ目";
                Time.timeScale = 1;
                if (waveCount == MaxWaveCount)
                {
                    WaveText.text = "今は最終ウェーブ";
                    StartCoroutine(EndGame());
                }
            }
        }
    }
    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(10);
        GameClearUI.SetActive(true);
        Time.timeScale = 0;
    }
}
