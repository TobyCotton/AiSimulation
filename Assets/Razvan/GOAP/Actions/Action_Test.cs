using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Test : Action
{
    override public bool PrePerform()
    {
        return true;
    }
    override public bool PostPerform()
    {
        return true;
    }
}
