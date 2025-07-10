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
    /// <summary>
    /// 敵生成の処理
    /// </summary>
    private void EnemySpawn()
    {
        //本当は１～４、4～7くらいでまとめたかったけど断念
        if (waveCount == 1)
        {
            if (Time.frameCount % (Application.targetFrameRate * 6) == 0)//6秒ごとに一体
            {
                Debug.Log("敵生成");
                Instantiate(enemy[0], spawnpoint[0].transform.position, Quaternion.identity);
                //enemy配列の最初の敵を、spawnpointの最初の場所から、回転させずに生成
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
        if (waveCount == 4)
        {
            if (Time.frameCount % (Application.targetFrameRate * 4) == 0)
            {
                _spawnrnd = Random.Range(0, 2);//出てくる敵をランダムで決定
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
    /// ウェーブ切り替え機能
    /// </summary>
    private void WaveSetting()
    {
        _timer += Time.deltaTime; //タイマーに1f分の秒数を足す
        _remainTime = _stageTimer - (int)_timer;//(int)でfloatをint型に(キャスト)
        //1ウェーブの秒数から引いていく
        timerText.text = "残り時間 " + _remainTime.ToString("D2") + " 秒";
        //それを表示することでタイマーになる
        if (_remainTime <= 0 && waveCount != MaxWaveCount)//残り時間が0の時
        {
                _hero._HouseEnter = true;//ショップを開く
                Time.timeScale = 0;
                WaveText.text = $"次は{waveCount + 1}ウェーブ目";
                timerText.text = "Sを押して\nスタート";//\nで改行
            if (waveCount == MaxWaveCount - 1)
            {
                WaveText.text = "次は最終ウェーブ";//ウェーブ数が最大より一個少なかったら文字変更
            }
            if (Input.GetKeyDown(KeyCode.S))//Sを押したらリスタート
            {
                _hero._HouseEnter = false;
                _timer = 0;//足したdeltaTimeを0にし、また最初から
                waveCount++;//ウェーブ数を一個増やす
                Time.timeScale = 1;
                WaveText.text = $"今は{waveCount}ウェーブ目";
                if (waveCount == MaxWaveCount)
                //Sを押したタイミングでウェーブ数が最大ウェーブ数と同じになったら
                {
                    WaveText.text = "今は最終ウェーブ";
                }
            }
        }
        else if(_remainTime <= 0 && waveCount == MaxWaveCount)
        {
            GameClearUI.SetActive(true);//ゲームクリア処理を行う
            _isCleared = true;
            Time.timeScale = 0;
        }
    }
}
