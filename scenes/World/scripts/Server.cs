using Godot;
using System;

public partial class Server : Node2D
{
	int port = 8080;
	Godot.Collections.Dictionary<long, Player> players = new Godot.Collections.Dictionary<long, Player>();
	//Godot.Collections.Dictionary<long, NetworkGame> games = new Godot.Collections.Dictionary<long, NetworkGame>();
	//Nodes
	Button exitServerButton;
	ENetMultiplayerPeer peer;
	TextEdit playersConnected;
	
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		peer = new ENetMultiplayerPeer();
		exitServerButton = GetNode("UI/MarginContainer/VBoxContainer/ExitServerButton") as Button;
		exitServerButton.Pressed += OnExitServerButtonPressed;
		playersConnected = GetNode("UI/MarginContainer/VBoxContainer/Lobby/Players") as TextEdit;
		startServer();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	private void startServer()
	{	
		Multiplayer.PeerDisconnected += OnPeerDisconnectedEventHandler;
		Multiplayer.PeerConnected += OnPeerConnectedEventHandler;
		GD.Print(peer.CreateServer(port));
		GD.Print("Running server on port: " + peer.Host.GetLocalPort());
		Multiplayer.MultiplayerPeer = peer;
	}
	
	public void OnPeerConnectedEventHandler(long id)
	{
		//id is unique provided by event
		//Player p = PlayerScene.Instantiate();
		//p.Id = id;
		//run stuff to create game
		Player p = new Player(id);
		players.Add(id, p);
		refreshPlayersConnectedText();
		GD.Print("ID joined: " + p.Id);
	}
	
	private void refreshPlayersConnectedText()
	{
		System.Text.StringBuilder playerText = new System.Text.StringBuilder();
		foreach (System.Collections.Generic.KeyValuePair<long, Player> p in players)
		{
			GD.Print(playerText.ToString());
			playerText.Append("Player ");
			playerText.Append(p.Key);
			playerText.AppendLine();
			GD.Print(playerText.ToString());
		}
		
		playersConnected.Text = playerText.ToString();
	}
	
	public void OnPeerDisconnectedEventHandler(long id)
	{
		//id is unique provided by event
		players.Remove(id);
		refreshPlayersConnectedText();
		//run stuff to end game
	}
	
	private void OnExitServerButtonPressed()
	{
		peer.Host.Destroy();
		GD.Print("Destroyed server");
		GetTree().ChangeSceneToFile("res://scenes/World/menu.tscn");
	}
	
	private void addGame(Player player)
	{
		//add game to list
		//NetworkGame ng = new NetworkGame();
		//games.Add(player.Id, ng);
	}
	
	private void removeGame(long gameId)
	{
		//remove game from list.  Possibly use events when game is exited
		//games.Remove(gameId);
	}
	
	private void joinGame(long gameId, Player player)
	{
		//add game to list
		
		//NetworkGame ng = games[gameId];
		//ng.AddPlayer(player);
		//ng.StartGame();
	}
}
