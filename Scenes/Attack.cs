using Godot;
using System;


public partial class PlayerAttack : Area2D
{
	[Export]
	private AnimationPlayer animationPlayer;
	
	public void onPlayerAttack(String attackName){
		animationPlayer.Play(attackName);
	}
}
