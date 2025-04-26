using UnityEngine;
using VContainer;
using VContainer.Unity;

public class RootLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);
        builder.Register<StageIndexHolder>(Lifetime.Singleton);
        builder.Register<ScoreHolder>(Lifetime.Singleton);
    }
}
