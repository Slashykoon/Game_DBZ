using Godot;
using System;
using System.Collections.Generic;

public class Stand : PlayerState
{

    public override void Update(float delta)
    {
   
    }

    public override void Physics_Update(float delta)
    {
        if ((Player.Position.DistanceTo(Player.TargetPosition) >= 100))// || !Player.StatePlayer.IsEngaging) && !StatePlayer.IsAttacking && !StatePlayer.IsHurt )
        {
            state_machine.Transition_To("Moving",new Dictionary<string, string>()); //envoyer si c'est move dans les air ou ground
        }

        if(Player.ListSequence.Count > 0)
        {
            state_machine.Transition_To("Sequencing",new Dictionary<string, string>());
        }

        if(Mathf.Abs(Player.Position.y - Player.TargetPosition.y) > 80 && (Player.Position.y > Player.TargetPosition.y)) 
        {
            state_machine.Transition_To("JumpingAir",new Dictionary<string, string>());
        }

        if(Mathf.Abs(Player.Position.y - Player.TargetPosition.y) > 80 && (Player.Position.y < Player.TargetPosition.y)) 
        {
            state_machine.Transition_To("FallingDown",new Dictionary<string, string>());
        }

        if(Mathf.Abs(Player.Position.y - Player.TargetPosition.y) > 80 && (Player.Position.y < Player.TargetPosition.y)) 
        {
            state_machine.Transition_To("FallingDown",new Dictionary<string, string>());
        }

        
    }

    public override void Enter(Dictionary<string, string> _msg)
    {
        Player.AnimPlayer.Play("Stand");
    }
    public override void Exit()
    {

    }

}
