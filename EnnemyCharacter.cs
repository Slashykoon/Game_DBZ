using Godot;
using System;

public class EnnemyCharacter : KinematicBody2D
{
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
    public AnimationNodeStateMachinePlayback pStateMachine;
    public System.Collections.Generic.List<string> ListSequence= new System.Collections.Generic.List<string>();

    public partial class cStatePlayer
    {
        public bool IsAttacking;
        public bool IsDefending;
        public bool IsEngaging;
        public bool IsHurt;
        public bool bDebugMode;
    
    }
    cStatePlayer StatePlayer = new cStatePlayer();



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
        if (PlayerToTarget != null)
        {
            tmp_vector = PlayerToTarget.GlobalPosition;
        }
        return tmp_vector;
    }

    public override void _Ready()
    {

        HurtArea = (Area2D)GetNode("DamageArea");
        HitBox = (Area2D)GetNode("HitArea");

        AnimPlayer = (AnimationPlayer)GetNode("SpriteAnimation"); // Access via absolute path. //GetTree().Root
        pAnimationTree = (AnimationTree)GetNode("StateMachine");
        pStateMachine = (AnimationNodeStateMachinePlayback) pAnimationTree.Get("parameters/playback");
        pSprite = (Sprite)GetNode("Sprite");
    
        HurtArea.Connect("area_entered", this, "_on_DamageArea_entered");
        HitBox.Connect("area_entered", this, "_on_HitArea_entered");
        AnimPlayer.Connect("animation_finished", this, "_on_Animation_Finished");

        GetNode<Node>("../Body").Connect("HitDamageChanged", this, "OnHitDamageChanged");
    }

    public void OnHitDamageChanged(int DmgRcv)
    {
        GD.Print(DmgRcv);
    }

    public void _on_HitArea_entered(Area2D area)
    {
    }

    public void _on_DamageArea_entered(Area2D area)
    {
        if(area.Name == "HitArea" && pTypeOfChar==(int)eTypeChar.PLAYER)
        {
            GD.Print("PLAYER HIT BY ENNEMY");
            //EmitSignal("HealthChanged", Health); //not used
        }
        if(area.Name == "HitArea" && pTypeOfChar==(int)eTypeChar.ENNEMY)
        {
            GD.Print("ENNEMY hit by PLAYER");
            StatePlayer.IsHurt = true;
            AnimPlayer.Play("Hurt");
            //EmitSignal("HealthChanged", Health); //not used
        }
    }

    public void _on_Animation_Finished(String AnimName)
    {
        if(AnimName == "Stand"){}
        if(AnimName == "Punch"){StatePlayer.IsAttacking=false;}
        if(AnimName == "Punch_2"){StatePlayer.IsAttacking=false;}
        if(AnimName == "Kick"){StatePlayer.IsAttacking=false;}
        if(AnimName == "Hurt"){StatePlayer.IsHurt=false;}
    }

    public void GetSequenceToExecute()
    {
        ListSequence.Add("Punch");
    }


    public void AdjustOrientation()
    {
        if (pTypeOfChar == (int)eTypeChar.PLAYER){ pSprite.FlipH = Velocity.x > 0; }
        if (pTypeOfChar == (int)eTypeChar.PLAYER){ HitBox.RotationDegrees = Velocity.x > 0? 0 : 180; }
        if (pTypeOfChar == (int)eTypeChar.ENNEMY){ pSprite.FlipH = Velocity.x < 0; }
        if (pTypeOfChar == (int)eTypeChar.ENNEMY){ HitBox.RotationDegrees = Velocity.x < 0? 0 : 180; }
    }

    public override void _PhysicsProcess(float delta)
    {
        Velocity= Vector2.Zero;
        if(pTypeOfChar == (int)eTypeChar.PLAYER && StatePlayer.bDebugMode){ TargetPosition = GetGlobalMousePosition(); }
        if(pTypeOfChar == (int)eTypeChar.PLAYER && !StatePlayer.bDebugMode){ TargetPosition = GetTargetPosition(); }
        if(pTypeOfChar == (int)eTypeChar.ENNEMY){ TargetPosition = GetTargetPosition();}

        Velocity = Position.DirectionTo(TargetPosition) * Speed_factor;

        if (Position.DistanceTo(TargetPosition) > 100 && (StatePlayer.IsEngaging||StatePlayer.bDebugMode) && TargetPosition != Vector2.Zero) 
        {
            MoveAndSlide(Velocity,null,false,4,(float)0.785398,false);
            if(!StatePlayer.IsAttacking && !StatePlayer.IsHurt)
            {
                AnimPlayer.Play("Walk");
         
            }
        }
        else if ((Position.DistanceTo(TargetPosition) <= 100 || !StatePlayer.IsEngaging) && !StatePlayer.IsAttacking && !StatePlayer.IsHurt )
        {
            AnimPlayer.Play("Stand");

        }

        AdjustOrientation();
    }



    public override void _Input(InputEvent inputEvent)
    {

        if (inputEvent is InputEventKey keyEvent && keyEvent.Pressed )
        {
            if ((KeyList)keyEvent.Scancode == KeyList.A && pTypeOfChar == (int)eTypeChar.PLAYER)
            {
                AnimPlayer.Play("Punch");
                StatePlayer.IsAttacking = true;
            }
            if ((KeyList)keyEvent.Scancode == KeyList.Z && pTypeOfChar == (int)eTypeChar.PLAYER)
            {
                AnimPlayer.Play("Kick");
                StatePlayer.IsAttacking = true;
            }
            if ((KeyList)keyEvent.Scancode == KeyList.E && pTypeOfChar == (int)eTypeChar.PLAYER)
            {
                StatePlayer.IsEngaging = !StatePlayer.IsEngaging ;
                GD.Print("ENGAGING : " + StatePlayer.IsEngaging );
            }
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
        }
    }
}
