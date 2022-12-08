using Godot;
using System;

public partial class Menu : VBoxContainer
{
	//Nodes
	Button startButton;
	Button exitButton;
	Button serverButton;
	Button clientButton;
	
	// Called when the node enters the scene tree for the first time.
	//Initialize all nodes in the _Ready function
	// as the OnReady annotation doesn't exist in C#
	public override void _Ready()
	{
		startButton = GetNode("StartButton") as Button;
		exitButton = GetNode("ExitButton") as Button;
		serverButton = GetNode("NetworkButtons/ServerButton") as Button;
		clientButton = GetNode("NetworkButtons/ClientButton") as Button;
		
		//Event connections are made/destroyed by addinr(+=) or removing(-=)
		// methods to the event
		startButton.Pressed += OnStartButtonPressed;
		exitButton.Pressed += OnExitButtonPressed;
		serverButton.Pressed += OnServerButtonPressed;
		clientButton.Pressed += OnClientButtonPressed;
	}
	
	private void OnStartButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/World/main.tscn");
	}

	private void OnExitButtonPressed()
	{
		GetTree().Quit();
	}
	
	private void OnServerButtonPressed()
	{
		GD.Print("Server Button Pressed");
		GetTree().ChangeSceneToFile("res://scenes/World/server.tscn");
	}
	
	private void OnClientButtonPressed()
	{
		GD.Print("Client Button Pressed");
		GetTree().ChangeSceneToFile("res://scenes/World/client.tscn");
	}
}
