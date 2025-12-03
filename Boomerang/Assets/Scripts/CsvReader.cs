using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;

/// <summary>
/// プラットフォームを考慮してStreamingAssetsからCSVファイルを読み込む静的ユーティリティクラス
/// </summary>
public static class CsvReader
{
    /// <summary>
    /// 指定されたCSVファイルを非同期で読み込み、内容をstringのジャグ配列（行と列）として返す
    /// </summary>
    /// <param name="csvFileName">StreamingAssetsフォルダ内のCSVファイル名 (例: "data.csv")</param>
    /// <returns>CSVデータを格納したジャグ配列 string[][]。読み込み失敗時はnullを返す。</returns>
    public static async Task<string[][]> LoadCsvData(string csvFileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, csvFileName);
        string csvText = "";

#if UNITY_ANDROID && !UNITY_EDITOR
        Debug.Log("Android platform: Reading CSV with UnityWebRequest.");
        UnityWebRequest www = UnityWebRequest.Get(filePath);
        var operation = www.SendWebRequest();

        // リクエストが完了するまで待機
        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (www.result == UnityWebRequest.Result.Success)
        {
            csvText = www.downloadHandler.text;
        }
        else
        {
            Debug.LogError($"Failed to load CSV file '{csvFileName}': {www.error}");
            return null;
        }
#else
        Debug.Log($"PC/Other platform: Reading CSV with File.ReadAllTextAsync.");
        if (!File.Exists(filePath))
        {
            Debug.LogError($"File not found at path: {filePath}");
            return null;
        }

        try
        {
            // 同期的なファイル読み込みをバックグラウンドスレッドで実行し、メインスレッドのブロッキングを防ぐ
            csvText = await Task.Run(() => File.ReadAllText(filePath));
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load CSV file '{csvFileName}': {e.Message}");
            return null;
        }
#endif
        return ParseCsv(csvText);
    }

    /// <summary>
    /// CSV形式の文字列を解析し、stringのジャグ配列に変換する
    /// </summary>
    /// <param name="csvText">解析対象のCSV文字列</param>
    /// <returns>解析後のジャグ配列 string[][]</returns>
    private static string[][] ParseCsv(string csvText)
    {
        if (string.IsNullOrEmpty(csvText))
        {
            Debug.LogWarning("CSV data is empty or null.");
            return new string[0][]; // 空の配列を返す
        }

        // 改行で各行に分割（空の行は無視する）
        string[] lines = csvText.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        List<string[]> data = new List<string[]>();

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            // ダブルクォートで囲まれたカンマを無視する正規表現でフィールドに分割
            string[] fields = Regex.Split(line.Trim(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
            
            // 各フィールドの前後の空白とダブルクォートを削除
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = fields[i].Trim().Trim('"');
            }
            data.Add(fields);
        }

        return data.ToArray();
    }
}