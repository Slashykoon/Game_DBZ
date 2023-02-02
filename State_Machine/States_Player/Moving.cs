using Godot;
using System;
using System.Collections.Generic;

public class Moving : PlayerState
{

    public override void Update(float delta)
    {
        Player.Speed_factor=230;
        Player.Velocity = Player.Position.DirectionTo(Player.TargetPosition) * Player.Speed_factor;
        Player.MoveAndSlide(Player.Velocity,null,false,4,(float)0.785398,false);
    }

    public override void Physics_Update(float delta)
    {
        if (Player.Position.DistanceTo(Player.TargetPosition) < 100 )
        {
             state_machine.Transition_To("Standing",new Dictionary<string, string>());
        }
    }

    public override void Enter(Dictionary<string, string> _msg)
    {
        Player.AnimPlayer.Play("Walk");
    }
    public override void Exit()
    {

    }

}
