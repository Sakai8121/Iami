#nullable enable

using System;
using TMPro;
using UnityEngine;

public class LastScoreViewMono: MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;

    void Awake()
    {
        scoreText.text = "";
    }

    public void ChangeText(float time)
    {
        scoreText.gameObject.SetActive(true);
        scoreText.text = time.ToString("F2");
    }
}