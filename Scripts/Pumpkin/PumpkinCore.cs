using Godot;
using System;

public partial class PumpkinCore : Node
{
	[Export]
	private bool _canDropGrain = false;
	[Export]
	private Timer _dropSeedTimer;
	[Export]
	private Timer _cooldownBramblersTimer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnDropSeedTimerTimeOut(){
		
	}

	public void OnCoolDownBramblersTimerTimeout(){
		// Grandir les ronces
	}
}
