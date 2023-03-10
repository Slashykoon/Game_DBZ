using Godot;
using System;
using System.Collections.Generic;


public class PlayerState : State
{
    public PlayerCharacter Player;
    public override async void _Ready()
    {
        await ToSignal(Owner, "ready");

        Player= Owner as PlayerCharacter;  
    }

    public override void Handle_Input(InputEvent _Event ){}
    public override void Update(float delta){}
    public override void Physics_Update(float delta){}
    public override void Enter(Dictionary<string, string> _msg){}
    public override void Exit(){}

}
