
using VContainer;
using VContainer.Unity;
using UnityEngine;
using UnityEngine.UIElements;

public class BodyDataObserver : IInitializable
{
    PlayerBodyViewMono _bodyViewMono;
    BodyPhysicsDataCalculator _calculator;

    [Inject]
    public BodyDataObserver(PlayerBodyViewMono bodyViewMono,BodyPhysicsDataCalculator bodyPhysicsDataCalculator)
    {
        _bodyViewMono = bodyViewMono;
        _calculator = bodyPhysicsDataCalculator;
    }

    public void Initialize()
    {
        _calculator.BodyScale.OnValueChanged += scale =>
        {
            _bodyViewMono.ChangeBodyView(new Vector2(scale.x, scale.y));
        };

        _calculator.BodyRotation.OnValueChanged += rotation =>
        {
            _bodyViewMono.ChangeRotation(rotation);
        };
    }
}