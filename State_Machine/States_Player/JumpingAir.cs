using Godot;
using System;
using System.Collections.Generic;

public class JumpingAir : PlayerState
{

    public override void Update(float delta)
    {
    }

    public override void Physics_Update(float delta)
    {
        if (Mathf.Abs(Player.Position.y - Player.TargetPosition.y) > 8)
        { 
            Player.Velocity = Player.Position.DirectionTo(new Vector2(Player.Position.x,Player.TargetPosition.y)) * Player.Speed_factor;
            Player.MoveAndSlide(Player.Velocity,null,false,4,(float)0.785398,false);
        }
        else
        {
            state_machine.Transition_To("Standing",new Dictionary<string, string>());
        }
        //state_machine.Transition_To("Sequencing",new Dictionary<string, string>());
    }

    public override void Enter(Dictionary<string, string> _msg)
    {
        Player.Speed_factor = 650;
        Player.AnimPlayer.Play("GroundToAir");
    }
    public override void Exit()
    {

    }

}
