using UnityEngine;

public class HealItem : MonoBehaviour
{
    [SerializeField] int HouseHealPower;
    [SerializeField] int HeroHealPower;

    private House _house;
    private Hero _hero;
    // Start is called before the first frame update
    void Start()
    {
        _house = FindAnyObjectByType<House>();
        _hero = FindAnyObjectByType<Hero>();
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject,10);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _house.HouseHP += HouseHealPower;
            _hero.H_HP += HeroHealPower;
            Destroy(this.gameObject);
            _house.SetHouseHP();
            _hero.SetHP();
            Debug.Log($"{_house.HouseHP}");
        }
    }
}
