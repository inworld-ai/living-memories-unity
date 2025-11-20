using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    public void LoadScene(string sceneName) => SceneManager.LoadSceneAsync(sceneName);
}
