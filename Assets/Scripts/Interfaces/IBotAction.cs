using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBotAction
{
    public void Initialize(BotController bot);
    public void Execute();
    bool IsCompleted { get; }
}

