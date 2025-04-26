# nullable enable
using System;
using Cysharp.Threading.Tasks;
using VContainer;

public class TruthCheckExecutor
{
    float checkWaitingTime = 2;

    public event Action EndGameAction = () => { };
    public event Action RestartGameAction = () => { };

    CameraControllerMono _cameraControllerMono;
    LastTextControllerMono _lastTextControllerMono;
    
    [Inject]
    public TruthCheckExecutor(CameraControllerMono cameraControllerMono,LastTextControllerMono lastTextControllerMono)
    {
        _cameraControllerMono = cameraControllerMono;
        _lastTextControllerMono = lastTextControllerMono;
    }

    public void CheckTruth(bool isTruth,IEntity ientity)
    {
        if (isTruth)
        {
            TrueAnimation(ientity).Forget();
            _cameraControllerMono.SuccessCameraAnimation(checkWaitingTime);
        }
        else
        {
            FalseAnimation(ientity).Forget();
            _cameraControllerMono.FailCameraAnimation(checkWaitingTime);
        }
    }

    async UniTaskVoid TrueAnimation(IEntity ientity)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(checkWaitingTime));
        ientity.SuccessAnimation();
        EndGameAction();
        _lastTextControllerMono.ThankYouAnimation();
        EyeSoundTask().Forget();
    }

    async UniTaskVoid EyeSoundTask()
    {
        SoundManagerMono.Instance.PlaySEOneShot(SESoundData.SE.Eye);
        await UniTask.Delay(TimeSpan.FromSeconds(0.7f));
        SoundManagerMono.Instance.PlaySEOneShot(SESoundData.SE.Eye);
        await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
        SoundManagerMono.Instance.PlaySEOneShot(SESoundData.SE.Eye);
    }

    async UniTaskVoid FalseAnimation(IEntity ientity)
    {
        
        await UniTask.Delay(TimeSpan.FromSeconds(checkWaitingTime));
        
        SoundManagerMono.Instance.PlaySEOneShot(SESoundData.SE.Miss);
        
        ientity.FailAnimation();
        RestartGameAction();
    }
}