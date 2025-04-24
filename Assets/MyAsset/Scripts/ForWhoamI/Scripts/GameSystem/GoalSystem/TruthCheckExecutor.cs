# nullable enable
using System;
using Cysharp.Threading.Tasks;
using VContainer;

public class TruthCheckExecutor
{
    float checkWaitingTime = 3;

    public event Action EndGameAction = () => { };
    public event Action RestartGameAction = () => { };

    public void CheckTruth(bool isTruth,IEntity ientity)
    {
        if (isTruth)
        {
            TrueAnimation(ientity).Forget();
        }
        else
        {
            FalseAnimation(ientity).Forget();
        }
    }

    async UniTaskVoid TrueAnimation(IEntity ientity)
    {
        
        await UniTask.Delay(TimeSpan.FromSeconds(checkWaitingTime));
        ientity.SuccessAnimation();
        EndGameAction();
    }

    async UniTaskVoid FalseAnimation(IEntity ientity)
    {
        
        await UniTask.Delay(TimeSpan.FromSeconds(checkWaitingTime));
        ientity.FailAnimation();
        RestartGameAction();
    }
}