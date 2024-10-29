using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertPanel : MonoBehaviour
{
    /// <summary>
    /// play alert animation
    /// </summary>
    /// <param name="argAlertStr">alert text</param>
    public void Alert(string argAlertStr)
    {
        Debug.Log("in");
        GetComponentInChildren<Text>().text =
            argAlertStr;
    }

    /// <summary>
    /// destroy this object over animation
    /// </summary>
    public void AniOver()
    {
        Destroy(gameObject);
    }
}
