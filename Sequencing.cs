using Godot;
using System;
using System.Collections.Generic;

public class Sequencing : PlayerState
{
    
    public override void Update(float delta)
    {
    }

    public override void Physics_Update(float delta)
    {

        if(Player.ListSequence.Count > 0)
        {
            string ListSequence_Peek = (string)Player.ListSequence.Peek();
            Dictionary<string,string> DictData =new Dictionary<string, string>();
            DictData.Add("CurrentActionInSequence",ListSequence_Peek);
            switch(ListSequence_Peek) 
            {
                case "Punch": 
                        state_machine.Transition_To("Attack",DictData);
                    break;
                case "Punch_2":
                        state_machine.Transition_To("Attack",DictData);
                    break;
                case "Kick":
                        state_machine.Transition_To("Attack",DictData);
                    break;
                case "START_ENGAGING":
                        Player.ListSequence.Pop();
                        state_machine.Transition_To("Standing",DictData);
                    break;
                default:
                    break;
            }
        }
        else
        {
            state_machine.Transition_To("Standing",new Dictionary<string, string>());
        }
    }

    public override void Enter(Dictionary<string, string> _msg)
    {
    }
    public override void Exit()
    {
    }

}
