#nullable enable
using Unity.VisualScripting;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CatchEntitySystem : ITickable
{ 
    bool _isEnabled = false;

    GoalControllerMono _goalControllerMono;
    TruthCheckExecutor _truthCheckExecutor;
    EntityActionExecutor _entityActionExecutor;

    [Inject] 
    public CatchEntitySystem(GoalControllerMono goalControllerMono,EntityActionExecutor entityActionExecutor,
        TruthCheckExecutor truthCheckExecutor)
    {
        _goalControllerMono = goalControllerMono;
        _entityActionExecutor = entityActionExecutor;
        _truthCheckExecutor = truthCheckExecutor;

        DisableSystem();

        truthCheckExecutor.RestartGameAction += EnableSystem;
    }

    public void Tick()
    {
        if (!_isEnabled) return;

        if (Input.GetMouseButtonDown(0)) // 左クリック
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                IEntity? entity = hit.collider.GetComponent<IEntity>();
                if (entity != null)
                {
                    SoundManagerMono.Instance.PlaySEOneShot(SESoundData.SE.Check);
                    
                    entity.Caught(_goalControllerMono.GoalPosition());
                    _truthCheckExecutor.CheckTruth(entity.IsTruth(), entity);

                    DisableSystem();
                    _entityActionExecutor.LockActivate();
                }
            }
        }
    }

    public void EnableSystem()
    {
        _isEnabled = true;
    }

    public void DisableSystem()
    {
        _isEnabled = false;
    }
}
