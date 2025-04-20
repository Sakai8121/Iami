#nullable enable

public class StageIndexHolder
{
    public StageIndex CurrentStageIndex => _currentStageIndex;

    StageIndex _currentStageIndex;

    public void SetStageIndex(StageIndex nextIndex)
    {
        _currentStageIndex = nextIndex;
    }
}

public enum StageIndex
{
    First,
    Second,
    Third,
    Fourth,
    Fifth
}