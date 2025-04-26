# nullable enable
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class TruthCheckExecutor
{
    float checkWaitingTime = 2;

    public event Action EndGameAction = () => { };
    public event Action RestartGameAction = () => { };

    CameraControllerMono _cameraControllerMono;
    LastTextControllerMono _lastTextControllerMono;
    ScoreHolder _scoreHolder;
    TimeHolder _timeHolder;
    StageIndexHolder _stageIndexHolder;
    LastScoreViewMono _lastScoreViewMono;
    
    [Inject]
    public TruthCheckExecutor(CameraControllerMono cameraControllerMono,LastTextControllerMono lastTextControllerMono,
        ScoreHolder scoreHolder,TimeHolder timeHolder,StageIndexHolder stageIndexHolder,LastScoreViewMono lastScoreViewMono)
    {
        _cameraControllerMono = cameraControllerMono;
        _lastTextControllerMono = lastTextControllerMono;
        _scoreHolder = scoreHolder;
        _timeHolder = timeHolder;
        _stageIndexHolder = stageIndexHolder;
        _lastScoreViewMono = lastScoreViewMono;
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

        float clearTime = _timeHolder.EndGameTimer();
        EndGameAction();
        _scoreHolder.SaveHighScore(_stageIndexHolder.CurrentStageIndex,clearTime);
        _lastScoreViewMono.ChangeText(clearTime);
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