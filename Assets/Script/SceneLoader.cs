using Constants;
using UnityEngine;
using UnityEngine.SceneManagement;
using Banzan.Lib.Utility;

/// <summary>
/// 指定したシーンに遷移する
/// </summary>
public class SceneLoader : MonoBehaviour
{
    /// <summary> フェードアウト -> シーン遷移 </summary>
    /// <param name="sceneName"> 遷移先のシーン名 </param>
    private void PassToLoad(string sceneName)
    {
        Fade.StartFadeOut
            (() => SceneManager.LoadScene(sceneName));
    }

    [EnumAction(typeof(SceneNames))]
    /// <summary> フェードアウト -> シーン遷移
    ///          (シーン上のPanel,Button等に設定) </summary>
    public void LoadToScene(int scene)
    {
        PassToLoad(Consts.Scenes[(SceneNames)scene]);
    }
}