using Godot;
using System.Linq;
using System;
using System.Collections.Generic;
using static Scripts.Utils.NodeExtension;

public partial class MainScene : Node2D
{
	[Export]
	private TileMap _level;
	[Export]
	private string _brambleScenePath;
	[Export]
	private string _normalPumpkinScenePath;
	[Export]
	private Vector2i _spriteOffSet;
	[Export]
	private Vector2 _rightCorner;
	[Export]
	private Vector2 _leftCorner;
	[Export]
	private Timer _growthCooldown;

	private int _timeScore = 0;
	private int _pumpkinScore = 0;
	public int PumpkinScore {
		get => _pumpkinScore;
		set{
			_pumpkinScore = value;
			bus.EmitSignal(nameof(bus.ScoreChanged), _timeScore+_pumpkinScore);
		}
	}
	private double _timeElapsed = 0.0f;
	private int _timeToPLaceNewPumpkin;
	private double _lastTimePumpkinPLaced;

	Random rand = new Random(); // 2 => 27 x -- 2 => 15 y
	EventBus bus;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		for(int x = 0; x <= 29; x++){
			for(int y = 0; y <= 1; y++){
				PlaceTiles(x,y,false);
			}
		}
		for(int x = 0; x <= 29; x++){
			for(int y = 15; y <= 17 ; y++){
				PlaceTiles(x,y,false);
			}
		}
		for(int x = 0; x <= 1; x++){
			for(int y = 2; y <= 14 ; y++){
				PlaceTiles(x,y,false);
			}
		}
		for(int x = 28; x <= 29; x++){
			for(int y = 2; y <= 14 ; y++){ 
				PlaceTiles(x,y,false);
			}
		}

		PlacePumpkin(rand.Next((int)_rightCorner.x,(int)_leftCorner.x),rand.Next((int)_rightCorner.y,(int)_leftCorner.y));
		PlacePumpkin(rand.Next((int)_rightCorner.x,(int)_leftCorner.x),rand.Next((int)_rightCorner.y,(int)_leftCorner.y));
		
		_growthCooldown.Timeout += Growth;
		_growthCooldown.Start();
		_timeToPLaceNewPumpkin = rand.Next(10);
		GD.Print($"New placement in {_timeToPLaceNewPumpkin}");
		bus = GetTree().Root.GetNode<EventBus>("EventBus");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_timeElapsed += delta;
		_timeScore = ((int)_timeElapsed)*10;
		bus.EmitSignal(nameof(bus.ScoreChanged), _timeScore+_pumpkinScore);
		if(_timeElapsed - _lastTimePumpkinPLaced >= _timeToPLaceNewPumpkin )
		{
			GD.Print("PLACE PUMPKIN");
			PlacePumpkin(rand.Next((int)_rightCorner.x,(int)_leftCorner.x),rand.Next((int)_rightCorner.y,(int)_leftCorner.y));
			_lastTimePumpkinPLaced = _timeElapsed;
			_timeToPLaceNewPumpkin = rand.Next(10);
		}
	}

	public Bramble PlaceTiles(int x, int y, bool canGrowth)
	{
		Bramble newTile = SmartLoader<Bramble>(_brambleScenePath);
		newTile.GlobalPosition = _level.MapToLocal(new Vector2i(x,y)) + _spriteOffSet;
		AddChild(newTile);
		MoveChild(newTile,2);
		newTile.CanGrowth = canGrowth;
		return newTile;
	}
	public Bramble PlaceTiles(Vector2 globalPosition, bool canGrowth){
		Bramble newTile = SmartLoader<Bramble>(_brambleScenePath);
		newTile.GlobalPosition =  globalPosition;
		if(rand.Next(5) == 4) newTile.IsThorned = true;
		AddChild(newTile);
		MoveChild(newTile,2);
		newTile.CanGrowth = canGrowth;
		return newTile;
	}

	public normal_pumpkin PlacePumpkin(int x, int y){
		Vector2 pos = _level.MapToLocal(new Vector2i(x,y)) + _spriteOffSet;
		List<Bramble> brambleOnThatPosition = this.FindChildrenOfType<Bramble>();
		List<normal_pumpkin> pumpkinOnThatPosition = this.FindChildrenOfType<normal_pumpkin>();
		List<Player> playerOnThatPosition = this.FindChildrenOfType<Player>();

		if(!brambleOnThatPosition.Any(b => b.GlobalPosition == pos) && !pumpkinOnThatPosition.Any(b => b.GlobalPosition == pos) && !playerOnThatPosition.Any(b => b.GlobalPosition == pos)){
			normal_pumpkin newTile = SmartLoader<normal_pumpkin>(_normalPumpkinScenePath);
			newTile.GlobalPosition = pos;
			AddChild(newTile);
			return newTile;
		}

		GD.Print("Already taken");
		return null;

	}

	public void Growth(){
		List<Bramble> toGrowth = this.FindChildrenOfType<Bramble>(b => b.CanGrowth == true);
		foreach( Bramble b in toGrowth){
			b.Growth();
		}

		_growthCooldown.Start();
	}

	public void DisposeFrom(Vector2 start, Vector2 direction ){
		Vector2 brambleCoord = start + (direction * 16);
		Bramble bramble = FindBrambleFromGlobalCoord(brambleCoord);
		if( bramble != null){
			bramble.QueueFree();
		}
	}

	public Bramble FindBrambleFromGlobalCoord(Vector2 coord){
		return this.FindChildrenOfType<Bramble>(b => b.GlobalPosition == coord).FirstOrDefault();
	}
	public Bramble FindBrambleFromGlobalCoord(float x, float y){
		return this.FindChildrenOfType<Bramble>().FirstOrDefault(b => b.GlobalPosition == new Vector2(x,y));
	}

}
