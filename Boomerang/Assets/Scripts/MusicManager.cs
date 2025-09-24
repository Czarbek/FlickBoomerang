using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    /// <summary>
    /// BGM
    /// </summary>
    public enum BGM
    {
        /// <summary>インゲーム</summary>
        Stage,
        /// <summary>オープニング</summary>
        Opening,
        /// <summary>ゲームオーバー</summary>
        GameOver,
        /// <summary>ゲームクリア</summary>
        GameClear,
    };
    /// <summary>
    /// BGMリストの数
    /// </summary>
    private const int BGMNum = 4;
    /// <summary>
    /// BGMリスト
    /// </summary>
    private List<AudioClip> bgmList;
    /// <summary>
    /// BGMをループするか
    /// </summary>
    private List<bool> bgmLoop;
    /// <summary>
    /// ループ終端のサンプル数
    /// </summary>
    private List<int> LoopEnd;
    /// <summary>
    /// ループ始点のサンプル数
    /// </summary>
    private List<int> LoopStart;
    /// <summary>
    /// 演奏中のBGM番号
    /// </summary>
    private int playing;

    /// <summary>
    /// BGMを鳴らす
    /// </summary>
    /// <param name="bgm"></param>
    public void PlayMusic(BGM bgm)
    {
        if(playing >= 0)
        {
            GetComponent<AudioSource>().Stop();
        }
        playing = (int)bgm;
        GetComponent<AudioSource>().PlayOneShot(bgmList[playing]);
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        bgmList = new List<AudioClip>
        {
            (AudioClip)Resources.Load("bgm_01"),
            (AudioClip)Resources.Load("bgm_02"),
            (AudioClip)Resources.Load("bgm_03"),
            (AudioClip)Resources.Load("bgm_04")
        };
        bgmLoop = new List<bool>
        {
            true,
            true,
            false,
            false,
        };
        LoopEnd = new List<int>
        {
            //26292222,
            1000000,
            4324816,
            0,
            0,

        };
        LoopStart = new List<int>
        {
            22401,
            292095,
            0,
            0,
        };

        playing = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if(playing >= 0)
        {
            Debug.Log(GetComponent<AudioSource>().timeSamples);
            if(GetComponent<AudioSource>().timeSamples >= LoopEnd[playing] && bgmLoop[playing])
            {
                Debug.Log("loop");
                GetComponent<AudioSource>().timeSamples -= LoopEnd[playing] - LoopStart[playing];
            }
        }
    }
}
