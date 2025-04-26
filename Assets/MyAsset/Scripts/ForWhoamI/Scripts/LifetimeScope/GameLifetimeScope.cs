using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        // PureC#の登録
        builder.Register<StageHolder>(Lifetime.Scoped);
        builder.Register<TruthCheckExecutor>(Lifetime.Scoped);
        builder.Register<SceneLoader>(Lifetime.Scoped);
        builder.Register<GameStateObserver>(Lifetime.Scoped);

        //InitializedやITickableを使いたいとき、他のクラスから参照されるだけのとき（AsSelfをつける）
        //他のクラスを参照したいときはRegister
        builder.RegisterEntryPoint<TimeHolder>().AsSelf();
        builder.RegisterEntryPoint<CatchEntitySystem>().AsSelf();
        builder.RegisterEntryPoint<GameInitializer>();
        builder.RegisterEntryPoint<EntityActionExecutor>().AsSelf();

        // MonoBehaviourの登録
        builder.RegisterComponentInHierarchy<TimerTextViewMono>();
        builder.RegisterComponentInHierarchy<EntityFactoryMono>();
        builder.RegisterComponentInHierarchy<StageTitleExecutorMono>();
        builder.RegisterComponentInHierarchy<GoalControllerMono>();
        builder.RegisterComponentInHierarchy<SpaceUIMono>();
        builder.RegisterComponentInHierarchy<EntitySpriteHolderMono>();
        builder.RegisterComponentInHierarchy<StartExplainUIMono>();
        builder.RegisterComponentInHierarchy<EntityInstanceHolderMono>();
        builder.RegisterComponentInHierarchy<CameraControllerMono>();
        builder.RegisterComponentInHierarchy<LastTextControllerMono>();
    }

}