using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    public void GoToGame()
    {
        AudioController.Instance.PlaySFX("tap");
        SceneManager.LoadScene("game-scene");
    }

    public void QuitButton()
    {
        AudioController.Instance.PlaySFX("tap");
        Application.Quit();
    }
}
