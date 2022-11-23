using Godot;
using System;

public partial class Projectile : AnimatableBody2D
{
	//ulong is an unsigned (always positive) long used with Time
	ulong deathTime = 0;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		deathTime = Time.GetTicksMsec() + 2000;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Time.GetTicksMsec() > deathTime)
		{
			this.QueueFree();
		}
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 distance = Vector2.Right * 1000 * (float) delta;
		KinematicCollision2D collision = MoveAndCollide(distance);
		
		if (collision != null)
		{
			if (((Node2D)collision.GetCollider()).IsInGroup("enemy")) 
			{
				((Enemy)collision.GetCollider()).Die();
			}
			this.QueueFree();
		}
	}
}
