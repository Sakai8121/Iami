using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        // PureC#の登録
        builder.Register<StageHolder>(Lifetime.Scoped);
        
        //InitializedやITickableを使いたいとき、他のクラスから参照されるだけのとき（AsSelfをつける）
        //他のクラスを参照したいときはRegister
        builder.RegisterEntryPoint<TimeHolder>().AsSelf();
        builder.RegisterEntryPoint<GameStateManager>();

        // MonoBehaviourの登録
        builder.RegisterComponentInHierarchy<TimerTextViewMono>();
        builder.RegisterComponentInHierarchy<EntityFactoryMono>();
        builder.RegisterComponentInHierarchy<StageTitleExecutorMono>();
    }

}