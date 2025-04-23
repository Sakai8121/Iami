# nullable enable
using UnityEngine.SceneManagement;

public class SceneLoader
{
    public void LoadHomeScene()
    {
        SceneManager.LoadScene("HomeScene");
    }

    public void LoadPlayScene()
    {
        SceneManager.LoadScene("PlayScene");
    }
}