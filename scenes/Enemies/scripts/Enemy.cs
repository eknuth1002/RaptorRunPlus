using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export] float movementSpeed = 150.0f;
	[Export] bool movingRight = false;
	
	[Signal] public delegate void EnemyDiedEventHandler();
	
	public bool active { get; private set;}
	int gravity = 1600;
	
	//Nodes
	AnimatedSprite2D sprite;
	Area2D hitbox;
	Player playerChar;
	AudioStreamPlayer deathSound;
	
	// Called when the node enters the scene tree for the first time.
	//Initialize all nodes in the _Ready function
	// as the OnReady annotation doesn't exist in C#
	public override void _Ready()
	{
		sprite = GetNode("AnimatedSprite2D") as AnimatedSprite2D;
		playerChar = GetNode("/root/World/Player") as Player;
		hitbox = GetNode("HitBox") as Area2D;
		deathSound = GetNode("DeathSound") as AudioStreamPlayer;
		
		//Event connections are made/destroyed by addinr(+=) or removing(-=)
		// methods to the event
		hitbox.BodyEntered += OnBodyEntered;
		playerChar.PlayerDied += OnPlayerDied;
	}
	
	private void OnPlayerDied()
	{
		SetActive(false);
		sprite.Play("idle");
	}
	
	private void OnBodyEntered(Node2D body)
	{
		if (active)
		{
			playerChar.Die();
		}
	}
	
	public void Die()
	{
		SetActive(false);
		deathSound.Play();
		sprite.Play("death");
		sprite.AnimationFinished += OnAnimationFinished;
		EmitSignal("EnemyDied");
	}
	
	public void OnAnimationFinished()
	{
		this.QueueFree();
	}
	
	public void SetActive(bool active)
	{
		this.active = active;
		if (active)
		{
			sprite.Play("walk");
		}
	}
	
	public void Reverse()
	{
		if (active)
		{
			movingRight = !movingRight;
			sprite.FlipH = movingRight;
		}
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (!active) { return; }
		
		Vector2 v = Velocity;

		v.x = movingRight ? movementSpeed : -movementSpeed;
		v.y += gravity * (float) delta;

		Velocity = v;
		MoveAndSlide();
	}
}
