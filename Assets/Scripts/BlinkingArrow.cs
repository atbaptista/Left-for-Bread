using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingArrow : MonoBehaviour
{
    private bool _isOn = false;
    private SpriteRenderer _spr;

    private void Start()
    {
        _spr = this.GetComponent<SpriteRenderer>();
    }


    public void StartBlinking()
    {
        InvokeRepeating("Blink", 0, 1);
    }

    private void Blink()
    {
        if (_isOn)
        {
            _spr.color = new Color(_spr.color.r, _spr.color.g, _spr.color.b, 0);
            _isOn = false;
        }
        else
        {
            _spr.color = new Color(_spr.color.r, _spr.color.g, _spr.color.b, 255);
            _isOn = true;
        }
        
    }
}
