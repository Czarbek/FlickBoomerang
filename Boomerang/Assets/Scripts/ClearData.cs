using System.Collections;
using System.Collections.Generic;

/// <summary>
/// クリア状況保存
/// </summary>
public class ClearData
{
    /// <summary>
    /// クリア状況のリスト
    /// </summary>
    private static List<bool> clear;

    /// <summary>
    /// ステージがクリア済みかを判定する
    /// </summary>
    /// <param name="stageNumber"></param>
    /// <returns></returns>
    public static bool IsCleared(int stageNumber)
    {
        return clear[stageNumber];
    }
    /// <summary>
    /// ステージのクリア状況を更新する
    /// </summary>
    /// <param name="stageNumber"></param>
    public static void SetClear(int stageNumber)
    {
        clear[stageNumber] = true;
    }
    /// <summary>
    /// リスト初期化
    /// </summary>
    public static void Initialize()
    {
        clear = new List<bool>();
        for(int i = 0; i < DiffButton.StageNum; i++)
        {
            clear.Add(false);
        }
    }
}
