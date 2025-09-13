using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテム
/// </summary>
public class Item : MonoBehaviour
{
    /// <summary>
    /// アイテムの種類一覧
    /// </summary>
    public enum ItemSort
    {
        /// <summary>リング</summary>
        Ring,
        /// <summary>クリスタル</summary>
        Crystal,
        /// <summary>黄金の果実</summary>
        Fruit,
    };
    /// <summary>
    /// アイテムの種類
    /// </summary>
    public ItemSort sort;
    /// <summary>
    /// x座標
    /// </summary>
    public float x;
    /// <summary>
    /// y座標
    /// </summary>
    public float y;
    /// <summary>
    /// 当たり判定の半径
    /// </summary>
    public float CollisionRadius;
    /// <summary>
    /// サイズ
    /// </summary>
    public int sizePattern;
    /// <summary>
    /// パワーの最大値
    /// </summary>
    private const int MaxPower = 10;
    /// <summary>
    /// 初期時点でのパワー
    /// </summary>
    public int InitialPower;
    /// <summary>
    /// 現在時点でのパワー
    /// </summary>
    private int power;
    /// <summary>
    /// クリスタルの属性
    /// </summary>
    public Enemy.Element element;
    /// <summary>
    /// 黄金の果実のHP回復割合
    /// </summary>
    public float cureRate;
    /// <summary>
    /// 経過ターン
    /// </summary>
    public int turnCount;
    /// <summary>
    /// 復活にかかるターン
    /// </summary>
    private int validationTurn;
    /// <summary>
    /// リング復活にかかるターン
    /// </summary>
    private const int RingValidationTurn = 1;
    /// <summary>
    /// クリスタル復活にかかるターン
    /// </summary>
    private const int CrystalValidationTurn = 3;
    /// <summary>
    /// 黄金の果実復活にかかるターン
    /// </summary>
    private const int FruitValidationTurn = 3;
    /// <summary>
    /// <summary>
    /// 取得可能かどうか
    /// </summary>
    public bool valid;
    /// <summary>
    /// プレイヤーにヒットしたときの処理
    /// </summary>
    public void Hit()
    {
        switch(sort)
        {
        case ItemSort.Ring:
            GameObject.Find("Player").GetComponent<Player>().AddPower(power);
            power = 0;
            break;
        case ItemSort.Crystal:
            GameObject.Find("Player").GetComponent<Player>().SetElement(element);
            break;
        case ItemSort.Fruit:
            GameObject.Find("Player").GetComponent<Player>().HPCure(cureRate);
            break;
        }
        turnCount = 0;
        valid = false;
    }
    /// <summary>
    /// 敵ターン終了時処理
    /// </summary>
    public void EndTurn()
    {
        if(valid)
        {
            if(sort == ItemSort.Ring)
            {
                if(power<MaxPower) power++;
            }
        }
        else
        {
            turnCount++;
            if(turnCount == validationTurn)
            {
                power = 1;
                turnCount = 0;
                valid = true;
            }
        }
    }
    /// <summary>
    /// 当たり判定の半径を取得する
    /// </summary>
    /// <returns>当たり判定の半径</returns>
    public float GetCollision()
    {
        return CollisionRadius;
    }
    /// <summary>
    /// 有効かどうかを判定する
    /// </summary>
    /// <returns>有効ならtrue</returns>
    public bool IsValid()
    {
        return valid;
    }
    // Start is called before the first frame update
    void Start()
    {
        power = InitialPower;
        turnCount = 0;
        switch(sort)
        {
        case ItemSort.Ring:
            validationTurn = RingValidationTurn;
            if(sizePattern == 1)
            {
                CollisionRadius = func.metrecalc(10);
            }
            else if(sizePattern == 2)
            {
                CollisionRadius = func.metrecalc(8);
            }
            else if(sizePattern == 3)
            {
                CollisionRadius = func.metrecalc(5);
            }
            break;
        case ItemSort.Crystal:
            validationTurn = CrystalValidationTurn;
            CollisionRadius = func.metrecalc(5);
            break;
        case ItemSort.Fruit:
            validationTurn = FruitValidationTurn;
            CollisionRadius = func.metrecalc(5);
            break;
        }
        valid = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
