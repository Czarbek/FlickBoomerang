using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    /// <summary>
    /// �{�^���̎�ވꗗ
    /// </summary>
    public enum ButtonSort
    {
        /// <summary>�R���e�B�j���[����</summary>
        Continue_Yes,
        /// <summary>�R���e�B�j���[���Ȃ�</summary>
        Continue_No,
        /// <summary>���ɐi��</summary>
        Clear_Next,
        /// <summary>�^�C�g���ɖ߂�</summary>
        Clear_Quit,
    };
    /// <summary>
    /// �{�^���̏�Ԉꗗ
    /// </summary>
    public enum State
    {
        /// <summary>��\��</summary>
        Invalid,
        /// <summary>�t�F�[�h�C��</summary>
        FadeIn,
        /// <summary>�����ꂽ</summary>
        Pushed,
        /// <summary>������Ȃ�����</summary>
        NotPushed,
    };
    /// <summary>
    /// �{�^���̎��
    /// </summary>
    ButtonSort buttonSort;
    /// <summary>
    /// Component<SpriteRenderer>
    /// </summary>
    SpriteRenderer sp;
    /// <summary>
    /// �摜��r�l
    /// </summary>
    private float r;
    /// <summary>
    /// �摜��g�l
    /// </summary>
    private float g;
    /// <summary>
    /// �摜��b�l
    /// </summary>
    private float b;
    /// <summary>
    /// �摜��alpha�l
    /// </summary>
    private int alpha;
    public void SetButton(ButtonSort buttonSort)
    {
        this.buttonSort = buttonSort;
    }
    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        r = sp.color.r;
        g = sp.color.g;
        b = sp.color.b;
        alpha = 0;
        sp.color = new Color(r, g, b, alpha);

        switch(buttonSort)
        {
        case ButtonSort.Continue_Yes:
            sp.sprite = (Sprite)Resources.Load("button_yes");
            break;
        case ButtonSort.Continue_No:
            sp.sprite = (Sprite)Resources.Load("button_no");
            break;
        case ButtonSort.Clear_Next:
            sp.sprite = (Sprite)Resources.Load("button_next");
            break;
        case ButtonSort.Clear_Quit:
            sp.sprite = (Sprite)Resources.Load("button_quit");
            break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(buttonSort)
        {
        case ButtonSort.Continue_Yes:
            break;
        case ButtonSort.Continue_No:
            break;
        case ButtonSort.Clear_Next:
            break;
        case ButtonSort.Clear_Quit:
            break;
        }
    }
}
