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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
        _spawnrnd = Random.Range(0, 5);
        for (int i = 0; i < _spawnrnd; i++)
        {
        Instantiate(enemy[0], spawnpoint[0].transform.position, Quaternion.identity);

        }
    }
}
