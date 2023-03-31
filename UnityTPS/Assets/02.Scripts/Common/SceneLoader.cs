using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public CanvasGroup fadeCG;
    [Range(0.5f, 2.0f)]
    public float fadeDuration = 1.0f;

    public Dictionary<string, LoadSceneMode> loadScenes = new Dictionary<string, LoadSceneMode>();

    void InitSceneInfo()
    {
        SceneManager.LoadScene("Level1");
        SceneManager.LoadScene("PlayScene", LoadSceneMode.Additive);
    }

    // �ڷ�ƾ���� Start �Լ� ȣ��
    private IEnumerator Start()
    {
        InitSceneInfo();
        fadeCG.alpha = 1.0f;
        foreach(var _loadScene in loadScenes)
        {
            yield return StartCoroutine(LoadScene(_loadScene.Key, _loadScene.Value));
        }

        // fade in �Լ� ȣ��
        StartCoroutine(Fade(0.0f));
    }

    IEnumerator LoadScene(string sceneName, LoadSceneMode mode)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, mode);

        Scene loaddedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(loaddedScene);
    }

    IEnumerator Fade(float finalAlpha)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level1"));
        fadeCG.blocksRaycasts = true;
        // ���밪 �Լ��� ������� ���
        float fadeSpeed = Mathf.Abs(fadeCG.alpha - finalAlpha) / fadeDuration;
        // ���� �� ����
        while(!Mathf.Approximately(fadeCG.alpha, finalAlpha))
        {
            // MoveTowards �Լ��� Lerp �Լ��� ���� �� �Լ��� �ַ� ���İ� ������  ���δ�.
            fadeCG.alpha = Mathf.MoveTowards(fadeCG.alpha, finalAlpha, fadeSpeed * Time.deltaTime);
            yield return null;
        }
        fadeCG.blocksRaycasts = false;

        // fade in �� �Ϸ�� �� SceneLoader ���� ����(Unload)
        SceneManager.UnloadSceneAsync("SceneLoader");
    }
}
