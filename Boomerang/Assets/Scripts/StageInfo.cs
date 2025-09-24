using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// �X�e�[�W���ǂݍ���/�Ăяo��
/// </summary>
public class StageInfo : MonoBehaviour
{
    /// <summary>
    /// ��舵�����̐�
    /// </summary>
    private const int ContentNum = 8;
    /// <summary>
    /// �I�u�W�F�N�g�̎��
    /// </summary>
    public enum ObjSort {
        /// <summary>�ʏ�G</summary>
        Enemy,
        /// <summary>�{�X</summary>
        Boss,
        /// <summary>�����O</summary>
        Ring,
        /// <summary>�N���X�^��</summary>
        Crystal,
        /// <summary>�����̉ʎ�</summary>
        Fruit,
    };
    /// <summary>
    /// �I�u�W�F�N�g���
    /// </summary>
    public struct ObjInfo
    {
        /// <summary>�ǂݍ��񂾐�</summary>
        public int loaderIndex;
        /// <summary>���</summary>
        public List<ObjSort> sort;
        /// <summary>�q�b�g�|�C���g</summary>
        public List<int> hp;
        /// <summary>����</summary>
        public List<Enemy.Element> element;
        /// <summary>�U����</summary>
        public List<int> atk;
        /// <summary>�s���^�[���E�����ɂ�����^�[��</summary>
        public List<int> turn;
        /// <summary>x���W</summary>
        public List<float> x;
        /// <summary>y���W</summary>
        public List<float> y;
        /// <summary>�T�C�Y</summary>
        public List<int> size;
        /// <summary>�R���X�g���N�^</summary>
        public ObjInfo(bool constructed = true)
        {
            loaderIndex = 0;
            sort = new List<ObjSort>();
            hp = new List<int>();
            element = new List<Enemy.Element>();
            atk = new List<int>();
            turn = new List<int>();
            x = new List<float>();
            y = new List<float>();
            size = new List<int>();
        }
    };
    /// <summary>
    /// �t���A���Ƃ̃I�u�W�F�N�g���
    /// </summary>
    public ObjInfo[] objInfo;
    /// <summary>
    /// �Ō�̃t���A
    /// </summary>
    private int lastFloor;
    /// <summary>
    /// �ǂݍ��񂾃X�e�[�W�̔ԍ�
    /// </summary>
    private int stage;

