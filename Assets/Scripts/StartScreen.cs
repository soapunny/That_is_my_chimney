using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickNewGame()
    {
        LoadingSceneController.Instance.LoadScene("MainGame");
    }

    public void OnClickExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        
        Application.Quit();
    }
}