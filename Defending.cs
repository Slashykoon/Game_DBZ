using Godot;
using System;
using System.Collections.Generic;

public class Defending : PlayerState
{

    public override void Update(float delta)
    {
    }

    public void SignalAnimationFinished(String AnimName)
    {
        state_machine.Transition_To("Standing",new Dictionary<string, string>());
    }

    public override void Physics_Update(float delta)
    {
    }

    public override void Enter(Dictionary<string, string> _msg)
    {
        Player.Connect("SignalAnimationEnd", this, "SignalAnimationFinished");
        //GetNode<Node>("../Body").Connect("HitDamageChanged", this, "OnHitDamageChanged");
        Player.RandomGen.Randomize();
        //uint rdmvaluedefense = GD.Randi() % Player.DefenseFactor;

        string[] AnimationDefense = { "StandBlock", "StandBlock2" };
        string StrAnimToPlay;
        
        StrAnimToPlay = AnimationDefense[GD.Randi() % AnimationDefense.Length];
        Player.AnimPlayer.Play(StrAnimToPlay);

    }
    public override void Exit()
    {

    }

}
