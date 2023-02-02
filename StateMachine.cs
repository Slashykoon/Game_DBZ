using Godot;
using System;
using System.Collections.Generic;

public class StateMachine : Node
{

    [Signal] public delegate void Transitioned(string state_name);
    
    [Export]public NodePath InitialStatePath= new NodePath();
    
    public State ActiveState;

    public override async void _Ready()
    {
        await ToSignal(Owner, "ready");
        ActiveState = (State)GetNode(InitialStatePath);
        foreach(State child in GetChildren())
        {
            child.state_machine = this;
        }
        ActiveState.Enter(new Dictionary<string, string>());
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if(ActiveState!=null)
        {
            ActiveState.Handle_Input(@event);
        }
    }

    public override void _Process(float delta)
    {
        if(ActiveState!=null)
        {
            ActiveState.Update(delta);
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if(ActiveState!=null)
        {
            ActiveState.Physics_Update(delta);
        }
    }

    public void Transition_To(String TargetStateName, Dictionary<string,string> msg)
    {
        if (!HasNode(TargetStateName))
        {
            return;
        }
        ActiveState.Exit();
        ActiveState=(State) GetNode(TargetStateName);
        ActiveState.Enter(msg);
        EmitSignal("Transitioned", ActiveState.Name);
    }

    

}
