using System;

[Serializable]
public class PauseObject
{
    public string behavior;
    public bool pause = false;

    public PauseObject(string behaviorName)
    {
        this.behavior = behaviorName;
    }
}