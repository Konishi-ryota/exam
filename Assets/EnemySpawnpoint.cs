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
    public int _stageTimer = 30;
    [SerializeField] Text timerText;
    private Hero _hero;
    // Start is called before the first frame update
    void Start()
    {
        _hero = FindAnyObjectByType<Hero>();
    }

    // Update is called once per frame
    void Update()
    {
        WaveSetting();
        if (Time.frameCount % Application.targetFrameRate == 0 && _isSpawning)
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
           _spawnrnd = Random.Range(0, 5);
           for (int i = 0; i < _spawnrnd; i++)
           {
              Instantiate(enemy[0], spawnpoint[0].transform.position, Quaternion.identity);

           }
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
