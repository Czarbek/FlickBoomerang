using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// フロア遷移時のフロア数表示
/// </summary>
public class FloorCount : MonoBehaviour
{
    /// <summary>
    /// TextMeshProUGUIコンポーネント
    /// </summary>
    TextMeshProUGUI tmpro;
    /// <summary>
    /// テキストを表示する
    /// </summary>
    /// <param name="currentFloorNumber">現在のフロア数</param>
    /// <param name="lastFloorNumber">最終フロア</param>
    public void SetText(int currentFloorNumber, int lastFloorNumber)
    {
        tmpro.text = "Battle " + currentFloorNumber + "/" + lastFloorNumber;
    }
    /// <summary>
    /// テキストを非表示にする
    /// </summary>
    public void DeleteText()
    {
        tmpro.text = "";
    }
    // Start is called before the first frame update
    void Start()
    {
        tmpro = GetComponent<TextMeshProUGUI>();
        tmpro.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
