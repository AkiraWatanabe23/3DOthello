using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Constants;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image _image = default;
    [SerializeField] private Text _blackText = default;
    [SerializeField] private Text _whiteText = default;
    [SerializeField] private Text _turn = default;

    private int _blackCount = 0;
    private int _whiteCount = 0;
    private RectTransform _pos = default;
    private Text[] _results = new Text[2];

    public int BlackCount { get => _blackCount; set => _blackCount = value; }
    public int WhiteCount { get => _whiteCount; set => _whiteCount = value; }

    private void Start()
    {
        _pos = _image.rectTransform;
        _results = _image.GetComponentsInChildren<Text>();
    }

    private void Update()
    {
        _blackText.text = "Black : " + _blackCount.ToString();
        _whiteText.text = "White : " + _whiteCount.ToString();

        if (GameManager.CurrentColor == Consts.BLACK)
        {
            _turn.text = "TURN : BLACK";
        }
        else if (GameManager.CurrentColor == Consts.WHITE)
        {
            _turn.text = "TURN : WHITE";
        }
    }

    public void GameFinish()
    {
        _image.gameObject.SetActive(true);
        _results[0].text = "Black : " + _blackCount.ToString();
        _results[1].text = "White : " + _whiteCount.ToString();

        //ここでDOTween(Panelの移動)
        _pos.DOLocalMoveX(0f, 2f).SetEase(Ease.Linear);
    }
}
