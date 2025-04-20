#nullable enable

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

public class StageSelectMono: MonoBehaviour
{
    [SerializeField] Button stage1Button = null!;
    [SerializeField] Button stage2Button = null!;
    [SerializeField] Button stage3Button = null!;
    [SerializeField] Button stage4Button = null!;
    [SerializeField] Button stage5Button = null!;

    [Inject]
    public void Construct(StageIndexHolder stageIndexHolder)
    {
        stage1Button.onClick.AddListener(()=>
        {
            stageIndexHolder.SetStageIndex(StageIndex.First);
            LoadGameScene();
        });
        stage2Button.onClick.AddListener(()=>
        {
            stageIndexHolder.SetStageIndex(StageIndex.Second);
            LoadGameScene();
        });
        stage3Button.onClick.AddListener(()=>
        {
            stageIndexHolder.SetStageIndex(StageIndex.Third);
            LoadGameScene();
        });
        stage4Button.onClick.AddListener(()=>
        {
            stageIndexHolder.SetStageIndex(StageIndex.Fourth);
            LoadGameScene();
        });
        stage5Button.onClick.AddListener(()=>
        {
            stageIndexHolder.SetStageIndex(StageIndex.Fifth);
            LoadGameScene();
        });
    }

    void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}