using Godot;
using System;

public partial class Client : Node2D
{
	ENetMultiplayerPeer peer;
	
	//Nodes
	Button joinButton;
	TextEdit serverAddressTextEdit;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		peer = new ENetMultiplayerPeer();
		serverAddressTextEdit = GetNode("UI/MarginContainer/VBoxContainer/MarginContainer/NetworkButtons/ServerAddress") as TextEdit;
		joinButton = GetNode("UI/MarginContainer/VBoxContainer/MarginContainer/NetworkButtons/JoinButton") as Button;
		
		joinButton.Pressed += OnJoinButtonPressed;
	}

	private void OnJoinButtonPressed()
	{
		string[] ipAddressParts = serverAddressTextEdit.Text.Split(':');
		
		if (ipAddressParts.Length < 2)
		{
			return;
		}
		
		GD.Print(peer.CreateClient(ipAddressParts[0], int.Parse(ipAddressParts[1])));
		Multiplayer.MultiplayerPeer = peer;
	}
}
