using Godot;
using System;

public partial class Menu : VBoxContainer
{
	//Nodes
	Button startButton;
	Button exitButton;
	
	// Called when the node enters the scene tree for the first time.
	//Initialize all nodes in the _Ready function
	// as the OnReady annotation doesn't exist in C#
	public override void _Ready()
	{
		startButton = GetNode("StartButton") as Button;
		exitButton = GetNode("ExitButton") as Button;
		
		//Event connections are made/destroyed by addinr(+=) or removing(-=)
		// methods to the event
		startButton.Pressed += OnStartButtonPressed;
		exitButton.Pressed += OnExitButtonPressed;
	}
	
	private void OnStartButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/World/main.tscn");
	}

	private void OnExitButtonPressed()
	{
		GetTree().Quit();
	}
}
