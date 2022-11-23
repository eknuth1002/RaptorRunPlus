using Godot;
using System;

/**
*  The tutorial has you create a second nearly identical class for both
*  ammo and score collectables.
*
*  This class extends the logic by having the item type exported rather than
*  all the collection logic duplicated.  The major change is to OnBodyEntered
*  where we either add to pitch and score or to ammo, based on type.
*/
public partial class Collectable : Area2D
{
	[Export] int collectValue = 10;	
	[Export] float collectablesPitchResetInterval = 2000;
	[Export(PropertyHint.Enum,"Normal,Ammo")] String CollectableType = "Normal";
	
	//Private variables
	float collectablesPitch = 1.0f;
	float resetCollectablesPitchTime = 0;
	
	//Nodes
	Game world;
	Player playerChar;
	AnimatedSprite2D sprite;
	AudioStreamPlayer collect;
	
	// Called when the node enters the scene tree for the first time.
	// Initialize all nodes in the _Ready function
	// as the OnReady annotation doesn't exist in C#
	public override void _Ready()
	{
		world = GetNode("/root/World") as Game;
		sprite = GetNode("AnimatedSprite2D") as AnimatedSprite2D;
		playerChar = GetNode("/root/World/Player") as Player;
		collect = GetNode("Collect") as AudioStreamPlayer;
		
		//Event connections are made/destroyed by adding(+=) or removing(-=)
		// methods to the event
		this.BodyEntered += OnBodyEntered;
	}
	
	public override void _Process(double delta)
	{
		if (Time.GetTicksMsec() > resetCollectablesPitchTime)
		{
			collectablesPitch = 1.0f;
		}
	}
	
	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("player"))
		{
			collect.PitchScale = collectablesPitch;
			collect.Play();
			sprite.Play("collected");
			sprite.AnimationFinished += OnAnimationFinished;
			
			if (CollectableType == "Normal")
			{
				world.AddScore(collectValue);
				collectablesPitch += 0.1f;
				resetCollectablesPitchTime = Time.GetTicksMsec() + collectablesPitchResetInterval;
			}
			else
			{
				playerChar.AddAmmo(collectValue);
			}
		}
	}
	
	private void OnAnimationFinished()
	{
		QueueFree();
	}
}
