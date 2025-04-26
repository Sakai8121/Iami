#nullable enable

using VContainer;
using VContainer.Unity;

public class HomeLifetimeScope:LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        // PureC#の登録
        //builder.Register<StageHolder>(Lifetime.Scoped);

        //InitializedやITickableを使いたいとき、他のクラスから参照されるだけのとき（AsSelfをつける）
        //他のクラスを参照したいときはRegister
        //builder.RegisterEntryPoint<TimeHolder>().AsSelf();

        // MonoBehaviourの登録
        builder.RegisterComponentInHierarchy<StageSelectMono>();
    }
}