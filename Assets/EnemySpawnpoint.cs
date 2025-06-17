using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnpoint : MonoBehaviour
{
    [SerializeField] GameObject spawnpoint;
    [SerializeField] GameObject enemy;
    private Hero _hero;
    private bool _isSpawning = true;
    // Start is called before the first frame update
    void Start()
    {
        _hero = FindAnyObjectByType<Hero>();
    }

    // Update is called once per frame
    void Update()
    {
        int rnd = 0;
        if (Time.frameCount % 60 == 0 && _isSpawning)
        {
            Hero _hero = FindAnyObjectByType<Hero>();
            rnd = Random.Range(0, 11);
            Instantiate(enemy, spawnpoint.transform.position, Quaternion.identity);
        }
        if (_hero._RemainTime <= 0)
        {
            _isSpawning = false;
        }
        else
        {
            _isSpawning=true;
        }
    }
}
