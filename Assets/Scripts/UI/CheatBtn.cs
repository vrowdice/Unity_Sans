using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatBtn : MonoBehaviour
{
    private int _cheaterCount = 0;

    public void Click()
    {
        _cheaterCount++;
        GameManager.Instance.Alert("Cheater Count: " + _cheaterCount);
        PlayerController _playerController = null;
        try
        {
            _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        }
        catch
        {

        }
        
        if (_playerController != null)
        {
            _playerController.Recover(100, true);
        }
    }
}
