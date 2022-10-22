using Godot;
using System;

public partial class Shear : Node2D
{
	[Export]
	private Player _player;
	[Export]
	private Vector2 _finalPos;
	[Export]
	private float _attackSpeed;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_player.Attack += OnPlayerAttack;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnPlayerAttack(){
		Tween tween = CreateTween();
		tween.TweenProperty(this,"position",_finalPos,_attackSpeed);
		tween.TweenProperty(this,"position",Vector2.Zero,_attackSpeed);
		tween.Play();
	}
}
