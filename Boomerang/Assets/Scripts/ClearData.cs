using System.Collections;
using System.Collections.Generic;

/// <summary>
/// �N���A�󋵕ۑ�
/// </summary>
public class ClearData
{
    /// <summary>
    /// �N���A�󋵂̃��X�g
    /// </summary>
    private static List<bool> clear;

    /// <summary>
    /// �X�e�[�W���N���A�ς݂��𔻒肷��
    /// </summary>
    /// <param name="stageNumber"></param>
    /// <returns></returns>
    public static bool IsCleared(int stageNumber)
    {
        return clear[stageNumber];
    }
    /// <summary>
    /// �X�e�[�W�̃N���A�󋵂��X�V����
    /// </summary>
    /// <param name="stageNumber"></param>
    public static void SetClear(int stageNumber)
    {
        clear[stageNumber] = true;
    }
    /// <summary>
    /// ���X�g������
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
