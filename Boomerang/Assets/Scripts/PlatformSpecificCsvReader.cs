
using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using Unity.VisualScripting;

public class PlatformSpecificCsvReader : MonoBehaviour
{
    private string csvFileName = "stagedata_01.csv";

    // Start���\�b�h�̓I�u�W�F�N�g���A�N�e�B�u�ɂȂ����ŏ��̃t���[���ň�x�����Ăяo����܂��B
    void Start()
    {
        StartCoroutine(LoadCsvFile());
    }

    IEnumerator LoadCsvFile()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, csvFileName);

//#if UNITY_ANDROID
        Debug.Log("Android�r���h: UnityWebRequest��CSV��ǂݍ��݂܂��B");
        UnityWebRequest www = UnityWebRequest.Get(filePath);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            ParseCsvData(www.downloadHandler.text);
        }
        else
        {
            Debug.LogError("CSV�t�@�C���̓ǂݍ��݂Ɏ��s���܂���: " + www.error);
        }
        /*
#else
        Debug.Log("PC/���̑��r���h: File.ReadAllText��CSV��ǂݍ��݂܂��B");
        try
        {
            string csvText = File.ReadAllText(filePath);
            ParseCsvData(csvText);
        }
        catch(System.Exception e)
        {
            Debug.LogError("CSV�t�@�C���̓ǂݍ��݂Ɏ��s���܂���: " + e.Message);
        }
#endif
        */
    }

    void ParseCsvData(string csvText)
    {
        if(string.IsNullOrEmpty(csvText))
        {
            Debug.LogWarning("CSV�f�[�^����ł��B");
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
