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

		PlacePumpkin(5,5);
		PlacePumpkin(10,10);
		PlacePumpkin(8,7);
		PlacePumpkin(6,12);
		
		_growthCooldown.Timeout += Growth;
		_growthCooldown.Start();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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
		AddChild(newTile);
		MoveChild(newTile,2);
		newTile.CanGrowth = canGrowth;
		return newTile;
	}

	public normal_pumpkin PlacePumpkin(int x, int y){
		normal_pumpkin newTile = SmartLoader<normal_pumpkin>(_normalPumpkinScenePath);
		newTile.GlobalPosition = _level.MapToLocal(new Vector2i(x,y)) + _spriteOffSet;
		AddChild(newTile);
		return newTile;

	}

	public void Growth(){
		List<Bramble> toGrowth = this.FindChildrenOfType<Bramble>(b => b.CanGrowth == true);
		//GD.Print(toGrowth.Count);
		foreach( Bramble b in toGrowth){
			b.Growth();
		}

		_growthCooldown.Start();
	}
	public Bramble FindBrambleFromGlobalCoord(Vector2 coord){
		return this.FindChildrenOfType<Bramble>(b => b.GlobalPosition == coord).FirstOrDefault();
	}
	public Bramble FindBrambleFromGlobalCoord(float x, float y){
		return this.FindChildrenOfType<Bramble>().FirstOrDefault(b => b.GlobalPosition == new Vector2(x,y));
	}
}
