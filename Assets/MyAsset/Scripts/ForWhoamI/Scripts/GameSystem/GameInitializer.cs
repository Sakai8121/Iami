#nullable enable

using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameInitializer:IInitializable
{
    TimeHolder _timeHolder;
    EntityFactoryMono _entityFactoryMono;
    StageTitleExecutorMono _stageTitleExecutorMono;
    GameStateObserver _gameStateObserver;
    StartExplainUIMono _startExplainUIMono;
    EntityInstanceHolderMono _entityInstanceHolder;
    CatchEntitySystem _entityCatchSystem;

    [Inject]
    public GameInitializer(GameStateObserver gameStateObserver,StageIndexHolder stageIndexHolder,TimeHolder timeHolder,
        EntityFactoryMono entityFactoryMono,StageTitleExecutorMono stageTitleExecutorMono,GoalControllerMono goalControllerMono,
        EntitySpriteHolderMono entitySpriteHolderMono,StartExplainUIMono startExplainUIMono,EntityInstanceHolderMono entityInstanceHolder,
        CatchEntitySystem catchEntity)
    {
        _timeHolder = timeHolder;
        _entityFactoryMono = entityFactoryMono;
        _stageTitleExecutorMono = stageTitleExecutorMono;
        _gameStateObserver = gameStateObserver;
        _startExplainUIMono = startExplainUIMono;
        _entityInstanceHolder = entityInstanceHolder;
        _entityCatchSystem = catchEntity;

        var currentStage = stageIndexHolder.CurrentStageIndex;

        gameStateObserver.Subscribe(GameState.InGame, StartGame);

        var stageEntity = ConvertStageToEntity(currentStage);

        goalControllerMono.SetGoalImage(entitySpriteHolderMono.GetEntitySprite(stageEntity));

        _entityFactoryMono.GenerateEntities(stageEntity);
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
        _startExplainUIMono.ActiveStartUI();
        var animationTime = 2.0f;
        _stageTitleExecutorMono.StageTitleAnimation(stageIndex,animationTime);

        // どちらか早く完了した方を待つ
        await UniTask.WhenAny(
            UniTask.Delay(TimeSpan.FromSeconds(3.0f)),
            UniTask.WaitUntil(() => Input.GetMouseButtonDown(0))); // 左クリック

        _startExplainUIMono.ShowAllText();

        await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
        
        _startExplainUIMono.DisActiveStartUI();
        _entityInstanceHolder.VisibleEntities();
        _gameStateObserver.CurrentState = GameState.InGame;
    }

    void StartGame()
    {
        _timeHolder.StartTimer();

        EnableTask().Forget();
    }

    async UniTaskVoid EnableTask()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        _entityCatchSystem.EnableSystem();
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