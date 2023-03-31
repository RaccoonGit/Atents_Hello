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

    // 코루틴으로 Start 함수 호출
    private IEnumerator Start()
    {
        InitSceneInfo();
        fadeCG.alpha = 1.0f;
        foreach(var _loadScene in loadScenes)
        {
            yield return StartCoroutine(LoadScene(_loadScene.Key, _loadScene.Value));
        }

        // fade in 함수 호출
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
        // 절대값 함수로 백분율을 계산
        float fadeSpeed = Mathf.Abs(fadeCG.alpha - finalAlpha) / fadeDuration;
        // 알파 값 조정
        while(!Mathf.Approximately(fadeCG.alpha, finalAlpha))
        {
            // MoveTowards 함수는 Lerp 함수와 동일 한 함수로 주로 알파값 보간에  쓰인다.
            fadeCG.alpha = Mathf.MoveTowards(fadeCG.alpha, finalAlpha, fadeSpeed * Time.deltaTime);
            yield return null;
        }
        fadeCG.blocksRaycasts = false;

        // fade in 이 완료된 후 SceneLoader 씬은 삭제(Unload)
        SceneManager.UnloadSceneAsync("SceneLoader");
    }
}
