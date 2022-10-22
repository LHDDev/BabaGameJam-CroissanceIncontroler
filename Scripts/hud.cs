using Godot;
using System;

public partial class hud : CanvasLayer
{
	[Export]
	private Sprite2D _sprite;
	
	public override void _Ready()
	{
		EventBus bus = GetTree().Root.GetNode<EventBus>("EventBus");
		bus.WeaponEquiped += onWeaponEquiped;
	}

	public void onWeaponEquiped(int weaponType){
		_sprite.Frame = weaponType;
	}
}
