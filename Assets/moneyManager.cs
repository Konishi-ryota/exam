using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class goldManager : MonoBehaviour
{
    [SerializeField] private int firstgold;
    [SerializeField] private Text _goldText;
    private int _gold;

    private void Start()
    {
        _gold = firstgold;
        SetGoldText();
    }
    private void SetGoldText()
    { 
        _goldText.text = _gold.ToString();
    }


    public void Addgold(int gold)
    {
        _gold += gold;
        SetGoldText();
    }
    public void DecreaseMoney(int money)
    {
        _gold -= money;
        if (_gold < 0)
        {
            _gold = 0;
        }
        SetGoldText();
    }
}
