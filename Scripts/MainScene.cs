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
		toVisiteNode = _level.FindChildrenOfType<Bramble>();
	}

	// public List<Vector2i> GetPositionOfGrowth(Bramble tile){
	// 	if(visitedNode.Contains(tile) || toVisiteNode.Contains(tile))
	// 	{
	// 		return new List<Vector2i>();
	// 	}
	// 	visitedNode.Add(tile);
	// 	toVisiteNode.Remove(tile);
	// 	Vector2i mapCoord = _level.LocalToMap(tile.GlobalPosition);

		
	// }

	
}
