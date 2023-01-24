using Godot;
using System;

public class Scene_Fight : Node2D
{
    private enum eTypeChar{
        PLAYER,
        ENNEMY
    }
    public PackedScene PackedPlayerChar = ResourceLoader.Load<PackedScene>("res://PlayerCharacter.tscn");
    public PackedScene PackedEnnemyChar = ResourceLoader.Load<PackedScene>("res://EnnemyCharacter.tscn");

    public PlayerCharacter PlayerCharInstance;
    public EnnemyCharacter EnnemyCharInstance;
    //public PlayerCharacter EnnemyCharInstance;

    public Camera2D CameraSceneNode;

    public override void _Ready()
    {
        CameraSceneNode= (Camera2D) GetNode("./CameraScene");

        PackedPlayerChar.Instance();
        PackedEnnemyChar.Instance();

        PlayerCharInstance = (PlayerCharacter)PackedPlayerChar.Instance();
        EnnemyCharInstance = (EnnemyCharacter)PackedEnnemyChar.Instance();
        //EnnemyCharInstance = (PlayerCharacter)PackedEnnemyChar.Instance();

        PlayerCharInstance.InitCharacter(100,100,210,(int)eTypeChar.PLAYER,new Vector2(200,200));
        EnnemyCharInstance.InitCharacter(100,100,210,(int)eTypeChar.ENNEMY,new Vector2(850,200));
        AddChild(PlayerCharInstance);
        AddChild(EnnemyCharInstance);

    }

    public override void _Process(float delta)
    {
        
    }
}
