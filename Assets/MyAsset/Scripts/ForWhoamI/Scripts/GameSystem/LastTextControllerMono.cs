#nullable enable

using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using VContainer;

public class LastTextControllerMono: MonoBehaviour
{
    [SerializeField] TextMeshProUGUI thankYouText = null!;
    [SerializeField] GameObject uiCanvas = null!;

    string _thankYouTextValue = "Thank You!!";

    SceneLoader _sceneLoader;

    [Inject]
    public void Construct(SceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }
    
    public void ThankYouAnimation()
    {
        uiCanvas.gameObject.SetActive(false);
        StartCoroutine(TypeText());
    }
    
    IEnumerator TypeText()
    {
        thankYouText.text = "";
        foreach (char c in _thankYouTextValue)
        {
            thankYouText.text += c;
            yield return new WaitForSeconds(0.05f); // 文字ごとの表示間隔
        }

        WaitClickTask().Forget();
    }

    async UniTaskVoid WaitClickTask()
    {
        SoundManagerMono.Instance.PlaySEOneShot(SESoundData.SE.Clear);
        await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
        _sceneLoader.LoadHomeScene();
    }
}