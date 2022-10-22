using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private enum Weapon {
		Scythe,
		Spray,
		Shovel,
		Shear
	};
	
	[Export]
	private Attack attackArea;
	[Export]
	private int _speed;
	[Export]
	private int _towardDelta;
	[Export]
	private Timer _cooldownTimer;
	[Export]
	private float _cooldownValue;
	[Export]
	private AnimatedSprite2D _playerSprite;
	
	
	private bool _canAttack = true;
	private bool _canMove = true;
	
	[Export]
	private int _weapon = 0;
	
	[Signal]
	public delegate void AttackEventHandler(String attack_name);
	public override void _Ready(){
		this.Attack += attackArea.onPlayerAttack;
		
		EventBus bus = GetTree().Root.GetNode<EventBus>("EventBus");
		bus.Collect += onCollectableCollected;
		_cooldownTimer.WaitTime = _cooldownValue;
		_cooldownTimer.Timeout += onCooldownFinished;
	}

	public override void _PhysicsProcess(double delta)
	{
		if(Input.IsMouseButtonPressed(MouseButton.Left) && _canAttack){
			_canAttack = false;
			EmitSignal(nameof(Attack), Enum.GetName(typeof(Weapon), _weapon));
			_cooldownTimer.Start();
		}

		float hDirection =  Input.GetAxis("ui_left","ui_right");
		float vDirection =  Input.GetAxis("ui_up","ui_down");
		Vector2 dir = new Vector2(hDirection,vDirection );
		MovePlayer(dir);
		SetAnimation();

	}

	public void SetAnimation(){
		if(Velocity == Vector2.Zero){
			// idles
			if(_playerSprite.Animation == "MoveRight") _playerSprite.Animation="IdleRight";
			if(_playerSprite.Animation == "MoveUp") _playerSprite.Animation="IdleUp";
			if(_playerSprite.Animation == "MoveDown") _playerSprite.Animation="IdleDown";
		}
		else{
			GD.Print("Anima toi !!");
			_playerSprite.FlipH = Velocity.x < 0;
			if(Velocity.x != 0){
				_playerSprite.Animation = "MoveRight";
			}
			else{
				if(Velocity.y < 0) _playerSprite.Animation = "MoveUp";
				if(Velocity.y > 0) _playerSprite.Animation = "MoveDown";
			}

		}
	}
	public void MovePlayer(Vector2 dir){

		if(dir !=  Vector2.Zero && _canMove)
			Velocity = dir * _speed;
		else 		
			Velocity = Velocity.MoveToward(Vector2.Zero,_towardDelta);
		MoveAndSlide();

	}
	public void onCooldownFinished(){
		_canAttack = true;
	}

	public async void PushBack(Vector2 force){
		_canMove= false;
		_canAttack=false;
		GD.Print(force);
		Velocity = force;
		GD.Print(Velocity + "PUSH");
		MoveAndSlide();
		await ToSignal(GetTree().CreateTimer(0.25f),"timeout");
		GD.Print(Velocity + "STOP PUSH");
		_canMove = true;
		_canAttack = true;
	}
	
	public void onCollectableCollected(int collectableType){
		GD.Print("Collectable collected " + collectableType);
		_weapon = collectableType;
	}
}
