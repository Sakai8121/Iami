using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        // PureC#の登録
        builder.Register<StageHolder>(Lifetime.Scoped);
        builder.Register<PlayerBodyStateHolder>(Lifetime.Scoped);
        builder.Register<PlayerHandStateHolder>(Lifetime.Scoped);
        builder.Register<BodyPhysicsDataCalculator>(Lifetime.Scoped);
        
        //InitializedやITickableを使いたいとき、他のクラスから参照されるだけのとき（AsSelfをつける）
        //他のクラスを参照したいときはRegister
        builder.RegisterEntryPoint<PlayerInputController>().AsSelf();
        builder.RegisterEntryPoint<PlayerBodyStateObserver>();
        builder.RegisterEntryPoint<PlayerHandStateObserver>();
        builder.RegisterEntryPoint<BodyDataObserver>();

        // MonoBehaviourの登録
        builder.RegisterComponentInHierarchy<PlayerBodyViewMono>();
        builder.RegisterComponentInHierarchy<BodyTransformHolderMono>();
    }

}