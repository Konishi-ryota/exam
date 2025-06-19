using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemySpawnpoint : MonoBehaviour
{
    [SerializeField] GameObject[] spawnpoint;
    [SerializeField] GameObject[] enemy;
    [SerializeField] Text timerText;

    private Hero _hero;
    private bool _isSpawning = true;
    private float _timer = 0;
    [NonSerialized] public int _remainTime;
    public int _stageTimer = 30;
    private int _waveTimer => _stageTimer;
    // Start is called before the first frame update
    void Start()
    {
        _hero = FindAnyObjectByType<Hero>();
    }

    // Update is called once per frame
    void Update()
    {
        WaveSetting();
        int rnd = 0;
        if (Time.frameCount % Application.targetFrameRate == 0 && _isSpawning)
        {
            rnd = Random.Range(0, 11);
            Instantiate(enemy[0], spawnpoint[0].transform.position, Quaternion.identity);
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
            _stageTimer = 0;
            _timer = 0;
            Time.timeScale = 0;
            timerText.text = "S�������ăX�^�[�g";
            if (Input.GetKeyDown(KeyCode.S))
            {
                _hero._HouseEnter = false;
                _stageTimer = 3;
                Time.timeScale = 1;
            }
        }
    }
}
