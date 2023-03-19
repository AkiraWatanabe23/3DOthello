using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image _image = default;
    [SerializeField] private Text _blackText = default;
    [SerializeField] private Text _whiteText = default;

    private int _blackCount = 0;
    private int _whiteCount = 0;

    public int BlackCount { get => _blackCount; set => _blackCount = value; }
    public int WhiteCount { get => _whiteCount; set => _whiteCount = value; }

    private void Start()
    {
        
    }

    private void Update()
    {
        _blackText.text = "Black : " + _blackCount.ToString();
        _whiteText.text = "White : " + _whiteCount.ToString();

        if (Input.GetMouseButtonDown(0))
        {
            GameFinish();
        }
    }

    public void GameFinish()
    {
        _image.gameObject.SetActive(true);
        //ここでDOTween
    }
}
