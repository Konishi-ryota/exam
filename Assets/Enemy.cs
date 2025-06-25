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

        _hero = FindAnyObjectByType<Hero>();
        _spawn = FindAnyObjectByType<EnemySpawnpoint>();
        if (!_hero)
        {
            Debug.LogWarning("hero is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        _rig.velocity = new Vector3(E_speed, 0);
        if (_spawn._remainTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "bullet")
        {
            _bullet = FindAnyObjectByType<Bullet>();
            E_hp = E_hp - _bullet.bulletAttackPower;
            Destroy(collision.gameObject); //íeä€è¡Ç∑
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
            _hero.H_HP -= E_attack;
            E_hp -=E_hp;
            Debug.Log($"remain {_hero.H_HP} player health");
            _hero.SetHP();
            EnemyDeath();
        }
        if (collision.gameObject.tag == "Respawn")
        {
            Destroy(this.gameObject);
        }
    }
    private void EnemyDeath()
    {
        if (E_hp <= 0)
        {
            Debug.Log($"death {gameObject.name}");
            Destroy(this.gameObject);
            _hero.AddGold(E_Gold);//ÉSÅ[ÉãÉhëùÇ‚Ç∑
        }
        else
        {
            Debug.Log($"remain {gameObject.name} health : {E_hp}");
        }
    }
}
