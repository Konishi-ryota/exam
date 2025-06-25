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
        //ウェーブ数が3または、5、または7の時
        {
            CreateItem();
            StartCoroutine(Spawnbool());
        }
    }
    /// <summary>
    /// アイテム生成用メソッド
    /// </summary>
    private void CreateItem()
    {
        if (!_isSpawned && Time.timeScale != 0)
        {
            Debug.Log("アイテム生成");
            _isSpawned =true;
            Instantiate(HealItem, transform.position, Quaternion.identity);
        }
    }
    /// <summary>
    /// 1ウェーブに1個生成する用
    /// </summary>
    /// <returns></returns>
    IEnumerator Spawnbool()
    {
        yield return new WaitForSeconds(_enemySpawnpoint._stageTimer);
        _isSpawned = false;
        //これも1回動かしたら終わらせるやり方がわからんかったからごり押し
        //Startでも使うんだろうか、でもそういう1回だけって意味じゃないからなぁ
    }
}
