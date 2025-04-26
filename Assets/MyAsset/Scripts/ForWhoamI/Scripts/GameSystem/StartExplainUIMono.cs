using TMPro;
using UnityEngine;
using System.Collections;

public class StartExplainUIMono : MonoBehaviour
{
    [SerializeField] GameObject startUIObject = null!;
    [SerializeField] TextMeshProUGUI textMesh = null!;

    string clickText = "[Space]�L�[���q���g��\r\n[�N���b�N]�Ŏ��������āc";
    Coroutine? typingCoroutine = null;

    public void ActiveStartUI()
    {
        startUIObject.SetActive(true);

        // �O��̃R���[�`��������Β�~
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
            yield return new WaitForSeconds(0.1f); // �������Ƃ̕\���Ԋu
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
