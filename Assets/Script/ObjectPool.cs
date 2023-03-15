using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _tileObj = default;
    [SerializeField] private GameObject _whiteObj = default;
    [SerializeField] private GameObject _blackObj = default;

    private readonly List<GameObject> _tiles = new();
    private readonly List<GameObject> _white = new();
    private readonly List<GameObject> _black = new();

    private const int POOL_SIZE = 64;

    private void Awake()
    {
        for (int i = 0; i < POOL_SIZE; i++)
        {
            //オブジェクトを出現させる
            GameObject tile = Instantiate(_tileObj);
            GameObject white = Instantiate(_whiteObj);
            GameObject black = Instantiate(_blackObj);

            //非アクティブにする
            tile.SetActive(false);
            white.SetActive(false);
            black.SetActive(false);

            //Listに格納する
            _tiles.Add(tile);
            _white.Add(white);
            _black.Add(black);
        }

        for (int i = 1; i < 9; i++)
        {
            for (int j = 1; j < 9; j++)
            {
                SetBoard(new Vector3(i, 0, j));
            }
        }
        SetWhite(new Vector3(4, 0.1f, 5));
        SetWhite(new Vector3(5, 0.1f, 4));
        SetBlack(new Vector3(4, 0.1f, 4));
        SetBlack(new Vector3(5, 0.1f, 5));
    }

    private void SetBoard(Vector3 pos)
    {
        //オブジェクトのList内から、非アクティブなものを検索する
        foreach (var obj in _tiles)
        {
            if (!obj.activeSelf)
            {
                //アクティブにして、positionを変更する
                obj.SetActive(true);
                obj.transform.localPosition = pos;
                //ループを終了
                break;
            }
        }
    }

    public void SetWhite(Vector3 pos)
    {
        //オブジェクトのList内から、非アクティブなものを検索する
        foreach (var obj in _white)
        {
            if (!obj.activeSelf)
            {
                //アクティブにして、positionを変更する
                obj.SetActive(true);
                obj.transform.localPosition = pos;
                //ループを終了
                break;
            }
        }
    }

    public void SetBlack(Vector3 pos)
    {
        foreach (var obj in _black)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                obj.transform.localPosition = pos;
                break;
            }
        }
    }
}
