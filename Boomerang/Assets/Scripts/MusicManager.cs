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
        /// <summary>�C���Q�[��</summary>
        Stage,
        /// <summary>�I�[�v�j���O</summary>
        Opening,
        /// <summary>�Q�[���I�[�o�[</summary>
        GameOver,
        /// <summary>�Q�[���N���A</summary>
        GameClear,
    };
    /// <summary>
    /// BGM���X�g�̐�
    /// </summary>
    private const int BGMNum = 4;
    /// <summary>
    /// BGM���X�g
    /// </summary>
    private List<AudioClip> bgmList;
    /// <summary>
    /// BGM�����[�v���邩
    /// </summary>
    private List<bool> bgmLoop;
    /// <summary>
    /// ���[�v�I�[�̃T���v����
    /// </summary>
    private List<int> LoopEnd;
    /// <summary>
    /// ���[�v�n�_�̃T���v����
    /// </summary>
    private List<int> LoopStart;
    /// <summary>
    /// ���t����BGM�ԍ�
    /// </summary>
    private int playing;

    /// <summary>
    /// BGM��炷
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
