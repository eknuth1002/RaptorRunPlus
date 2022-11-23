using Godot;
using System;

public partial class PlayArea : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Event connections are made/destroyed by addinr(+=) or removing(-=)
		// methods to the event
		this.BodyEntered += OnBodyEntered;
		this.BodyExited += OnBodyExited;
	}
	
	public void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("enemy"))
		{
			((Enemy) body).SetActive(true);
		}
	}
	
	public void OnBodyExited(Node2D body)
	{
		if (body.IsInGroup("enemy"))
		{
			body.QueueFree();
		}
	}
}
