using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 共通ライブラリ
/// </summary>
public class func
{
    /// <summary>
    /// フレームレート(/秒)
    /// </summary>
    public const int FRAMERATE = 60;
    /// <summary>
    /// フレームあたりの時間(ミリ秒)
    /// </summary>
    public const float FRAMETIME = 1000.0f / FRAMERATE;
    /// <summary>
    /// 画面中心のx座標
    /// </summary>
    public const float SCCX = 0.0f;
    /// <summary>
    /// 画面中心のy座標
    /// </summary>
    public const float SCCY = 0.0f;
    /// <summary>
    /// 画面の横幅(ピクセル)
    /// </summary>
    public const int SCW = 1080;
    /// <summary>
    /// 画面の高さ(ピクセル)
    /// </summary>
    public const int SCH = 1920;
    /// <summary>
    /// カメラの縦の大きさ
    /// </summary>
    public const float camHeight = 2.5f;
    /// <summary>
    /// カメラの横の大きさ
    /// </summary>
    public const float camWidth = (float)SCW / (float)SCH * camHeight;
    /// <summary>
    /// 画面上端のy座標
    /// </summary>
    public const float TopEdge = 2.5f;
    /// <summary>
    /// 画面下端のy座標
    /// </summary>
    public const float BottomEdge = -2.5f;
    /// <summary>
    /// 画面左端のx座標
    /// </summary>
    public const float LeftEdge = -camWidth;
    /// <summary>
    /// 画面右端のx座標
    /// </summary>
    public const float RightEdge = camWidth;

    /// <summary>
    /// キーの入力状態
    /// </summary>
    static private int[] keystate = new int[255];

    /// <summary>
    /// タッチの状態
    /// </summary>
    static private int touchstate;

    /// <summary>
    /// 画面端の方向
    /// </summary>
    public enum edge
    {
        /// <summary>右端</summary>
        right,
        /// <summary>左端</summary>
        left,
        /// <summary>上端</summary>
        up,
        /// <summary>下端</summary>
        down,
    };
    /// <summary>
    /// 加速度計算に用いる構造体
    /// </summary>
    public struct accel
    {
        /// <summary>初速度</summary>
        public float firstspd;
        /// <summary>加速度</summary>
        public float delta;
        /// <summary>角度</summary>
        public float angle;
    };

