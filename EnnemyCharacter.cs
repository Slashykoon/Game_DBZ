using Godot;
using System;
using System.Collections;

public class EnnemyCharacter : KinematicBody2D
{
   [Signal] public delegate void HitDamageChanged(int newDmg);
    [Signal] public delegate void SignalAnimationEnd(string AnimName);
    [Signal] public delegate void SignalDamageAreEntered();


    private enum eTypeChar{
        PLAYER,
        ENNEMY
    }
    public int pTypeOfChar;
    public int Ki_level;
    public int Health;
    public int Speed_factor;
    public Vector2 Velocity;
    public Vector2 TargetPosition;
    public AnimationPlayer AnimPlayer;
    public Area2D HurtArea;
    public Area2D HitBox;
    public Sprite pSprite;
    public AnimationTree pAnimationTree;
    
    public Stack ListSequence = new Stack();

    public partial class cStatePlayer
    {
        public bool IsAttacking;
        public bool IsDefending;
        public bool IsEngaging;
        public bool IsHurt;
        public bool IsCollided;
        public bool bDebugMode;
        public bool IsJumping;
        public bool IsFalling;
    
    }
    public cStatePlayer StatePlayer = new cStatePlayer();
    public StateMachine pStateMachine;
    public RandomNumberGenerator RandomGen;
    public uint DefenseFactor = 5;
 

    public void InitCharacter(int pHealth, int pKiLvl, int pSpeedFactor , short TypeOfChar, Vector2 PositionCoord)
    {
        Health = pHealth;
        Ki_level = pKiLvl;
        Speed_factor = pSpeedFactor;
        pTypeOfChar = TypeOfChar;
        this.GlobalPosition = PositionCoord;
    }

    public void SetTargetPosition(KinematicBody2D Target)
    {
        TargetPosition = Target.Position;
    }

    public Vector2 GetTargetPosition()
    {
        Vector2 tmp_vector = Vector2.Zero;
        KinematicBody2D PlayerToTarget = null;
        if(pTypeOfChar == (int)eTypeChar.ENNEMY){ 
            PlayerToTarget= (KinematicBody2D) GetNodeOrNull("../Body");
        }
        else if (pTypeOfChar == (int)eTypeChar.PLAYER){
            PlayerToTarget= (KinematicBody2D) GetNodeOrNull("../BodyEnnemy");
        }
        if (PlayerToTarget != null){ tmp_vector = PlayerToTarget.GlobalPosition; }
        return tmp_vector;
    }

    public override void _Ready()
    {  
        HurtArea = (Area2D)GetNode("DamageArea");
        HitBox = (Area2D)GetNode("HitArea");

        AnimPlayer = (AnimationPlayer)GetNode("SpriteAnimation"); 
        pSprite = (Sprite)GetNode("Sprite");
        pStateMachine = (StateMachine)GetNode("StateMachine");

        HurtArea.Connect("area_entered", this, "_on_DamageArea_entered");
        HitBox.Connect("area_entered", this, "_on_HitArea_entered");
        AnimPlayer.Connect("animation_finished", this, "_on_Animation_Finished");

        RandomGen = new RandomNumberGenerator();

        pStateMachine.Connect("Transitioned", this, "On_Transition_End");
    }

    public void On_Transition_End(String ActiveStateName)
    {
        GD.Print(ActiveStateName);
    }
    public void _on_HitArea_entered(Area2D area)
    {
    }

    public void _on_DamageArea_entered(Area2D area)
    {
        if(area.Name == "HitArea" && pTypeOfChar==(int)eTypeChar.PLAYER)
        {
            GD.Print("PLAYER HIT BY ENNEMY");
        }
        if(area.Name == "HitArea" && pTypeOfChar==(int)eTypeChar.ENNEMY)
        {
            GD.Print("ENNEMY hit by PLAYER");
            EmitSignal("SignalDamageAreEntered");
        }
    }

    public void _on_Animation_Finished(String AnimName)
    {
        EmitSignal("SignalAnimationEnd", AnimName);
    }


    public void GetSequenceToExecute()
    {
        ListSequence.Push("Punch_2");
        ListSequence.Push("Kick");
        ListSequence.Push("Punch");
        ListSequence.Push("Punch");
        ListSequence.Push("Kick");
        ListSequence.Push("Kick");
        ListSequence.Push("Punch");
        ListSequence.Push("START_ENGAGING");
        //ListSequence.Push("SET_MOVING_SPEED:250");
    }


    public void AdjustOrientation()
    {
        if (pTypeOfChar == (int)eTypeChar.PLAYER){ pSprite.FlipH = Velocity.x > -1; }
        if (pTypeOfChar == (int)eTypeChar.PLAYER){ HitBox.RotationDegrees = Velocity.x > 0? 0 : 180; }
        if (pTypeOfChar == (int)eTypeChar.ENNEMY){ pSprite.FlipH = Velocity.x < -1; }
        if (pTypeOfChar == (int)eTypeChar.ENNEMY){ HitBox.RotationDegrees = Velocity.x < 0? 0 : 180; }
    }

    public override void _PhysicsProcess(float delta)
    {
        if(pTypeOfChar == (int)eTypeChar.PLAYER && StatePlayer.bDebugMode){ TargetPosition = GetGlobalMousePosition(); }
        if(pTypeOfChar == (int)eTypeChar.PLAYER && !StatePlayer.bDebugMode){ TargetPosition = GetTargetPosition(); }
        if(pTypeOfChar == (int)eTypeChar.ENNEMY){ TargetPosition = GetTargetPosition();}

        if (StatePlayer.bDebugMode)
        {
            Velocity = Position.DirectionTo(TargetPosition) * Speed_factor;
            MoveAndSlide(Velocity,null,false,4,(float)0.785398,false);
        }
        /*GD.Print(Position.y);
        GD.Print(TargetPosition.y);
        GD.Print(Mathf.Abs(Position.y - TargetPosition.y));
        GD.Print("---------");*/

        AdjustOrientation();
    }


    public override void _Input(InputEvent inputEvent)
    {

        if (inputEvent is InputEventKey keyEvent && keyEvent.Pressed )
        {

            if ((KeyList)keyEvent.Scancode == KeyList.R && pTypeOfChar == (int)eTypeChar.ENNEMY)
            {
                StatePlayer.IsEngaging = !StatePlayer.IsEngaging ;
                GD.Print("ENGAGING : " + StatePlayer.IsEngaging );
            }
            if ((KeyList)keyEvent.Scancode == KeyList.D && pTypeOfChar == (int)eTypeChar.PLAYER)
            {
                StatePlayer.bDebugMode = !StatePlayer.bDebugMode;
                GD.Print("DEBUG MODE : " + StatePlayer.bDebugMode);
            }
            if ((KeyList)keyEvent.Scancode == KeyList.S && pTypeOfChar == (int)eTypeChar.PLAYER)
            {
                GetSequenceToExecute();
                GD.Print("SEQUENCE LOADED : " );
            }
        }
    }
}
