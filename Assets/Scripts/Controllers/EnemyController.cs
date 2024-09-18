using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    /// <summary>
    /// 플레이어 위치
    /// </summary>
    private Transform m_playerTransform = null;

    /// <summary>
    /// 위치 제한 정보
    /// </summary>
    private float m_minXPos = -10.0f;
    private float m_maxXPos = 10.0f;
    private float m_minYPos = 10.0f;
    private float m_maxYPos = 30.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _pos = new Vector3();
        _pos.x = Mathf.Clamp(m_playerTransform.localPosition.x, m_minXPos, m_maxXPos);
        _pos.y = Mathf.Clamp(m_playerTransform.localPosition.y, m_minYPos, m_maxYPos);

        transform.position = _pos;
    }
}
