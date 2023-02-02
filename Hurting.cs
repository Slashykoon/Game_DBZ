using Godot;
using System;
using System.Collections.Generic;

public class Hurting : PlayerState
{

    public override void Update(float delta)
    {
    }

    public void SignalAnimationFinished(String AnimName)
    {
        //if (mCurrentAction == "Punch" || mCurrentAction == "Punch_2" || mCurrentAction == "Kick")
        //{
            state_machine.Transition_To("Standing",new Dictionary<string, string>());
        //}
    }
    
    public override void Physics_Update(float delta)
    {
    }

    public override void Enter(Dictionary<string, string> _msg)
    {

        Player.Connect("SignalAnimationEnd", this, "SignalAnimationFinished");

        Player.RandomGen.Randomize();

        string[] AnimationHurt = { "Hurt", "Hurt2" };
        string StrAnimToPlay;

        StrAnimToPlay = AnimationHurt[GD.Randi() % AnimationHurt.Length];
        Player.AnimPlayer.Play(StrAnimToPlay);
    }
    
    public override void Exit()
    {
    }

}
