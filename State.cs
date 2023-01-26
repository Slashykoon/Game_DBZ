using Godot;
using System;
using System.Collections.Generic;

public class State : Node2D
{

    public StateMachine state_machine = null;

    public virtual void Handle_Input(InputEvent _Event ){}
    public virtual void Update(float delta){}
    public virtual void Physics_Update(float delta){}
    public virtual void Enter(Dictionary<string, string> _msg){}
    public virtual void Exit(){}

}
