using Constants;
using UnityEngine;
using UnityEngine.SceneManagement;
using Banzan.Lib.Utility;

/// <summary>
/// 指定したシーンに遷移する
/// </summary>
public class SceneLoader : MonoBehaviour
{
    [Tooltip("遷移先のシーン")]
    [SerializeField] private SceneNames _nextScene = default;

    /// <summary> フェードアウト -> シーン遷移 </summary>
    /// <param name="sceneName"> 遷移先のシーン名 </param>
    private void PassToLoad(string sceneName)
    {
        //Fade.StartFadeOut
        //    (() => SceneManager.LoadScene(sceneName));
        SceneManager.LoadScene(sceneName);
    }

    /// <summary> フェードアウト -> シーン遷移
    ///          (シーン上のPanel,Button等に設定) </summary>
    public void LoadToScene()
    {
        PassToLoad(Consts.Scenes[_nextScene]);
    }

    [EnumAction(typeof(SceneNames))]
    public void Test(int scene)
    {
        SceneNames name = (SceneNames)scene;
        PassToLoad(Consts.Scenes[name]);
    }
}