    /// <summary>
    /// keystateの初期化
    /// </summary>
    /// <returns>正常に動作していれば1を返す</returns>
    static public int KeystateInit()
    {
        for(int i = 0; i < 255; i++)
        {
            keystate[i] = 0;
        }
        touchstate = -1;
        return 1;
    }
    /// <summary>
    /// キー入力状態の更新
    /// </summary>
    public static void keystateUpdate()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            keystate[32]++;
        }
        else
        {
            keystate[32] = 0;
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            keystate[39]++;
        }
        else
        {
            keystate[39] = 0;
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            keystate[37]++;
        }
        else
        {
            keystate[37] = 0;
        }
        if(Input.GetKey(KeyCode.UpArrow))
        {
            keystate[38]++;
        }
        else
        {
            keystate[38] = 0;
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            keystate[40]++;
        }
        else
        {
            keystate[40] = 0;
        }
        if(Input.GetKey(KeyCode.A))
        {
            keystate[65]++;
        }
        else
        {
            keystate[65] = 0;
        }
        if(Input.GetKey(KeyCode.S))
        {
            keystate[83]++;
        }
        else
        {
            keystate[83] = 0;
        }
        if(Input.GetKey(KeyCode.W))
        {
            keystate[87]++;
        }
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                touchstate = 1;
            }
            else if(touch.phase == TouchPhase.Moved)
            {
                touchstate++;
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                touchstate = -1;
            }
        }
        else
        {
            touchstate = 0;
        }
    }
    /// <summary>
    /// キー入力状態を取得
    /// </summary>
    /// <param name="i">入力を取得するキー</param>
    /// <returns>キーが押されてから経過したフレーム数</returns>
    public static int getkey(int i)
    {
        return keystate[i];
    }
    /// <summary>
    /// タッチの状態を取得
    /// </summary>
    /// <returns></returns>
    public static int getTouch()
    {
        return touchstate;
    }
    /// <summary>
    /// タッチの空間上の2D座標を取得する
    /// </summary>
    /// <returns>マウスの2D座標</returns>
    public static Vector2 getTouchPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
    }
    /// <summary>
    /// スケールの再計算を行う
    /// </summary>
    /// <param name="a">メートル単位</param>
    /// <returns>Unity空間スケール</returns>
    public static float metrecalc(float a)
    {
        return a / 160 * camHeight * 4;
    }
    /// <summary>
    /// ピクセル単位のスケールをUnityの空間スケールに変換する
    /// </summary>
    /// <param name="a">ピクセル単位</param>
    /// <returns>Unity空間スケール</returns>
    public static float pxcalc(int a)
    {
        return (float)a / SCH * camHeight * 4;
    }
    /// <summary>
    /// マウスの空間上の2D座標を取得する
    /// </summary>
    /// <returns>マウスの2D座標</returns>
    public static Vector2 mouse()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    /// <summary>
    /// 絶対値を求める
    /// </summary>
    /// <param name="a">数値</param>
    /// <returns>引数の絶対値</returns>
    public static float abs(float a)
    {
        return a < 0 ? -a : a;
    }
    /// <summary>
    /// 平方根を求める
    /// </summary>
    /// <param name="a">数値</param>
    /// <returns>引数の平方根</returns>
    static public float sqrt(float a)
    {
        return Mathf.Sqrt(a);
    }
    /// <summary>
    /// 度数法で表記された角度を弧度法に変換する
    /// </summary>
    /// <param name="angle">角度(度数)</param>
    /// <returns>角度(ラジアン)</returns>
    static public float rad(float angle)
    {
        return angle * Mathf.PI / 180;
    }
    /// <summary>
    /// 弧度法で表記された角度を度数法に変換する
    /// </summary>
    /// <param name="angle">角度(ラジアン)</param>
    /// <returns>角度(度数)</returns>
    private static float deg(float angle)
    {
        return 180 / Mathf.PI * angle;
    }
    /// <summary>
    /// 正弦を求める
    /// </summary>
    /// <param name="angle">角度(度数)</param>
    /// <returns>正弦</returns>
    static public float sin(float angle)
    {
        return Mathf.Sin(rad(angle));
    }
    /// <summary>
    /// 余弦を求める
    /// </summary>
    /// <param name="angle">角度(度数)</param>
    /// <returns>余弦</returns>
    static public float cos(float angle)
    {
        return Mathf.Cos(rad(angle));
    }
    /// <summary>
    /// 二点間の距離を求める
    /// </summary>
    /// <param name="sx">始点のx座標</param>
    /// <param name="sy">始点のy座標</param>
    /// <param name="dx">終点のx座標</param>
    /// <param name="dy">終点のy座標</param>
    /// <returns>距離</returns>
    static public float dist(float sx, float sy, float dx, float dy)
    {
        return Mathf.Sqrt((dx - sx) * (dx - sx) + (dy - sy) * (dy - sy));
    }
    /// <summary>
    /// 二点間を結ぶ直線とx=0とのなす角を求める
    /// </summary>
    /// <param name="x">始点のx座標</param>
    /// <param name="y">始点のy座標</param>
    /// <param name="tx">終点のx座標</param>
    /// <param name="ty">終点のy座標</param>
    /// <returns>角度(ラジアン)</returns>
    static public float getAngle(float x, float y, float tx, float ty)
    {
        float result;
        if(tx - x != 0)
        {
            result = Mathf.Atan2(ty - y, tx - x);
        }
        else
        {
            if(ty > y)
            {
                result = rad(90);
            }
            else if(ty < y)
            {
                result = rad(-90);
            }
            else
            {
                result = rad(90);
            }
        }
        return result;
    }
    /// <summary>
    /// 徐々に減速し停止する運動の初速度と加速度(<0)を求める
    /// </summary>
    /// <param name="sum">総移動量</param>
    /// <param name="times">停止するまでの時間(フレーム数)</param>
    /// <returns>初速度・加速度(<0)</returns>
    public static accel getDeceleration(float sum, int times)
    {
        accel res;
        res.firstspd = sum * 2 / (times - 1);
        res.delta = -sum * 2 / (times - 1) / (times - 1);
        res.angle = 0;
        return res;
    }
    /// <summary>
    /// 徐々に減速し停止する運動の初速度・加速度(<0)・角度(度数)を始点と終点の座標から求める
    /// </summary>
    /// <param name="sx">始点のx座標</param>
    /// <param name="sy">始点のy座標</param>
    /// <param name="dx">終点のx座標</param>
    /// <param name="dy">終点のy座標</param>
    /// <param name="times">停止するまでの時間(フレーム数)</param>
    /// <returns>初速度・加速度(<0)・角度(度数)</returns>
    public static accel getDecelerationVector(float sx, float sy, float dx, float dy, int times)
    {
        accel res;
        float sum = dist(sx, sy, dx, dy);
        res.firstspd = sum * 2 / (times - 1);
        res.delta = -sum * 2 / (times - 1) / (times - 1);
        res.angle = deg(getAngle(sx, sy, dx, dy));
        return res;
    }
    /// <summary>
    /// 二つのオブジェクトを円形とみなし、衝突しているかを判定する
    /// </summary>
    /// <param name="x">オブジェクトのx座標</param>
    /// <param name="y">オブジェクトのy座標</param>
    /// <param name="r">オブジェクトの半径</param>
    /// <param name="tx">対するオブジェクトのx座標</param>
    /// <param name="ty">対するオブジェクトのy座標</param>
    /// <param name="tr">対するオブジェクトの半径</param>
    /// <returns>衝突しているときtrueを返す</returns>
    static public bool CircleCollision(float x, float y, float r, float tx, float ty, float tr)
    {
        return dist(x, y, tx, ty) <= r + tr;
    }
    static public bool CircleCollision(Vector2 v, float r, Vector2 tv, float tr)
    {
        return dist(v.x, v.y, tv.x, tv.y) <= r + tr;
    }
    /// <summary>
    /// 二つのオブジェクトを矩形とみなし、衝突しているかを判定する
    /// </summary>
    /// <param name="x">オブジェクトのx座標</param>
    /// <param name="y">オブジェクトのy座標</param>
    /// <param name="xr">オブジェクトの横の辺の長さ(半分)</param>
    /// <param name="yr">オブジェクトの縦の辺の長さ(半分)</param>
    /// <param name="tx">対するオブジェクトのx座標</param>
    /// <param name="ty">対するオブジェクトのy座標</param>
    /// <param name="txr">対するオブジェクトの横の辺の長さ(半分)</param>
    /// <param name="tyr">対するオブジェクトの縦の辺の長さ(半分)</param>
    /// <returns>衝突しているときtrueを返す</returns>
    public static bool RectCollision(float x, float y, float xr, float yr, float tx, float ty, float txr, float tyr)
    {
        bool conditionXA = (x <= tx) && (x + xr >= tx - txr);
        bool conditionXB = (x > tx) && (x - xr <= tx + txr);
        bool conditionYA = (y <= ty) && (y + yr >= ty - tyr);
        bool conditionYB = (y > ty) && (y - yr <= ty + tyr);
        return (conditionXA||conditionXB) && (conditionYA || conditionYB);
    }
    /// <summary>
    /// マウスポインタとオブジェクトの衝突を判定する
    /// </summary>
    /// <param name="x">オブジェクトのx座標</param>
    /// <param name="y">オブジェクトのy座標</param>
    /// <param name="xr">オブジェクトの横の半径</param>
    /// <param name="yr">オブジェクトの縦の半径</param>
    /// <param name="isRect">オブジェクトを矩形とみなすかどうか</param>
    /// <returns>衝突しているときtrueを返す</returns>
    public static bool MouseCollision(float x, float y, float xr, float yr = 0, bool isRect = false)
    {
        if(isRect)
        {
            return RectCollision(mouse().x, mouse().y, 0.01f, 0.01f, x, y, xr, yr);
        }
        else
        {
            return CircleCollision(mouse().x, mouse().y, 0.01f, x, y, xr);
        }
    }
    public static bool MouseCollision(Vector2 pos, float xr, float yr = 0, bool isRect = false)
    {
        if(isRect)
        {
            return RectCollision(mouse().x, mouse().y, 0.01f, 0.01f, pos.x, pos.y, xr, yr);
        }
        else
        {
            return CircleCollision(mouse().x, mouse().y, 0.01f, pos.x, pos.y, xr);
        }
    }
    /// <summary>
    /// タッチ位置とオブジェクトの衝突を判定する
    /// </summary>
    /// <param name="x">オブジェクトのx座標</param>
    /// <param name="y">オブジェクトのy座標</param>
    /// <param name="xr">オブジェクトの横の半径</param>
    /// <param name="yr">オブジェクトの縦の半径</param>
    /// <param name="isRect">オブジェクトを矩形とみなすかどうか</param>
    /// <returns>衝突しているときtrueを返す</returns>
    public static bool TouchCollision(float x, float y, float xr, float yr = 0, bool isRect = false)
    {
        if(touchstate == 0) return false;
        if(isRect)
        {
            return RectCollision(getTouchPosition().x, getTouchPosition().y, 0.01f, 0.01f, x, y, xr, yr);
        }
        else
        {
            return CircleCollision(getTouchPosition().x, getTouchPosition().y, 0.01f, x, y, xr);
        }
    }
    public static bool TouchCollision(Vector2 pos, float xr, float yr = 0, bool isRect = false)
    {
        if(isRect)
        {
            return RectCollision(getTouchPosition().x, getTouchPosition().y, 0.01f, 0.01f, pos.x, pos.y, xr, yr);
        }
        else
        {
            return CircleCollision(getTouchPosition().x, getTouchPosition().y, 0.01f, pos.x, pos.y, xr);
        }
    }
    /// <summary>
    /// 与えられた座標が画面の外にあるかを判定する
    /// </summary>
    /// <param name="x">x座標</param>
    /// <param name="y">y座標</param>
    /// <param name="direction">方向</param>
    /// <returns>画面外にあるならtrueを返す</returns>
    public static bool edgeOut(float x, float y, edge direction)
    {
        switch(direction)
        {
        case edge.right:
            return x > RightEdge;
        case edge.left:
            return x < LeftEdge;
        case edge.up:
            return y > TopEdge;
        case edge.down:
            return y < BottomEdge;
        default:
            return false;
        }
    }
}
