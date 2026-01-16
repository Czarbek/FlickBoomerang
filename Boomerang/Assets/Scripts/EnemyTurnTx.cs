using UnityEngine;
using TMPro;

/// <summary>
/// 敵ターンを示すテキスト
/// </summary>
public class EnemyTurnTx : MonoBehaviour
{
    /// <summary>
    /// 表示状態一覧
    /// </summary>
    private enum State
    {
        /// <summary>非表示</summary>
        Invalid,
        /// <summary>スライドイン</summary>
        SlideIn,
        /// <summary>待機</summary>
        Wait,
        /// <summary>スライドアウト</summary>
        SlideOut,
    };
    /// <summary>
    /// 初期位置x座標
    /// </summary>
    private const float InitialX = func.RightEdge;
    /// <summary>
    /// 初期位置y座標
    /// </summary>
    private const float InitialY = 0;
    /// <summary>
    /// 待機位置x座標
    /// </summary>
    private const float MediumX = 0;
    /// <summary>
    /// 終点x座標
    /// </summary>
    private const float EndX = func.LeftEdge;
    /// <summary>
    /// スライド時間
    /// </summary>
    private const int SlideTime = 10;
    /// <summary>
    /// TMProコンポーネント
    /// </summary>
    private TextMeshProUGUI tmp;
    /// <summary>
    /// 表示状態
    /// </summary>
    private State state;
    /// <summary>
    /// x座標
    /// </summary>
    private float x;
    /// <summary>
    /// y座標
    /// </summary>
    private float y;
    /// <summary>
    /// 不透明度
    /// </summary>
    private float alpha;
    /// <summary>
    /// 処理時間
    /// </summary>
    private int time;

    /// <summary>
    /// 表示状態を変更する
    /// </summary>
    /// <param name="isVisible">trueなら表示開始、falseなら表示終了</param>
    public void SetVisibility(bool isVisible)
    {
        if(isVisible && state == State.Invalid || !isVisible && state == State.Wait)
        {
            state = isVisible ? State.SlideIn : State.SlideOut;
            time = 0;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = State.Invalid;
        x = InitialX;
        y = InitialY;
        alpha = 0;
        time = 0;
        transform.position = new Vector2(x, y);
        tmp = GetComponent<TextMeshProUGUI>();
        tmp.color = new Color(1, 1, 1, alpha);
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
        case State.Invalid:
            alpha = 0;
            x = InitialX;
            break;
        case State.SlideIn:
            time++;
            x = InitialX + (MediumX - InitialX) * (float)time / SlideTime;
            alpha = (float)time / SlideTime;
            if(time == SlideTime)
            {
                state = State.Wait;
                time = 0;
                alpha = 1;
            }
            break;
        case State.Wait:
            alpha = 1;
            x = MediumX;
            break;
        case State.SlideOut:
            time++;
            x = MediumX + (EndX - MediumX) * (float)time / SlideTime;
            alpha = 1.0f - (float)time / SlideTime;
            if(time == SlideTime)
            {
                state = State.Invalid;
                time = 0;
                alpha = 0;
            }
            break;
        }
        tmp.color = new Color(1, 1, 1, alpha);
        transform.position = new Vector2(x, y);
    }
}
