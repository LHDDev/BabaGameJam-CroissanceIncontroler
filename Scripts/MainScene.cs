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
	private Vector2i _spriteOffSet;
	[Export]
	private Vector2 _rightCorner;
	[Export]
	private Vector2 _leftCorner;
	[Export]
	private Timer _growthCooldown;

	private Random rng = new Random();

	private List<Vector2> _cantGrowthAnymore = new List<Vector2>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		for(int x = 0; x <= 29; x++){
			for(int y = 0; y <= 1; y++){
				PlaceTiles(x,y);
			}
		}
		for(int x = 0; x <= 29; x++){
			for(int y = 15; y <= 17 ; y++){
				PlaceTiles(x,y);
			}
		}
		for(int x = 0; x <= 1; x++){
			for(int y = 2; y <= 14 ; y++){
				PlaceTiles(x,y);
			}
		}
		for(int x = 28; x <= 29; x++){
			for(int y = 2; y <= 14 ; y++){ 
				PlaceTiles(x,y);
			}
		}

		PlaceTiles(5,5);
		_growthCooldown.Timeout += Growth;
		_growthCooldown.Start();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public Bramble PlaceTiles(int x, int y)
	{
		Bramble newTile = SmartLoader<Bramble>(_brambleScenePath);
		newTile.GlobalPosition = _level.MapToLocal(new Vector2i(x,y)) + _spriteOffSet;
		_level.AddChild(newTile);
		return newTile;
	}
	public Bramble PlaceTiles(Vector2 globalPosition){
		Bramble newTile = SmartLoader<Bramble>(_brambleScenePath);
		newTile.GlobalPosition =  globalPosition;
		_level.AddChild(newTile);
		return newTile;
	}

	public void Growth(){
		List<Bramble>visitedNode = new List<Bramble>();

		List<Vector2> coord = new List<Vector2>();
		for(float x = _leftCorner.x; x <= _rightCorner.x; x += 16){
			for(float y = _leftCorner.y; y <= _rightCorner.y; y += 16 ){
				if( !_cantGrowthAnymore.Contains(new Vector2(x,y))){
					if(FindBrambleFromGlobalCoord(x,y) is Bramble bramble && !visitedNode.Contains(bramble))
					{
						if(FindBrambleFromGlobalCoord(bramble.GlobalPosition + Vector2.Left * 16) == null )
							coord.Add(bramble.GlobalPosition + Vector2.Left * 16);
						if(FindBrambleFromGlobalCoord(bramble.GlobalPosition + Vector2.Right * 16) == null )
							coord.Add(bramble.GlobalPosition + Vector2.Right * 16);
						if(FindBrambleFromGlobalCoord(bramble.GlobalPosition + Vector2.Up * 16) == null )
							coord.Add(bramble.GlobalPosition + Vector2.Up * 16);
						if(FindBrambleFromGlobalCoord(bramble.GlobalPosition + Vector2.Down * 16) == null )
							coord.Add(bramble.GlobalPosition + Vector2.Down * 16);
						

						if(coord.Count>=3){
							int idSelected = rng.Next(coord.Count);
							Vector2 coordToGrowth = coord.ElementAt(idSelected);
							visitedNode.Add(bramble);
							visitedNode.Add(PlaceTiles(coordToGrowth));
						}
						else{
							_cantGrowthAnymore.Add(bramble.GlobalPosition);
						}
					}
				}
			}
		}
		_growthCooldown.Start();
	}


	public Bramble FindBrambleFromGlobalCoord(Vector2 coord){
		return _level.FindChildrenOfType<Bramble>().FirstOrDefault(b => b.GlobalPosition == coord);
	}
	public Bramble FindBrambleFromGlobalCoord(float x, float y){
		return _level.FindChildrenOfType<Bramble>().FirstOrDefault(b => b.GlobalPosition == new Vector2(x,y));
	}

	public bool IsBetween(float min, float max, float val){
		return (val >= min && val <= max);
	}
}
