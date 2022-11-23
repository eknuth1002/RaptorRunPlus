using Godot;
using System;

//Marks an edge for the enemy to turn around after hitting,
// causing the enemy to patrol rather than immediately run off the edge
public partial class Edge : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Event connections are made/destroyed by addinr(+=) or removing(-=)
		// methods to the event
		this.BodyEntered += OnBodyEntered;
	}
	
		private void OnBodyEntered(Node2D body)
		{
			if (body.IsInGroup("enemy"))
			{
				((Enemy) body).Reverse();
			}
		}
}
