using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ʃ��C�u����
/// </summary>
public class func
{
    /// <summary>
    /// �t���[�����[�g(/�b)
    /// </summary>
    public const int FRAMERATE = 60;
    /// <summary>
    /// �t���[��������̎���(�~���b)
    /// </summary>
    public const float FRAMETIME = 1000.0f / FRAMERATE;
    /// <summary>
    /// ��ʒ��S��x���W
    /// </summary>
    public const float SCCX = 0.0f;
    /// <summary>
    /// ��ʒ��S��y���W
    /// </summary>
    public const float SCCY = 0.0f;
    /// <summary>
    /// ��ʂ̉���(�s�N�Z��)
    /// </summary>
    public const int SCW = 1080;
    /// <summary>
    /// ��ʂ̍���(�s�N�Z��)
    /// </summary>
    public const int SCH = 1920;
    /// <summary>
    /// �J�����̏c�̑傫��
    /// </summary>
    public const float camHeight = 2.5f;
    /// <summary>
    /// �J�����̉��̑傫��
    /// </summary>
    public const float camWidth = (float)SCW / (float)SCH * camHeight;
    /// <summary>
    /// ��ʏ�[��y���W
    /// </summary>
    public const float TopEdge = 2.5f;
    /// <summary>
    /// ��ʉ��[��y���W
    /// </summary>
    public const float BottomEdge = -2.5f;
    /// <summary>
    /// ��ʍ��[��x���W
    /// </summary>
    public const float LeftEdge = -camWidth;
    /// <summary>
    /// ��ʉE�[��x���W
    /// </summary>
    public const float RightEdge = camWidth;

    /// <summary>
    /// �L�[�̓��͏��
    /// </summary>
    static private int[] keystate = new int[255];

    /// <summary>
    /// �^�b�`�̏��
    /// </summary>
    static private int touchstate;

    /// <summary>
    /// ��ʒ[�̕���
    /// </summary>
    public enum edge
    {
        /// <summary>�E�[</summary>
        right,
        /// <summary>���[</summary>
        left,
        /// <summary>��[</summary>
        up,
        /// <summary>���[</summary>
        down,
    };
    /// <summary>
    /// �����x�v�Z�ɗp����\����
    /// </summary>
    public struct accel
    {
        /// <summary>�����x</summary>
        public float firstspd;
        /// <summary>�����x</summary>
        public float delta;
        /// <summary>�p�x</summary>
        public float angle;
    };

