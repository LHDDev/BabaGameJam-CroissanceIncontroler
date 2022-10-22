using Godot;
using System;

public partial class Collectable : Node2D
{
	private enum CollectableType {
		Shear,
		Spray,
		Shovel,
		Scythe
	};
	
	[Export]
	private Sprite2D sprite;
	
	[Export]
	private Area2D area;
	
	[Export]
	private int collectableType = 0;
	
	[Signal]
	public delegate void CollectedEventHandler(int collectableType);
	
	public void setCollectableType(int newValue){
		collectableType = newValue;
		sprite.Frame = collectableType;
	}
	
	public override void _Ready()
	{
		area.BodyEntered += onBodyEntered;
		setCollectableType(collectableType);
	}

	public override void _Process(double delta)
	{
	}
	
	public void onBodyEntered(Node2D body){
		EventBus bus = GetTree().Root.GetNode<EventBus>("EventBus");
		bus.EmitSignal(nameof(EventBus.Collect), collectableType);
		QueueFree();
	}
}
