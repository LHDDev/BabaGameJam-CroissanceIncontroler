using Godot;
using System;

public partial class PlayerHurtBox : Area2D
{
	[Export]
	private int pushBackForce;
	[Export]
	private Player _player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Hurt(){
		Vector2 velocity = _player.Velocity;
		_player.PushBack(new Vector2(Math.Sign(velocity.x), Math.Sign(velocity.y))* -pushBackForce);
	}
}
