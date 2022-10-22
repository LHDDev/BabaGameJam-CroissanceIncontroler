using Godot;
using System;

public partial class PumpkinCore : Node
{
	[Export]
	private bool _canDropGrain = false;
	[Export]
	private bool _canGrowBrambles = true;
	[Export]
	private Timer _cooldownDropSeedTimer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_cooldownDropSeedTimer.Timeout += OnCoolDownDropSeedTimerTimeOut;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(_canDropGrain){
			_canDropGrain = false;
			_cooldownDropSeedTimer.Start();
		}
	}

	public void OnCoolDownDropSeedTimerTimeOut(){
		GD.Print("DROP");
		_canDropGrain = true;
		
	}
}
