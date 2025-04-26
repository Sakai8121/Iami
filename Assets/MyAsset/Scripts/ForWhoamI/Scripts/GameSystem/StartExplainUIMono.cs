using TMPro;
using UnityEngine;
using System.Collections;

public class StartExplainUIMono : MonoBehaviour
{
    [SerializeField] GameObject startUIObject = null!;
    [SerializeField] TextMeshProUGUI textMesh = null!;

    string clickText = "[Space]キーをヒントに\r\n[クリック]で私を見つけて…";
    Coroutine? typingCoroutine = null;

    public void ActiveStartUI()
    {
        startUIObject.SetActive(true);

        // 前回のコルーチンがあれば停止
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        textMesh.text = "";
        foreach (char c in clickText)
        {
            textMesh.text += c;
            yield return new WaitForSeconds(0.1f); // 文字ごとの表示間隔
        }
    }

    public void DisActiveStartUI()
    {
        startUIObject.SetActive(false);

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
    }


    public void ShowAllText()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        textMesh.text = clickText;
    }
}
