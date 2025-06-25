using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("EnemySettings")]
    [SerializeField] private float E_speed;
    [SerializeField] private int E_attack;
    [SerializeField] private int E_hp =10;
    public int E_Gold;

    private Rigidbody2D _rig = null;
    private Hero _hero;
    private Bullet _bullet;
    private EnemySpawnpoint _spawn;

    // Start is called before the first frame update
    void Start()
    {
        _rig = GetComponent<Rigidbody2D>();
        Application.targetFrameRate = 60;

        _hero = FindAnyObjectByType<Hero>();//Hero����Hero�Ƃ������O�̃X�N���v�g������Ă���
        _spawn = FindAnyObjectByType<EnemySpawnpoint>();
    }

    // Update is called once per frame
    void Update()
    {
        _rig.velocity = new Vector3(E_speed, 0);//�ړ�
        if (_spawn._remainTime <= 0)//�E�F�[�u�c�莞�Ԃ�0�b�����̎�
        {
            Destroy(this.gameObject);//�j�󂷂�
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bullet")//�e�ɓ���������
        {
            _bullet = FindAnyObjectByType<Bullet>();//�e�̃X�N���v�g�̏����Ƃ��Ă���
            E_hp = E_hp - _bullet.bulletAttackPower;//HP��e�̍U���͕����炷
            Destroy(collision.gameObject); //�e�ۏ���
            EnemyDeath();
        }
        if (collision.gameObject.tag == "Pulse")
        {
            _bullet = FindAnyObjectByType<Bullet>();
            E_hp = E_hp - _bullet.bulletAttackPower;
            EnemyDeath();
        }
        if (collision.gameObject.tag == "Player")
        {
            _hero.H_HP -= E_attack;//Hero��HP���U���͕����炷
            E_hp -= E_hp;//HP��0�ɂ���
            Debug.Log($"remain {_hero.H_HP} player health");
            _hero.SetHP();
            EnemyDeath();
        }
        if (collision.gameObject.tag == "Respawn")//�ƂɐG�ꂽ��
        {
            Destroy(this.gameObject);//�j�󂷂�
        }
    }
    /// <summary>
    /// �G�����񂾂Ƃ��̏���
    /// </summary>
    private void EnemyDeath()
    {
        if (E_hp <= 0)
        {
            Debug.Log($"death {gameObject.name}");
            Destroy(this.gameObject);
            _hero.AddGold(E_Gold);//�S�[���h���₷
        }
        else
        {
            Debug.Log($"remain {gameObject.name} health : {E_hp}");//�c��HP�\��
        }
    }
}
