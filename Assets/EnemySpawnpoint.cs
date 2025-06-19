using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemySpawnpoint : MonoBehaviour
{
    [SerializeField] GameObject[] spawnpoint;
    [SerializeField] GameObject[] enemy;

    private bool _isSpawning = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
}
