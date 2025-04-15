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

        builder.RegisterEntryPoint<PlayerInputController>();
        builder.RegisterEntryPoint<PlayerBodyStateObserver>();
        builder.RegisterEntryPoint<PlayerHandStateObsever>();
        builder.RegisterEntryPoint<BodyDataObserver>();

        // MonoBehaviourの登録
        builder.RegisterComponentInHierarchy<PlayerBodyViewMono>();
    }

}