#nullable enable

using DG.Tweening;
using TMPro;
using UnityEngine;

public class StageTitleExecutorMono: MonoBehaviour
{
    [SerializeField] TextMeshProUGUI stageTitleText = null!;
    [SerializeField] RectTransform stageTitleRectTransform = null!;

    [SerializeField] float finalScale = 1f;

    private void Awake()
    {
        var currentColor = stageTitleText.color;
        currentColor.a = 0;
        stageTitleText.color = currentColor;
    }


    public void StageTitleAnimation(StageIndex stageIndex, float animationTime)
    {
        var stageTitle = ConvertStageTitle(stageIndex);
        stageTitleText.text = stageTitle;

        // 初期状態セット（中央表示、拡大）
        stageTitleRectTransform.localScale = Vector3.one;
        stageTitleText.alpha = 0;

        Sequence sequence = DOTween.Sequence();

        // フェードイン
        sequence.Append(stageTitleText.DOFade(1f, animationTime));
    }


    string ConvertStageTitle(StageIndex stageIndex)
    {
        switch (stageIndex)
        {
            case StageIndex.First:
                return "私は拡大する";
            case StageIndex.Second:
                return "私は加速する";
            case StageIndex.Third:
                return "私は反対に回転する";
            case StageIndex.Fourth:
                return "私は向きを変える";
            case StageIndex.Fifth:
                return "私は向きを変える";
            default:
                return "私は拡大する";
        }
    }
}