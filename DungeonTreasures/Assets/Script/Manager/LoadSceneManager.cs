using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : DontDestory<LoadSceneManager>
{
    public enum eScene
    {
        Title,
        Lobby,
        Stage,
        Game_Castle_01
    }

    public string NowScene()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void LoadGameScene(string GameSceneName)
    {
        SceneManager.LoadScene(GameSceneName);
    }

    public void LoadTitleScene()
    {
        SceneManager.LoadScene("Title");
    }

    public void LoadLobbyScene()
    {
        SceneManager.LoadScene("Lobby");

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGM(eScene.Lobby);
        }
    }

    public void LoadStageScene()
    {
        SceneManager.LoadScene("Stage");

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayBGM(eScene.Stage);
    }

    public void LoadRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayBGM((eScene)SceneManager.GetActiveScene().buildIndex);
    }

}
