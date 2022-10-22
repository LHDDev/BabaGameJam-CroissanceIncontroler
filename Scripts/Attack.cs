using Godot;
using System;


public partial class Attack : Area2D
{
	[Export]
	private AnimationPlayer animationPlayer;
	
	public override void _Ready(){
		
		this.BodyEntered += OnBodyEntered;
	}
	
	public void onPlayerAttack(String attackName){
		GD.Print("Attack attempt: " + attackName);
		animationPlayer.Play(attackName);
	}

	public void OnBodyEntered(Node2D body){
		if(body is normal_pumpkin pumpkin && body.IsInGroup("pumpkin_group")){
			pumpkin.Destroyed();
		}
	}
}
