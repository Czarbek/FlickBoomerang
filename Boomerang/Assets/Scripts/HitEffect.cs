using UnityEngine;

/// <summary>
/// “GƒqƒbƒgƒGƒtƒFƒNƒg
/// </summary>
public class HitEffect : MonoBehaviour
{
    /// <summary>
    /// •\¦ó‘Ôˆê——
    /// </summary>
    private enum State
    {
        /// <summary>x•ûŒüŠg‘å</summary>
        FirstExpand,
        /// <summary>y•ûŒüŠg‘å</summary>
        SecondExpand,
        /// <summary>‘Ò‹@</summary>
        Wait,
        /// <summary>x•ûŒük¬</summary>
        FirstShrink,
        /// <summary>y•ûŒük¬</summary>
        SecondShrink,
    };

    /// <summary>
    /// ˆê‰ñ–ÚŠg‘åŠÔ
    /// </summary>
    private const int FirstExpandTime = 5;
    /// <summary>
    /// “ñ‰ñ–ÚŠg‘åŠÔ
    /// </summary>
    private const int SecondExpandTime = 5;
    /// <summary>
    /// ‘Ò‹@ŠÔ
    /// </summary>
    private const int WaitTime = 10;
    /// <summary>
    /// ˆê‰ñ–Úk¬ŠÔ
    /// </summary>
    private const int FirstShrinkTime = 5;
    /// <summary>
    /// “ñ‰ñ–Úk¬ŠÔ
    /// </summary>
    private const int SecondShrinkTime = 5;
    /// <summary>
    /// Å¬Šg‘å—¦
    /// </summary>
    private const float MinScale = 0.1f;
    /// <summary>
    /// Å‘åŠg‘å—¦
    /// </summary>
    private const float MaxScale = 1.0f;
    /// <summary>
    /// ƒfƒtƒHƒ‹ƒgŠg‘å—¦
    /// </summary>
    private const float DefaultScale = 0.5f;

    /// <summary>
    /// •\¦ó‘Ô
    /// </summary>
    private State state;
    /// <summary>
    /// ˆ—ŠÔ
    /// </summary>
    private int time;
    /// <summary>
    /// x•ûŒüŠg‘å—¦
    /// </summary>
    private float scaleX;
    /// <summary>
    /// y•ûŒüŠg‘å—¦
    /// </summary>
    private float scaleY;

    /// <summary>
    /// •\¦ó‘Ô‚ğ•ÏX‚·‚é
    /// </summary>
    /// <param name="nextState"></param>
    private void SetState(State nextState)
    {
        state = nextState;
        time = 0;
    }
    void Start()
    {
        state = State.FirstExpand;
        time = 0;
        scaleX = MinScale;
        scaleY = MinScale;
        transform.localScale = new Vector3(scaleX * DefaultScale, scaleY * DefaultScale, 1);
    }

    // Update is called once per frame
    void Update()
    {
        time++;
        switch(state)
        {
        case State.FirstExpand:
            scaleX = MinScale + (MaxScale - MinScale) * (float)time / FirstExpandTime;
            if(time == FirstExpandTime)
            {
                scaleX = MaxScale;
                SetState(State.SecondExpand);
            }
            break;
        case State.SecondExpand:
            scaleY = MinScale + (MaxScale - MinScale) * (float)time / SecondExpandTime;
            if(time == SecondExpandTime)
            {
                scaleY = MaxScale;
                SetState(State.Wait);
            }
            break;
        case State.Wait:
            if(time == WaitTime)
            {
                SetState(State.FirstShrink);
            }
            break;
        case State.FirstShrink:
            scaleX = MaxScale - (MaxScale - MinScale) * (float)time / FirstShrinkTime;
            if(time == FirstShrinkTime)
            {
                scaleX = MinScale;
                SetState(State.SecondShrink);
            }
            break;
        case State.SecondShrink:
            scaleY = MaxScale - (MaxScale - MinScale) * (float)time / SecondShrinkTime;
            if(time == SecondShrinkTime)
            {
                scaleY = MinScale;
                Destroy(gameObject);
            }
            break;
        }
        transform.localScale = new Vector3(scaleX * DefaultScale, scaleY * DefaultScale, 1);
    }
}
