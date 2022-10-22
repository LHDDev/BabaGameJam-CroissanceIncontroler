using Godot;
using System;

public partial class hud : CanvasLayer
{
	[Export]
	private Sprite2D _sprite;
	
	[Export]
	private HBoxContainer heartsContainer;
	
	public override void _Ready()
	{
		EventBus bus = GetTree().Root.GetNode<EventBus>("EventBus");
		bus.WeaponEquiped += onWeaponEquiped;
		bus.PlayerHpChanged += onPlayerHpChanged;
	}

	public void onWeaponEquiped(int weaponType){
		_sprite.Frame = weaponType;
	}
	
	public void onPlayerHpChanged(int hp){
		int nb_hearts = heartsContainer.GetChildCount();
		GD.Print("Hp changed " + hp);
		
		for(int i = 0; i < nb_hearts; i++){
			TextureRect textureRect = heartsContainer.GetChild<TextureRect>(i);
			bool fullHeart = i < hp;
			AtlasTexture texture = (AtlasTexture)(textureRect.Texture);
			Vector2 textOffset = (fullHeart)? new Vector2(15.0f, 0.0f) : new Vector2(0.0f, 0.0f);
			GD.Print("TextureRect name " + textureRect.Name + ", i : " + i + " textOffset " + textOffset);
			texture.Region = new Rect2(textOffset, new Vector2(15.0f, 15.0f));
		}
	}
	
}
