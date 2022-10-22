using Godot;
using System;

public partial class Collectable : Node2D
{
	private enum CollectableTypes {
		Scythe,
		Shovel,
		Spray
	}
	
	[Export]
	private Sprite2D sprite;
	
	[Export]
	private int collectableType = 0;
	
	public void setCollectableType(int newValue){
		collectableType = newValue;
		sprite.Frame = collectableType;
	}
	
	public override void _Ready()
	{
		setCollectableType(collectableType);
	}

	public override void _Process(double delta)
	{
	}
}
