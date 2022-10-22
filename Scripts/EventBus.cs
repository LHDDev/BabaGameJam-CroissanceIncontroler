using Godot;
using System;

public partial class EventBus : Node
{
	[Signal]
	public delegate void CollectEventHandler(int collectableType);
	
}
