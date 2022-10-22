using Godot;
using System;

public partial class Player : CharacterBody2D
{

	[Export]
	private int _speed;
	[Export]
	private int _towardDelta;
	[Export]
	private Node2D _weapon;
	[Export]
	private Timer _cooldownTimer;
	[Export]
	private float _cooldownValue;

	private bool _canAttack = true;

	[Signal]
	public delegate void AttackEventHandler();
	public override void _Ready(){
		_cooldownTimer.WaitTime = _cooldownValue;
		_cooldownTimer.Timeout += onCooldownFinished;
	}

	public override void _PhysicsProcess(double delta)
	{
		if(Input.IsMouseButtonPressed(MouseButton.Left) && _canAttack){
			
				_canAttack = false;
				EmitSignal(nameof(Attack));
				_cooldownTimer.Start();
		}
		float hDirection =  Input.GetAxis("ui_left","ui_right");
		float vDirection =  Input.GetAxis("ui_up","ui_down");
		Vector2 dir = new Vector2(hDirection,vDirection );

		if(dir !=  Vector2.Zero)
			Velocity = new Vector2(hDirection,vDirection ) * _speed;
		else 		
			Velocity = Velocity.MoveToward(Vector2.Zero,_towardDelta);
		MoveAndSlide();

	}

	public void onCooldownFinished(){
		_canAttack = true;
	}
}
