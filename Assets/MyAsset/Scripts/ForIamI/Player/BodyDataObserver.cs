
using VContainer;
using VContainer.Unity;
using UnityEngine;

public class BodyDataObserver : IInitializable
{
    [Inject]
    public BodyDataObserver(PlayerBodyViewMono bodyViewMono,BodyPhysicsDataCalculator bodyPhysicsDataCalculator)
    {
        bodyPhysicsDataCalculator.BodyScale.OnValueChanged += scale =>
        {
            bodyViewMono.ChangeBodyView(new Vector2(scale.x, scale.y));
        };

        bodyPhysicsDataCalculator.BodyRotation.OnValueChanged += rotation =>
        {
            bodyViewMono.ChangeRotation(rotation);
        };
        
        bodyPhysicsDataCalculator.BodyPosition.OnValueChanged += position =>
        {
            bodyViewMono.ChangePosition(position);
        };
    }
    
    //エントリーポイント用
    public void Initialize(){}
}