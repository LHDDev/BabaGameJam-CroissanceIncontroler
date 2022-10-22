using Godot;
using System.Linq;
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

	private List<Bramble> visitedNode = new List<Bramble>();
	private List<Bramble> toVisiteNode = new List<Bramble>();
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
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void PlaceTiles(int x, int y){
		Bramble newTile = SmartLoader<Bramble>(_brambleScenePath);
		newTile.GlobalPosition = _level.MapToLocal(new Vector2i(x,y)) + _spriteOffSet;
		_level.AddChild(newTile);
	}

	public void Growth(){
		// Get All Brambles
		toVisiteNode = _level.FindChildrenOfType<Bramble>().Where(b => b.GlobalPosition >= _leftCorner && b.GlobalPosition >= _rightCorner).ToList<Bramble>();
		foreach(Bramble bramble in toVisiteNode){
			foreach(Vector2 pos in GetPositionOfGrowth(bramble))
			{
				Vector2i mapCoord = _level.LocalToMap(pos);
				PlaceTiles(mapCoord.x,mapCoord.y);
			}
		}
	}

	public List<Vector2> GetPositionOfGrowth(Bramble tile){
		List<Vector2> result = new List<Vector2>();
		if(visitedNode.Contains(tile))
		{
			return result;
		}
		visitedNode.Add(tile);
		
		if(tile.GlobalPosition.x > _leftCorner.x) {

			Bramble east = FindBrambleFromGlobalCoord(tile.GlobalPosition + Vector2.Left * 16);
			if(east != null) result.AddRange(GetPositionOfGrowth(east));
			else result.Add(tile.GlobalPosition + Vector2.Left * 16);
		}

		if(tile.GlobalPosition.x < _rightCorner.x){

			Bramble west = FindBrambleFromGlobalCoord(tile.GlobalPosition + Vector2.Right * 16);
			if(west != null) result.AddRange(GetPositionOfGrowth(west));
			else result.Add(tile.GlobalPosition + Vector2.Right * 16);	
		}

		if(tile.GlobalPosition.y > _leftCorner.y){

			Bramble up = FindBrambleFromGlobalCoord(tile.GlobalPosition + Vector2.Up * 16);	
			if(up != null) result.AddRange(GetPositionOfGrowth(up));
			else result.Add(tile.GlobalPosition + Vector2.Up * 16);
		}

		if(tile.GlobalPosition.y < _rightCorner.y){
			Bramble down = FindBrambleFromGlobalCoord(tile.GlobalPosition + Vector2.Down * 16);
			if(down  != null) result.AddRange(GetPositionOfGrowth(down));
			else result.Add(tile.GlobalPosition + Vector2.Down * 16);
		}


		return result;

		
	}

	public Bramble FindBrambleFromGlobalCoord(Vector2 coord){
		return _level.FindChildrenOfType<Bramble>().FirstOrDefault(b => b.GlobalPosition == coord);
	}

	
}
