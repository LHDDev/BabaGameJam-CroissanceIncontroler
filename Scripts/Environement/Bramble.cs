using Godot;
using System;

public partial class Bramble : Node2D
{
	[Export]
	public bool IsThorned = false;
	[Export]
	private Sprite2D _thorn;
	[Export]
	private Area2D _hitBox;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_hitBox.AreaEntered += OnAreaEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_thorn.Visible = IsThorned;
		_hitBox.Monitoring = IsThorned;
	}

	public void OnAreaEntered(Area2D entertedArea){
		if(entertedArea.GetGroups().Contains("hurtbox_player")){
			((PlayerHurtBox)entertedArea).Hurt();
		}
	}

}
