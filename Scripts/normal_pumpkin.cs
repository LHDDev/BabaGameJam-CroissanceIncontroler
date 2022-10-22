using Godot;
using System;

public partial class normal_pumpkin : StaticBody2D
{
	[Export]
	private AnimatedSprite2D _pumpkinSprite;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Destroyed(){
		_pumpkinSprite.Animation = "Death";
	}

}
