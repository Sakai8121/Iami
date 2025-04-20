#nullable enable

using VContainer;

public class GameStateManager
{
    TimeHolder _timeHolder;
    EntityFactoryMono _entityFactoryMono;
    
    [Inject]
    public GameStateManager(TimeHolder timeHolder, EntityFactoryMono entityFactoryMono)
    {
        _timeHolder = timeHolder;
        _entityFactoryMono = entityFactoryMono;
        
        
    }
    
    
}

public enum GameState
{
    Pause,
    Restart,
}