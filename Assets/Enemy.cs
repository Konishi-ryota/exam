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

        _hero = FindAnyObjectByType<Hero>();//HeroからHeroという名前のスクリプトを取ってくる
        _spawn = FindAnyObjectByType<EnemySpawnpoint>();
    }

    // Update is called once per frame
    void Update()
    {
        _rig.velocity = new Vector3(E_speed, 0);//移動
        if (_spawn._remainTime <= 0)//ウェーブ残り時間が0秒未満の時
        {
            Destroy(this.gameObject);//破壊する
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bullet")//弾に当たったら
        {
            _bullet = FindAnyObjectByType<Bullet>();//弾のスクリプトの情報をとってくる
            E_hp = E_hp - _bullet.bulletAttackPower;//HPを弾の攻撃力分減らす
            Destroy(collision.gameObject); //弾丸消す
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
            _hero.H_HP -= E_attack;//HeroのHPを攻撃力文減らす
            E_hp -= E_hp;//HPを0にする
            Debug.Log($"remain {_hero.H_HP} player health");
            _hero.SetHP();
            EnemyDeath();
        }
        if (collision.gameObject.tag == "Respawn")//家に触れたら
        {
            Destroy(this.gameObject);//破壊する
        }
    }
    /// <summary>
    /// 敵が死んだときの処理
    /// </summary>
    private void EnemyDeath()
    {
        if (E_hp <= 0)
        {
            Debug.Log($"death {gameObject.name}");
            Destroy(this.gameObject);
            _hero.AddGold(E_Gold);//ゴールド増やす
        }
        else
        {
            Debug.Log($"remain {gameObject.name} health : {E_hp}");//残りHP表示
        }
    }
}
