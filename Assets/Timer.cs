using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] int _StageTimer = 30;
    [SerializeField] Text timerText; 

    private float _timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        int remaining = _StageTimer - (int)_timer;
        timerText.text = remaining.ToString("D2");
    }
}
