using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Q�[�������ݒ�
/// </summary>
public class Initializer : MonoBehaviour
{
    /// <summary>
    /// �������ς݂̂Ƃ�true
    /// </summary>
    static private bool initialized;

    /// <summary>
    /// ���g���C�A���ɐi�ނ�I�񂾏ꍇtrue�ɂ���
    /// </summary>
    static private bool retry;


    /// <summary>
    /// �����O�̉摜���X�g
    /// </summary>
    static private List<Sprite> ringList;

    /// <summary>
    /// �����O�̉摜���擾����
    /// </summary>
    /// <param name="power">�����O�̃p���[</param>
    /// <returns>�����O�̉摜(Sprite)</returns>
    static public Sprite GetRingImg(int power)
    {
        return ringList[power - 1];
    }
    /// <summary>
    /// ���g���C������X�V����
    /// </summary>
    static public void SetRetry(bool goRetry)
    {
        retry = goRetry;
    }
    /// <summary>
    /// ���g���C������擾����
    /// </summary>
    static public bool GetRetry()
    {
        return retry;
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = func.FRAMERATE;
        ClearData.Initialize();
        initialized = false;

        ringList = new List<Sprite>
        {
            Resources.Load<Sprite>("ring_01"),
            Resources.Load<Sprite>("ring_02"),
            Resources.Load<Sprite>("ring_03"),
            Resources.Load<Sprite>("ring_04"),
            Resources.Load<Sprite>("ring_05"),
            Resources.Load<Sprite>("ring_06"),
            Resources.Load<Sprite>("ring_07"),
            Resources.Load<Sprite>("ring_08"),
            Resources.Load<Sprite>("ring_09"),
            Resources.Load<Sprite>("ring_10")
        };
    }

    // Update is called once per frame
    void Update()
    {
        if(!initialized)
        {
            Fader.SetFader(0, true, "Title");
            initialized = true;
        }
        
    }
}
