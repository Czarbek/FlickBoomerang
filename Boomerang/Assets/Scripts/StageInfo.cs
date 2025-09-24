using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// ステージ情報読み込み/呼び出し
/// </summary>
public class StageInfo : MonoBehaviour
{
    /// <summary>
    /// 取り扱う情報の数
    /// </summary>
    private const int ContentNum = 8;
    /// <summary>
    /// オブジェクトの種類
    /// </summary>
    public enum ObjSort {
        /// <summary>通常敵</summary>
        Enemy,
        /// <summary>ボス</summary>
        Boss,
        /// <summary>リング</summary>
        Ring,
        /// <summary>クリスタル</summary>
        Crystal,
        /// <summary>黄金の果実</summary>
        Fruit,
    };
    /// <summary>
    /// オブジェクト情報
    /// </summary>
    public struct ObjInfo
    {
        /// <summary>読み込んだ数</summary>
        public int loaderIndex;
        /// <summary>種類</summary>
        public List<ObjSort> sort;
        /// <summary>ヒットポイント</summary>
        public List<int> hp;
        /// <summary>属性</summary>
        public List<Enemy.Element> element;
        /// <summary>攻撃力</summary>
        public List<int> atk;
        /// <summary>行動ターン・復活にかかるターン</summary>
        public List<int> turn;
        /// <summary>x座標</summary>
        public List<float> x;
        /// <summary>y座標</summary>
        public List<float> y;
        /// <summary>サイズ</summary>
        public List<int> size;
        /// <summary>コンストラクタ</summary>
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
    /// フロアごとのオブジェクト情報
    /// </summary>
    public ObjInfo[] objInfo;
    /// <summary>
    /// 最後のフロア
    /// </summary>
    private int lastFloor;
    /// <summary>
    /// 読み込んだステージの番号
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
    /// 種類を割り当てる
    /// </summary>
    /// <param name="s">文字列</param>
    /// <returns>種類</returns>
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
    /// 属性を割り当てる
    /// </summary>
    /// <param name="s">文字列</param>
    /// <returns>属性</returns>
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
    /// サイズを数値情報に変換する
    /// </summary>
    /// <param name="s">文字列</param>
    /// <returns>サイズの番号</returns>
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
    /// ステージのオブジェクト情報をcsvファイルから読み込む
    /// </summary>
    /// <param name="stagenum">ステージ番号</param>
    public void LoadStageInfo(int stagenum)
    {
        DeleteInfo();
        stage = stagenum;

        string path = Application.streamingAssetsPath;
        // 読み込みたいCSVファイルのパスを指定して開く
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
        
        // 末尾まで繰り返す
        while(!reader.EndOfStream)
        {
            // CSVファイルの一行を読み込む
            string line = reader.ReadLine();
            if(header)
            {
                string[] values = line.Split(',');
                if(values[0] == "EndOfHeader") header = false;
            }
            else
            {
                // 読み込んだ一行をカンマ毎に分けて配列に格納する
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
    /// 該当フロアのオブジェクト情報を取得する
    /// </summary>
    /// <param name="floor">フロア数</param>
    /// <returns>オブジェクト情報</returns>
    public ObjInfo GetStageInfo(int floor)
    {
        if(floor == BattleManager.InitialFloor)
        {
            GameObject.Find("ClearTx").GetComponent<ClearTx>().SetStage(stage);
        }
        return objInfo[floor];
    }
    /// <summary>
    /// 最後のフロアの番号を取得する
    /// </summary>
    /// <returns>最後のフロアの番号</returns>
    public int GetLastFloorNumber()
    {
        return lastFloor;
    }
    /// <summary>
    /// 読み込んだオブジェクト情報を破棄する
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
