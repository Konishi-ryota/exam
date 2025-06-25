using UnityEngine;

public class HealItem : MonoBehaviour
{
    [SerializeField] int HouseHealPower;
    [SerializeField] int HeroHealPower;
    [SerializeField] int _beforeRemove = 10;

    private House _house;
    private Hero _hero;
    // Start is called before the first frame update
    void Start()
    {
        _house = FindAnyObjectByType<House>();
        _hero = FindAnyObjectByType<Hero>();
        Destroy(gameObject, _beforeRemove);//n秒後に消す
    }
    private void OnTriggerEnter2D(Collider2D collision)//isTriggerしてるのでTriggerEnter
    {
        if (collision.gameObject.tag == "Player")//プレイヤーと当たった時
            //タグ使ってるコードを先輩に見せると大体「タグ使ってんのか...」って言われる
            //他にどんなやり方があるのかは調べ得そう、このやり方タイピングミスって死ぬ
        {
            _house.HouseHP += HouseHealPower;
            _hero.H_HP += HeroHealPower;//プレイヤーと家のHPを増やす
            Destroy(this.gameObject);
            _house.SetHouseHP();
            _hero.SetHP();//それぞれの体力管理メソッド呼び出し
            Debug.Log($"{_house.HouseHP}");
        }
    }
}
