
using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using Unity.VisualScripting;

public class PlatformSpecificCsvReader : MonoBehaviour
{
    private string csvFileName = "stagedata_01.csv";

    // Startメソッドはオブジェクトがアクティブになった最初のフレームで一度だけ呼び出されます。
    void Start()
    {
        StartCoroutine(LoadCsvFile());
    }

    IEnumerator LoadCsvFile()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, csvFileName);

//#if UNITY_ANDROID
        Debug.Log("Androidビルド: UnityWebRequestでCSVを読み込みます。");
        UnityWebRequest www = UnityWebRequest.Get(filePath);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            ParseCsvData(www.downloadHandler.text);
        }
        else
        {
            Debug.LogError("CSVファイルの読み込みに失敗しました: " + www.error);
        }
        /*
#else
        Debug.Log("PC/その他ビルド: File.ReadAllTextでCSVを読み込みます。");
        try
        {
            string csvText = File.ReadAllText(filePath);
            ParseCsvData(csvText);
        }
        catch(System.Exception e)
        {
            Debug.LogError("CSVファイルの読み込みに失敗しました: " + e.Message);
        }
#endif
        */
    }

    void ParseCsvData(string csvText)
    {
        if(string.IsNullOrEmpty(csvText))
        {
            Debug.LogWarning("CSVデータが空です。");
            return;
        }

        string[] lines = csvText.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        foreach(string line in lines)
        {
            string[] fields = Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
            foreach(string field in fields)
            {
                //Debug.Log(field.Trim().Trim('"'));
            }
        }
    }
}
