using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadGame2Scene()
    {
        SceneManager.LoadScene("Game2");
    }

    public void LoadGame3Scene()
    {
        SceneManager.LoadScene("Game3");
    }

    public void LoadTest()
    {
        SceneManager.LoadScene("BossTest");
    }
}
