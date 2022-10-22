using Godot;
using System;


public partial class Attack : Area2D
{
	[Export]
	private AnimationPlayer animationPlayer;
	
	public void onPlayerAttack(String attackName){
		GD.Print("Attack attempt: " + attackName);
		animationPlayer.Play(attackName);
	}
}
