#nullable enable
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class CatchEntitySystem : ITickable
{ 
    bool _isEnabled = true;

    GoalControllerMono _goal;

    [Inject] public CatchEntitySystem()
    {

    }

    public void Tick()
    {
        if (!_isEnabled) return;

        if (Input.GetMouseButtonDown(0)) // ç∂ÉNÉäÉbÉN
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                IEntity? entity = hit.collider.GetComponent<IEntity>();
                entity?.Caught();
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
}
