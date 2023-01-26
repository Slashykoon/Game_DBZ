using Godot;
using System;
using System.Collections.Generic;

public class StateMachine : Node2D
{

    [Signal] public delegate void Transitioned(string state_name);
    
    [Export]public NodePath InitialStatePath= new NodePath();
    
    public State state;

    public override void _Ready()
    {
        state = (State)GetNode(InitialStatePath);
        foreach(State child in GetChildren())
        {
            child.state_machine = this;
        }
        state.Enter(new Dictionary<string, string>());
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if(state!=null)
        {
            state.Handle_Input(@event);
        }
    }

    public override void _Process(float delta)
    {
        if(state!=null)
        {
            state.Update(delta);
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if(state!=null)
        {
            state.Physics_Update(delta);
        }
    }

    public void Transition_To(String TargetStateName, Dictionary<string,string> msg)
    {
        if (!HasNode(TargetStateName))
        {
            return;
        }
        state.Exit();
        state=(State) GetNode(TargetStateName);
        state.Enter(msg);
        EmitSignal("Transitioned", state.Name);
    }

    

}
