using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void OnClickStartBtn()
    {
        //SceneManager.LoadScene("Level1");
        //SceneManager.LoadScene("PlayScene", LoadSceneMode.Additive);

        SceneManager.LoadScene("SceneLoader");
    }
    public void ExitBtn()
    {
    #if UNITY_EDITOR
        // ����Ƽ ���� ������ �����ϴ� ��� ����
        UnityEditor.EditorApplication.isPlaying = false;
    #else 
        Application.Quit();
    #endif

    }
}
