using UnityEngine;
using UnityEngine.UI;

public class MoneyPanel : MonoBehaviour
{
    /// <summary>
    /// µ∑ ≈ÿΩ∫∆Æ
    /// </summary>
    [SerializeField]
    Text m_moneyText = null;

    public string SetMoneyText
    {
        get { return m_moneyText.text; }
        set
        {
            m_moneyText.text = value;
        }
    }
}
