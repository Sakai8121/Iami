using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        // PureC#の登録
        builder.Register<StageHolder>(Lifetime.Scoped);
        
        builder.RegisterEntryPoint<PlayerInputController>();
        
        // MonoBehaviourの登録
        builder.RegisterComponentInHierarchy<PlayerBodyViewMono>();
    }

}