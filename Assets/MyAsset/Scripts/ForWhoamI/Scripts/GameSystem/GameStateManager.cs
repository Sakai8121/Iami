#nullable enable

using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameStateManager:IInitializable
{
    TimeHolder _timeHolder;
    EntityFactoryMono _entityFactoryMono;
    StageTitleExecutorMono _stageTitleExecutorMono;

    GameState _currentGameState;
    
    [Inject]
    public GameStateManager(StageIndexHolder stageIndexHolder,TimeHolder timeHolder,
        EntityFactoryMono entityFactoryMono,StageTitleExecutorMono stageTitleExecutorMono)
    {
        _timeHolder = timeHolder;
        _entityFactoryMono = entityFactoryMono;
        _stageTitleExecutorMono = stageTitleExecutorMono;

        var currentStage = stageIndexHolder.CurrentStageIndex;
        _currentGameState = GameState.ShowStageTitle;

        _entityFactoryMono.GenerateEntities(ConvertStageToEntity(currentStage));
        ShowStageTitleTask(currentStage).Forget();
    }

    EntityKind ConvertStageToEntity(StageIndex stageIndex)
    {
        switch (stageIndex)
        {
            case StageIndex.First:
                return EntityKind.Square;
            case StageIndex.Second:
                return EntityKind.Heart;
            case StageIndex.Third:
                return EntityKind.Gear;
            case StageIndex.Fourth:
                return EntityKind.Star;
            case StageIndex.Fifth:
                return EntityKind.Circle;
            default:
                return EntityKind.Square;
        }
    }

    async UniTaskVoid ShowStageTitleTask(StageIndex stageIndex)
    {
        var animationTime = 3.0f;
        _stageTitleExecutorMono.StageTitleAnimation(stageIndex,animationTime);
        await UniTask.Delay(TimeSpan.FromSeconds(animationTime));
        await UniTask.WaitUntil(() =>
            Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));

        _currentGameState = GameState.InGame;
        StartGame();
    }

    void StartGame()
    {
        _timeHolder.StartTimer();
    }

    //エントリーポイント用
    public void Initialize(){}
}

public enum GameState
{
    ShowStageTitle,
    InGame,
    Pause,
    CheckI
}