    /// <summary>
    /// keystate�̏�����
    /// </summary>
    /// <returns>����ɓ��삵�Ă����1��Ԃ�</returns>
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
    /// �L�[���͏�Ԃ̍X�V
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
    /// �L�[���͏�Ԃ��擾
    /// </summary>
    /// <param name="i">���͂��擾����L�[</param>
    /// <returns>�L�[��������Ă���o�߂����t���[����</returns>
    public static int getkey(int i)
    {
        return keystate[i];
    }
    /// <summary>
    /// �^�b�`�̏�Ԃ��擾
    /// </summary>
    /// <returns></returns>
    public static int getTouch()
    {
        return touchstate;
    }
    /// <summary>
    /// �^�b�`�̋�ԏ��2D���W���擾����
    /// </summary>
    /// <returns>�}�E�X��2D���W</returns>
    public static Vector2 getTouchPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
    }
    /// <summary>
    /// �X�P�[���̍Čv�Z���s��
    /// </summary>
    /// <param name="a">���[�g���P��</param>
    /// <returns>Unity��ԃX�P�[��</returns>
    public static float metrecalc(float a)
    {
        return a / 160 * camHeight * 4;
    }
    /// <summary>
    /// �s�N�Z���P�ʂ̃X�P�[����Unity�̋�ԃX�P�[���ɕϊ�����
    /// </summary>
    /// <param name="a">�s�N�Z���P��</param>
    /// <returns>Unity��ԃX�P�[��</returns>
    public static float pxcalc(int a)
    {
        return (float)a / SCH * camHeight * 4;
    }
    /// <summary>
    /// �}�E�X�̋�ԏ��2D���W���擾����
    /// </summary>
    /// <returns>�}�E�X��2D���W</returns>
    public static Vector2 mouse()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    /// <summary>
    /// ��Βl�����߂�
    /// </summary>
    /// <param name="a">���l</param>
    /// <returns>�����̐�Βl</returns>
    public static float abs(float a)
    {
        return a < 0 ? -a : a;
    }
    /// <summary>
    /// �����������߂�
    /// </summary>
    /// <param name="a">���l</param>
    /// <returns>�����̕�����</returns>
    static public float sqrt(float a)
    {
        return Mathf.Sqrt(a);
    }
    /// <summary>
    /// �x���@�ŕ\�L���ꂽ�p�x���ʓx�@�ɕϊ�����
    /// </summary>
    /// <param name="angle">�p�x(�x��)</param>
    /// <returns>�p�x(���W�A��)</returns>
    static public float rad(float angle)
    {
        return angle * Mathf.PI / 180;
    }
    /// <summary>
    /// �ʓx�@�ŕ\�L���ꂽ�p�x��x���@�ɕϊ�����
    /// </summary>
    /// <param name="angle">�p�x(���W�A��)</param>
    /// <returns>�p�x(�x��)</returns>
    private static float deg(float angle)
    {
        return 180 / Mathf.PI * angle;
    }
    /// <summary>
    /// ���������߂�
    /// </summary>
    /// <param name="angle">�p�x(�x��)</param>
    /// <returns>����</returns>
    static public float sin(float angle)
    {
        return Mathf.Sin(rad(angle));
    }
    /// <summary>
    /// �]�������߂�
    /// </summary>
    /// <param name="angle">�p�x(�x��)</param>
    /// <returns>�]��</returns>
    static public float cos(float angle)
    {
        return Mathf.Cos(rad(angle));
    }
    /// <summary>
    /// ��_�Ԃ̋��������߂�
    /// </summary>
    /// <param name="sx">�n�_��x���W</param>
    /// <param name="sy">�n�_��y���W</param>
    /// <param name="dx">�I�_��x���W</param>
    /// <param name="dy">�I�_��y���W</param>
    /// <returns>����</returns>
    static public float dist(float sx, float sy, float dx, float dy)
    {
        return Mathf.Sqrt((dx - sx) * (dx - sx) + (dy - sy) * (dy - sy));
    }
    /// <summary>
    /// ��_�Ԃ����Ԓ�����x=0�Ƃ̂Ȃ��p�����߂�
    /// </summary>
    /// <param name="x">�n�_��x���W</param>
    /// <param name="y">�n�_��y���W</param>
    /// <param name="tx">�I�_��x���W</param>
    /// <param name="ty">�I�_��y���W</param>
    /// <returns>�p�x(���W�A��)</returns>
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
    /// ���X�Ɍ�������~����^���̏����x�Ɖ����x(<0)�����߂�
    /// </summary>
    /// <param name="sum">���ړ���</param>
    /// <param name="times">��~����܂ł̎���(�t���[����)</param>
    /// <returns>�����x�E�����x(<0)</returns>
    public static accel getDeceleration(float sum, int times)
    {
        accel res;
        res.firstspd = sum * 2 / (times - 1);
        res.delta = -sum * 2 / (times - 1) / (times - 1);
        res.angle = 0;
        return res;
    }
    /// <summary>
    /// ���X�Ɍ�������~����^���̏����x�E�����x(<0)�E�p�x(�x��)���n�_�ƏI�_�̍��W���狁�߂�
    /// </summary>
    /// <param name="sx">�n�_��x���W</param>
    /// <param name="sy">�n�_��y���W</param>
    /// <param name="dx">�I�_��x���W</param>
    /// <param name="dy">�I�_��y���W</param>
    /// <param name="times">��~����܂ł̎���(�t���[����)</param>
    /// <returns>�����x�E�����x(<0)�E�p�x(�x��)</returns>
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
    /// ��̃I�u�W�F�N�g���~�`�Ƃ݂Ȃ��A�Փ˂��Ă��邩�𔻒肷��
    /// </summary>
    /// <param name="x">�I�u�W�F�N�g��x���W</param>
    /// <param name="y">�I�u�W�F�N�g��y���W</param>
    /// <param name="r">�I�u�W�F�N�g�̔��a</param>
    /// <param name="tx">�΂���I�u�W�F�N�g��x���W</param>
    /// <param name="ty">�΂���I�u�W�F�N�g��y���W</param>
    /// <param name="tr">�΂���I�u�W�F�N�g�̔��a</param>
    /// <returns>�Փ˂��Ă���Ƃ�true��Ԃ�</returns>
    static public bool CircleCollision(float x, float y, float r, float tx, float ty, float tr)
    {
        return dist(x, y, tx, ty) <= r + tr;
    }
    static public bool CircleCollision(Vector2 v, float r, Vector2 tv, float tr)
    {
        return dist(v.x, v.y, tv.x, tv.y) <= r + tr;
    }
    /// <summary>
    /// ��̃I�u�W�F�N�g����`�Ƃ݂Ȃ��A�Փ˂��Ă��邩�𔻒肷��
    /// </summary>
    /// <param name="x">�I�u�W�F�N�g��x���W</param>
    /// <param name="y">�I�u�W�F�N�g��y���W</param>
    /// <param name="xr">�I�u�W�F�N�g�̉��̕ӂ̒���(����)</param>
    /// <param name="yr">�I�u�W�F�N�g�̏c�̕ӂ̒���(����)</param>
    /// <param name="tx">�΂���I�u�W�F�N�g��x���W</param>
    /// <param name="ty">�΂���I�u�W�F�N�g��y���W</param>
    /// <param name="txr">�΂���I�u�W�F�N�g�̉��̕ӂ̒���(����)</param>
    /// <param name="tyr">�΂���I�u�W�F�N�g�̏c�̕ӂ̒���(����)</param>
    /// <returns>�Փ˂��Ă���Ƃ�true��Ԃ�</returns>
    public static bool RectCollision(float x, float y, float xr, float yr, float tx, float ty, float txr, float tyr)
    {
        bool conditionXA = (x <= tx) && (x + xr >= tx - txr);
        bool conditionXB = (x > tx) && (x - xr <= tx + txr);
        bool conditionYA = (y <= ty) && (y + yr >= ty - tyr);
        bool conditionYB = (y > ty) && (y - yr <= ty + tyr);
        return (conditionXA||conditionXB) && (conditionYA || conditionYB);
    }
    /// <summary>
    /// �}�E�X�|�C���^�ƃI�u�W�F�N�g�̏Փ˂𔻒肷��
    /// </summary>
    /// <param name="x">�I�u�W�F�N�g��x���W</param>
    /// <param name="y">�I�u�W�F�N�g��y���W</param>
    /// <param name="xr">�I�u�W�F�N�g�̉��̔��a</param>
    /// <param name="yr">�I�u�W�F�N�g�̏c�̔��a</param>
    /// <param name="isRect">�I�u�W�F�N�g����`�Ƃ݂Ȃ����ǂ���</param>
    /// <returns>�Փ˂��Ă���Ƃ�true��Ԃ�</returns>
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
    /// �^�b�`�ʒu�ƃI�u�W�F�N�g�̏Փ˂𔻒肷��
    /// </summary>
    /// <param name="x">�I�u�W�F�N�g��x���W</param>
    /// <param name="y">�I�u�W�F�N�g��y���W</param>
    /// <param name="xr">�I�u�W�F�N�g�̉��̔��a</param>
    /// <param name="yr">�I�u�W�F�N�g�̏c�̔��a</param>
    /// <param name="isRect">�I�u�W�F�N�g����`�Ƃ݂Ȃ����ǂ���</param>
    /// <returns>�Փ˂��Ă���Ƃ�true��Ԃ�</returns>
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
    /// �^����ꂽ���W����ʂ̊O�ɂ��邩�𔻒肷��
    /// </summary>
    /// <param name="x">x���W</param>
    /// <param name="y">y���W</param>
    /// <param name="direction">����</param>
    /// <returns>��ʊO�ɂ���Ȃ�true��Ԃ�</returns>
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
