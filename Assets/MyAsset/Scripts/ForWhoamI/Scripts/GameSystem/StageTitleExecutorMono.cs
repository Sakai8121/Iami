#nullable enable

using DG.Tweening;
using TMPro;
using UnityEngine;

public class StageTitleExecutorMono: MonoBehaviour
{
    [SerializeField] TextMeshProUGUI stageTitleText = null!;
    [SerializeField] RectTransform stageTitleRectTransform = null!;

    [SerializeField] Vector2 finalAnchorPos = new Vector2(100, -100); // 左上の目的座標（Canvas上）
    [SerializeField] float finalScale = 1f;

    public void StageTitleAnimation(StageIndex stageIndex, float animationTime)
    {
        var stageTitle = ConvertStageTitle(stageIndex);
        stageTitleText.text = stageTitle;

        // 初期状態セット（中央・拡大）
        stageTitleText.alignment = TextAlignmentOptions.Center;
        stageTitleText.alpha = 0;
        stageTitleRectTransform.localScale = Vector3.one * 2;

        float halfTime = animationTime / 2f;

        Sequence sequence = DOTween.Sequence();

        // ① フェードイン（中央拡大）
        sequence.Append(stageTitleText.DOFade(1f, halfTime));

        // ② 縮小＆左上に移動
        sequence.AppendCallback(() =>
        {
            // 一度フェードアウトしてアライメントを変える
            stageTitleText.DOFade(0f, 0.2f).OnComplete(() =>
            {
                stageTitleText.alignment = TextAlignmentOptions.TopLeft;
                stageTitleText.DOFade(1f, 0.2f);
            });
        });

        sequence.Join(stageTitleRectTransform.DOScale(finalScale, halfTime));
        sequence.Join(stageTitleRectTransform.DOAnchorPos(finalAnchorPos, halfTime));
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