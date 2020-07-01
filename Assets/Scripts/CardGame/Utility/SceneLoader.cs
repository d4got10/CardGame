using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void OnClick_LoadScene(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }
}
