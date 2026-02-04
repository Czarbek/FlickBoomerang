using UnityEngine;

/// <summary>
/// チュートリアル用矢印
/// </summary>
public class GuideArrow : MonoBehaviour
{
    enum State
    {
        Wait,
        Blink,
    };
    /// <summary>
    /// 不透明度最大値
    /// </summary>
    private const float MaxAlpha = 1.0f;
    /// <summary>
    /// 不透明度最小値
    /// </summary>
    private const float MinAlpha = 0.0f;
    /// <summary>
    /// 点滅周期
    /// </summary>
    [SerializeField]private float Frequency;
    /// <summary>
    /// 待機時間(ミリ秒)
    /// </summary>
    [SerializeField]private int WaitTimeMiliSec;
    /// <summary>
    /// 待機時間(フレーム数)
    /// </summary>
    [SerializeField] private int WaitTime;
    /// <summary>
    /// 状態
    /// </summary>
    private State state;
    /// <summary>
    /// 処理時間
    /// </summary>
    private int time;
    /// <summary>
    /// SpriteRendererコンポーネント
    /// </summary>
    private SpriteRenderer sr;

    /// <summary>
    /// 角度を変更する
    /// </summary>
    /// <param name="angle">角度(度数法)</param>
    public void SetAngle(float angle)
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    /// <summary>
    /// 縦の拡大率を変更する
    /// </summary>
    /// <param name="scale"></param>
    public void SetScale(float scale)
    {
        transform.localScale = new Vector2(1, scale);
    }
    private async void LoadParameter()
    {
        string path = Application.streamingAssetsPath;
        // 読み込みたいCSVファイルのパスを指定して開く
        path = "ArrowData.csv";
        string[][] values = await CsvReader.LoadCsvData(path);

        //データがNULLなら処理やめる
        if(values == null)
        {
            return;
        }

        int nbuffer;
        //WaitTimeMiliSec
        if(int.TryParse(values[0][1], out nbuffer))
        {
            WaitTimeMiliSec = nbuffer;
            WaitTime = (int)(WaitTimeMiliSec / func.FRAMETIME);
        }
        //Frequency
        if(int.TryParse(values[1][1], out nbuffer))
        {
            int freq = nbuffer;
            Frequency = 360.0f / func.FRAMERATE * 1000.0f / freq;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state=State.Wait;
        time = 0;
        sr = GetComponent<SpriteRenderer>();
        LoadParameter();
        sr.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        time++;
        switch(state)
        {
        case State.Wait:
            if(time == WaitTime)
            {
                time = 0;
                state = State.Blink;
            }
            break;
        case State.Blink:
            sr.color = new Color(1, 1, 1, MinAlpha + (MaxAlpha - MinAlpha) * (-func.cos(time * Frequency) + 1.0f) / 2.0f);
            break;
        }
    }
}
