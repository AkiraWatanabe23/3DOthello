using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TextMove : MonoBehaviour
{
    private Text _color = default;

    private void Start()
    {
        _color = GetComponent<Text>();

        //DOFade(x, y) ... y�b�����ă��l��x�ɂ���
        _color.DOFade(0.25f, 2f).SetLoops(-1, LoopType.Yoyo);
    }
}
