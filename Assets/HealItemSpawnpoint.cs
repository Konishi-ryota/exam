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
        //�E�F�[�u����3�܂��́A5�A�܂���7�̎�
        {
            CreateItem();
            StartCoroutine(Spawnbool());
        }
    }
    /// <summary>
    /// �A�C�e�������p���\�b�h
    /// </summary>
    private void CreateItem()
    {
        if (!_isSpawned && Time.timeScale != 0)
        {
            Debug.Log("�A�C�e������");
            _isSpawned =true;
            Instantiate(HealItem, transform.position, Quaternion.identity);
        }
    }
    /// <summary>
    /// 1�E�F�[�u��1��������p
    /// </summary>
    /// <returns></returns>
    IEnumerator Spawnbool()
    {
        yield return new WaitForSeconds(_enemySpawnpoint._stageTimer);
        _isSpawned = false;
        //�����1�񓮂�������I��点��������킩��񂩂������炲�艟��
        //Start�ł��g���񂾂낤���A�ł���������1�񂾂����ĈӖ�����Ȃ�����Ȃ�
    }
}
