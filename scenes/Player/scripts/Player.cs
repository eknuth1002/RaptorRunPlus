using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] int gravity = 1600;
	[Export] int jumpHeight = 600;
	
	[Signal] public delegate void PlayerDiedEventHandler();
	[Signal] public delegate void AmmoChangedEventHandler(int a);	
	
	//Public variables
	public bool Alive { get; private set;} = true;
	
	//Private variables
	int jumpsRemaining = 2;
	bool wasJumping = false;
	float jumpPitch = 1.0f;
	int ammo = 3;
	
	//Preloads
	PackedScene projectileScene = (PackedScene) ResourceLoader.Load("res://scenes/Projectile/projectile.tscn");
	
	//Nodes
	AnimatedSprite2D sprite;	
	AudioStreamPlayer jumpSound;
	AudioStreamPlayer deathSound;
	AudioStreamPlayer shootSound;
	Camera2D camera;
	CollisionShape2D collisionShape;
	Node2D projectilePosition;
	Node2D world;
	
	// Called when the node enters the scene tree for the first time.
	//Initialize all nodes in the _Ready function
	// as the OnReady annotation doesn't exist in C#
	public override void _Ready()
	{
		sprite = GetNode("AnimatedSprite2D") as AnimatedSprite2D;
		jumpSound = GetNode("JumpSound") as AudioStreamPlayer;
		deathSound = GetNode("DeathSound") as AudioStreamPlayer;
		camera = GetNode("/root/World/Camera2D") as Camera2D;
		collisionShape = GetNode("CollisionShape2D") as CollisionShape2D;
		shootSound = GetNode("ShootSound") as AudioStreamPlayer;
		projectilePosition = GetNode("ProjectilePosition") as Node2D;
		world = GetNode("/root/World") as Node2D;
		
		//Event connections are made/destroyed by addinr(+=) or removing(-=)
		// methods to the event
		sprite.AnimationFinished += OnAnimationFinished;
	}
	
	public override void _PhysicsProcess(double delta)
	{	
		if (!Alive) { return; }
		Vector2 v = Velocity;
		
		v.y += (gravity * (float) delta);
		Velocity = v;
		MoveAndSlide();
	
		camera.Position = Position;
		if (IsOnFloor() && wasJumping)
		{
			sprite.Play("run");
			wasJumping = false;
			jumpsRemaining = 2;
			jumpPitch = 1.0f;
		}
		
		if (Input.IsActionJustPressed("jump") && jumpsRemaining > 0)
		{
			jumpsRemaining -= 1;
			wasJumping = true;
			v.y = -jumpHeight;
			Velocity = v;
			
			if (jumpsRemaining == 1)
			{
				sprite.Play("jump");
			}
			else
			{
				sprite.Play("double jump");
			}
			
			jumpSound.PitchScale = jumpPitch;
			jumpSound.Play();
			jumpPitch += 0.2f;
		}
	}
	
	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent.IsActionPressed("fire") && Alive && ammo > 0)
		{
			Projectile projectileInst = (Projectile) projectileScene.Instantiate();
			projectileInst.Position = projectilePosition.GlobalPosition;
			world.AddChild(projectileInst);
			shootSound.Play();
			sprite.Play("shoot");
			ammo--;
			EmitSignal("AmmoChanged", ammo);
		}
	}
	
	private void OnAnimationFinished()
	{
		if (sprite.Animation == "shoot")
		{
			sprite.Play("run");
		}
	}
	
	public void Die()
	{
		deathSound.Play();
		sprite.Play("death");
		Alive = false;
		collisionShape.SetDeferred("disabled", true);
		EmitSignal("PlayerDied");
	}
	
	public void AddAmmo(int ammoAmount)
	{
		ammo += ammoAmount;
		EmitSignal("AmmoChanged", ammo);
	}
}
