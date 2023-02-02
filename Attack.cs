using Godot;
using System;
using System.Collections.Generic;

public class Attack : PlayerState
{
    private bool bWaitAnimFinished;
    private string mCurrentAction;
    public override void Update(float delta)
    {

    }

    public void SignalAnimationFinished(String AnimName)
    {
        if (mCurrentAction == "Punch" || mCurrentAction == "Punch_2" || mCurrentAction == "Kick")
        {
            state_machine.Transition_To("Sequencing",new Dictionary<string, string>());
        }
    }

    public override void Physics_Update(float delta)
    {

        if(Player.ListSequence.Count > 0)
        {
            switch(mCurrentAction) 
            {
                case "Punch":
                    if (!(mCurrentAction is null) && !bWaitAnimFinished)
                    {
                        bWaitAnimFinished=true;
                        EmitSignal("HitDamageChanged",5);
                        Player.ListSequence.Pop();
                        Player.AnimPlayer.Play("Punch");
                    }
                    break;
                case "Punch_2":
                    if (!(mCurrentAction is null) && !bWaitAnimFinished)
                    {
                        bWaitAnimFinished=true;
                        EmitSignal("HitDamageChanged",8);
                        Player.ListSequence.Pop();
                        Player.AnimPlayer.Play("Punch_2");
                    
                    }
                    break;
                case "Kick":
                    if (!(mCurrentAction is null) && !bWaitAnimFinished)
                    {
                        bWaitAnimFinished=true;
                        EmitSignal("HitDamageChanged",10);
                        Player.ListSequence.Pop();
                        Player.AnimPlayer.Play("Kick");
        
                    }
                    break;
                default:
                    break;
            }
        }

    }

    public override void Enter(Dictionary<string, string> _msg)
    {
        //mAnimFinished= false;
        mCurrentAction= _msg["CurrentActionInSequence"];
        Player.Connect("SignalAnimationEnd", this, "SignalAnimationFinished");

    }
    public override void Exit()
    {
        mCurrentAction = null;
        bWaitAnimFinished= false;
    }

}
