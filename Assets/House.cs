using UnityEngine;
using UnityEngine.UI;

public class House : MonoBehaviour
{
    [SerializeField] Text HouseHPText;
    [SerializeField] int MaxHouseHP;
    private int _enemyIn;

    void Start()
    {
        SetHouseHP();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            _enemyIn++;
            SetHouseHP();
        }
    }
    private void SetHouseHP()
    {
        HouseHPText.text = "Žc‚è‚Ì‘Ï‹v’l " + $"{MaxHouseHP - _enemyIn}";
    }
}
