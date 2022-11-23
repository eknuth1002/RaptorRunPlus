using Godot;
using System;

public partial class Game : Node2D
{
	[Export] int worldSpeed = 300;
	
	[Signal] public delegate void GameOverEventHandler();
	
	//Private Variables
	int score = 0;
	Vector2 lastPlatformLoc = Vector2.Zero;
	double nextSpawnTime = 0.0;	
	RandomNumberGenerator rng = new RandomNumberGenerator();
	PackedScene[] platformTypes;
	
	//Preloads
	PackedScene platform = (PackedScene) ResourceLoader.Load("res://scenes/platform/platform.tscn");
	PackedScene platformCollectableSingle = (PackedScene) ResourceLoader.Load("res://scenes/platform/platformCollectableSingle.tscn");
	PackedScene platformCollectableRow = (PackedScene) ResourceLoader.Load("res://scenes/platform/platformCollectableRow.tscn");
	PackedScene platformCollectableArch = (PackedScene) ResourceLoader.Load("res://scenes/platform/platformCollectableArch.tscn");
	PackedScene platformCollectableDoubleArch = (PackedScene) ResourceLoader.Load("res://scenes/platform/platformCollectableDoubleArch.tscn");
	PackedScene platformEnemy = (PackedScene) ResourceLoader.Load("res://scenes/platform/platformEnemy.tscn");
	PackedScene platformCollectableAmmo = (PackedScene) ResourceLoader.Load("res://scenes/platform/platformCollectableAmmo.tscn");
	
	//Nodes
	Player playerChar;
	Area2D ground;
	Node2D movingEnv;
	Label scoreLabel;
	Label ammoLabel;
	Label gameOverLabel;
		
	// Called when the node enters the scene tree for the first time.
	//Initialize all nodes in the _Ready function
	// as the OnReady annotation doesn't exist in C#
	public override void _Ready()
	{
		movingEnv = GetNode("Environment/Moving") as Node2D;
		scoreLabel = GetNode("HUD/UI/Score") as Label;
		ammoLabel = GetNode("HUD/UI/Ammo") as Label;
		gameOverLabel = GetNode("HUD/UI/GameOver") as Label;
		playerChar = GetNode("Player") as Player;
		ground = GetNode("Environment/Static/Ground") as Area2D;
		rng.Randomize();
		
		playerChar.AmmoChanged += updateAmmoLabel;
		
		//Populate the array that we will use to randomly chose a platform type
		//Repeated platforms will have a hgiher probability
		platformTypes = new PackedScene[] {
			platform,
			platformCollectableSingle,
			platformCollectableRow,
			platformCollectableArch,
			platformCollectableDoubleArch,
			platformEnemy,
			platformCollectableAmmo
		};
		
		playerChar.PlayerDied += OnPlayerDied;
		ground.BodyEntered += OnGroundBodyEntered;
		
	}
	
	private void updateAmmoLabel(int ammo)
	{
		ammoLabel.Text = $"Ammo: {ammo}";
	}

	public override void _PhysicsProcess(double delta)
	{
		
		if (!playerChar.Alive) { 
			if (Input.IsActionJustPressed("jump"))
			{
				GetTree().ReloadCurrentScene();
			}
			if (Input.IsActionJustPressed("quit"))
			{
				GetTree().Quit();
			}
			return; 
		}
		
		Vector2 mePos = movingEnv.Position;
		
		//Move platforms left
		mePos.x -= worldSpeed * (float) delta;
		movingEnv.Position = mePos;
	}
	
	public override void _Process(double delta)
	{
		if (!playerChar.Alive) { return; }
		
		scoreLabel.Text = $"Score: {score}";
		//Spawn a new platform
		if (Time.GetTicksMsec() > nextSpawnTime)
		{
			SpawnNextPlatform();
		}
	}
	private void SpawnNextPlatform()
	{		
		StaticBody2D p = (StaticBody2D) platformTypes[rng.RandiRange(0, platformTypes.Length - 1)].Instantiate();
		Vector2 pPos = p.Position;
		
		if (lastPlatformLoc == Vector2.Zero)
		{
			pPos.x += 400;
		}
		else
		{
			float x = lastPlatformLoc.x + rng.RandiRange(450, 550);
			float y = Mathf.Clamp(lastPlatformLoc.y + rng.RandiRange(-150, 150), 200, 1000);
			pPos.x = x;
			pPos.y = y;				
		}
		
		p.Position = pPos;
		movingEnv.AddChild(p);
		lastPlatformLoc = pPos;
		nextSpawnTime += worldSpeed;
	}
	
	private void OnPlayerDied()
	{
		//Uses placeholders in the text (0, 1, 2, etc) and then replaces them with format
		gameOverLabel.Text = string.Format(gameOverLabel.Text, score);
		gameOverLabel.Visible = true;
		EmitSignal("GameOver");
	}
	
	private void OnGroundBodyEntered(Node2D body)
	{
		playerChar.Die();
	}
	
	public void AddScore(int value)
	{
		score += value;
	}
}
