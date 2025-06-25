using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItemSpawnpoint : MonoBehaviour
{
    private EnemySpawnpoint _enemySpawnpoint;
    private bool _isSpawned;
    [SerializeField] GameObject HealItem;
    // Start is called before the first frame update
    void Start()
    {
        _enemySpawnpoint = FindAnyObjectByType<EnemySpawnpoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemySpawnpoint.waveCount == 3 || _enemySpawnpoint.waveCount == 5 || _enemySpawnpoint.waveCount == 7)
        {
            CreateItem();
            StartCoroutine(Spawnbool());
        }
    }
    private void CreateItem()
    {
        if (!_isSpawned && Time.timeScale != 0)
        {
            Debug.Log("ÉAÉCÉeÉÄê∂ê¨");
            _isSpawned =true;
            Instantiate(HealItem, transform.position, Quaternion.identity);
        }
    }
    IEnumerator Spawnbool()
    {
        yield return new WaitForSeconds(10);
        _isSpawned = false;
    }
}
