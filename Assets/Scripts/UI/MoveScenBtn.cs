using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScenBtn : MonoBehaviour
{
    private void Start()
    {
        if(GameManager.Instance == null)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 클릭 시
    /// </summary>
    /// <param name="argSceneName">씬 이름</param>
    public void Click(string argSceneName)
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.MoveSceneAsName(argSceneName, false);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