    static public float xcalc(float a)
    {
        return (a) / 45 * func.SCW / func.SCH * func.camHeight * 2 - func.SCW / func.SCH * func.camHeight;
    }
    static public float ycalc(float a)
    {
        return (a - 80) / 80 * func.camHeight * 2 + func.metrecalc(10);
    }
    /// <summary>
    /// ��ނ����蓖�Ă�
    /// </summary>
    /// <param name="s">������</param>
    /// <returns>���</returns>
    private ObjSort AssignSort(string s)
    {
        ObjSort res;
        if(s == "enemy")
        {
            res = ObjSort.Enemy;
        }
        else if(s == "boss")
        {
            res = ObjSort.Boss;
        }
        else if(s == "ring")
        {
            res = ObjSort.Ring;
        }
        else if(s == "crystal")
        {
            res = ObjSort.Crystal;
        }
        else if(s == "fruit")
        {
            res = ObjSort.Fruit;
        }
        else
        {
            res = ObjSort.Enemy;
        }
        return res;
    }
    /// <summary>
    /// ���������蓖�Ă�
    /// </summary>
    /// <param name="s">������</param>
    /// <returns>����</returns>
    private Enemy.Element AssignElement(string s)
    {
        Enemy.Element res;
        if(s == "fire")
        {
            res = Enemy.Element.Fire;
        }
        else if(s == "aqua")
        {
            res = Enemy.Element.Aqua;
        }
        else
        {
            res = Enemy.Element.Leaf;
        }
        return res;
    }
    /// <summary>
    /// �T�C�Y�𐔒l���ɕϊ�����
    /// </summary>
    /// <param name="s">������</param>
    /// <returns>�T�C�Y�̔ԍ�</returns>
    private int AssignSize(string s)
    {
        int res;
        if(s == "A")
        {
            res = 0;
        }
        else if(s == "B")
        {
            res = 1;
        }
        else if(s == "C")
        {
            res = 2;
        }
        else if(s == "D")
        {
            res = 3;
        }
        else
        {
            res = 3;
        }
        return res;
    }
    /// <summary>
    /// �X�e�[�W�̃I�u�W�F�N�g����csv�t�@�C������ǂݍ���
    /// </summary>
    /// <param name="stagenum">�X�e�[�W�ԍ�</param>
    public void LoadStageInfo(int stagenum)
    {
        DeleteInfo();
        stage = stagenum;

        string path = Application.streamingAssetsPath;
        // �ǂݍ��݂���CSV�t�@�C���̃p�X���w�肵�ĊJ��
        StreamReader reader;
        switch(stagenum)
        {
        case 0:
            reader = new StreamReader(path + "/stagedata_01.csv");
            break;
        case 1:
            reader = new StreamReader(path + "/stagedata_02.csv");
            break;
        case 2:
            reader = new StreamReader(path + "/stagedata_03.csv");
            break;
        default:
            reader = new StreamReader("Assets/Resources/stagedata_01.csv");
            break;
        }
        bool header = true;
        
        // �����܂ŌJ��Ԃ�
        while(!reader.EndOfStream)
        {
            // CSV�t�@�C���̈�s��ǂݍ���
            string line = reader.ReadLine();
            if(header)
            {
                string[] values = line.Split(',');
                if(values[0] == "EndOfHeader") header = false;
            }
            else
            {
                // �ǂݍ��񂾈�s���J���}���ɕ����Ĕz��Ɋi�[����
                string[] values = line.Split(',');
                int floor;
                if(int.TryParse(values[0], out floor))
                {
                    int nbuffer;
                    float fbuffer;
                    objInfo[floor].sort.Add(AssignSort(values[1]));
                    if(int.TryParse(values[2], out nbuffer))
                    {
                        objInfo[floor].hp.Add(nbuffer);
                    }
                    else
                    {
                        objInfo[floor].hp.Add(0);
                    }
                    objInfo[floor].element.Add(AssignElement(values[3]));
                    if(int.TryParse(values[4], out nbuffer))
                    {
                        objInfo[floor].atk.Add(nbuffer);
                    }
                    else
                    {
                        objInfo[floor].atk.Add(0);
                    }
                    if(int.TryParse(values[5], out nbuffer))
                    {
                        objInfo[floor].turn.Add(nbuffer);
                    }
                    else
                    {
                        objInfo[floor].turn.Add(0);
                    }
                    if(float.TryParse(values[6], out fbuffer))
                    {
                        fbuffer = xcalc(fbuffer);
                        objInfo[floor].x.Add(fbuffer);
                    }
                    else
                    {
                        objInfo[floor].x.Add(0);
                    }
                    if(float.TryParse(values[7], out fbuffer))
                    {
                        fbuffer = ycalc(fbuffer);
                        objInfo[floor].y.Add(fbuffer);
                    }
                    else
                    {
                        objInfo[floor].y.Add(0);
                    }
                    objInfo[floor].size.Add(AssignSize(values[8]));
                    objInfo[floor].loaderIndex++;
                }
            }
        }
        lastFloor = 0;
        for(int i = 1; i < BattleManager.MaxFloor; i++)
        {
            if(lastFloor < i && objInfo[i].loaderIndex > 0) lastFloor = i;
        }
    }
    /// <summary>
    /// �Y���t���A�̃I�u�W�F�N�g�����擾����
    /// </summary>
    /// <param name="floor">�t���A��</param>
    /// <returns>�I�u�W�F�N�g���</returns>
    public ObjInfo GetStageInfo(int floor)
    {
        if(floor == BattleManager.InitialFloor)
        {
            GameObject.Find("ClearTx").GetComponent<ClearTx>().SetStage(stage);
        }
        return objInfo[floor];
    }
    /// <summary>
    /// �Ō�̃t���A�̔ԍ����擾����
    /// </summary>
    /// <returns>�Ō�̃t���A�̔ԍ�</returns>
    public int GetLastFloorNumber()
    {
        return lastFloor;
    }
    /// <summary>
    /// �ǂݍ��񂾃I�u�W�F�N�g����j������
    /// </summary>
    public void DeleteInfo()
    {
        objInfo = null;
        objInfo = new ObjInfo[BattleManager.MaxFloor];
        for(int i = 0; i < BattleManager.MaxFloor; i++)
        {
            objInfo[i] = new ObjInfo(true);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        objInfo = new ObjInfo[BattleManager.MaxFloor];
        for(int i = 0; i < BattleManager.MaxFloor; i++)
        {
            objInfo[i] = new ObjInfo(true);
        }
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
