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
        if (Time.frameCount % Application.targetFrameRate == 0 && _isSpawning)
        {
            rnd = Random.Range(0, 11);
            Instantiate(enemy, spawnpoint.transform.position, Quaternion.identity);
        }
        if (_hero._RemainTime <= 0@|| Time.timeScale == 0)
        {
            _isSpawning = false;
        }
        else
        {
            _isSpawning=true;
        }
    }
}
