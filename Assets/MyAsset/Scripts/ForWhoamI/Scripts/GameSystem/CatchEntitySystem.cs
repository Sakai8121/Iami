#nullable enable
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class CatchEntitySystem : ITickable
{ 
    bool _isEnabled = true;

    GoalControllerMono _goalControllerMono;

    [Inject] 
    public CatchEntitySystem(GoalControllerMono goalControllerMono)
    {
        _goalControllerMono = goalControllerMono;
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
                entity?.Caught(CalculateTargetPosition());
                DisableSystem();
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

    Vector2 CalculateTargetPosition()
    {
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, 
            _goalControllerMono.GoalPosition());

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_goalControllerMono.GoalRect, screenPos, 
                Camera.main, out var worldPos))
        {
            return worldPos;
        }
        else
        {
            Debug.LogError("何かがNullになってる");
            return Vector2.zero;
        }
    }
}
