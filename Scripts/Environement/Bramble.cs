using Godot;
using System.Collections.Generic;
using System.Linq;
using System;

public partial class Bramble : Node2D
{
	[Export]
	public bool IsThorned = false;
	[Export]
	private Sprite2D _thorn;
	[Export]
	private Area2D _hitBox;

	public List<Vector2> Neighbors = new List<Vector2>();
	private Random rng = new Random();
	private MainScene mainScene;
	[Export]
	public bool CanGrowth;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_hitBox.AreaEntered += OnAreaEntered;
		mainScene = GetParent<MainScene>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_thorn.Visible = IsThorned;
		_hitBox.Monitoring = IsThorned;
	}

	public void OnAreaEntered(Area2D entertedArea){
		if(entertedArea.GetGroups().Contains("hurtbox_player")){
			((PlayerHurtBox)entertedArea).Hurt();
		}
	}
	
	public void Growth(){

		List<Vector2> possibleGrowth = new List<Vector2>(){Vector2.Up,Vector2.Down,Vector2.Left,Vector2.Right};

		foreach(Vector2 n in Neighbors)
		{
			possibleGrowth.RemoveAll(p => p == n);
		}
		Vector2 growthDirection = possibleGrowth[rng.Next(possibleGrowth.Count)];

		Vector2 result = GlobalPosition + growthDirection*16; 

		Bramble newBrambel = mainScene.PlaceTiles(result,true);

		newBrambel.Neighbors.Add(-growthDirection);
		newBrambel.CheckForNeighbours();
		this.Neighbors.Add(growthDirection);

		if(Neighbors.Count >= 2){
			CanGrowth = false;
		}
	}

	public void CheckForNeighbours(){
		// First check my position If I'm on the edge => I have neighbours and add them
		List<Vector2> possibleNeigbours = new List<Vector2>(){Vector2.Up,Vector2.Down,Vector2.Left,Vector2.Right};
		foreach(Vector2 n in Neighbors)
		{
			possibleNeigbours.RemoveAll(p => p == n);
		}

		if(GlobalPosition.x == 48) 
		{
			// Neighbour on my left
			Neighbors.Add(Vector2.Left);
			possibleNeigbours.Remove(Vector2.Left);
		}
		if(GlobalPosition.x == 448) 
		{
			// Neighbour on my left
			Neighbors.Add(Vector2.Right);
			possibleNeigbours.Remove(Vector2.Right);
		}

		if(GlobalPosition.y == 48) 
		{
			// Neighbour on my left
			Neighbors.Add(Vector2.Up);
			possibleNeigbours.Remove(Vector2.Up);
		}
		if(GlobalPosition.y == 240) 
		{
			// Neighbour on my left
			Neighbors.Add(Vector2.Down);
			possibleNeigbours.Remove(Vector2.Down);
		}

		foreach(Vector2 dir in possibleNeigbours){
			if(mainScene.FindBrambleFromGlobalCoord(GlobalPosition + dir*16) != null ){
				Neighbors.Add(dir);
			}
		}
		if(Neighbors.Count >= 2){
			CanGrowth = false;
		}
	}

}
