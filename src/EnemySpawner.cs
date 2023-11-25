using Godot;

namespace battleships;

public partial class EnemySpawner : Node2D
{
	[Export] public Ship Target;
	private double _time = double.MaxValue;
	private float _spawnDistance = 500;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Every 2 seconds, spawn a new enemy.
		
		if (_time > 60)
		{
			_time = 0;
			SpawnEnemy();
		}
		else
		{
			_time += delta;
		}
	}

	private void SpawnEnemy()
	{
		var enemy = new Enemy(GetSpawnPosition(), Target);
		AddSibling(enemy);
	}

	// Generates a vector that is _spawnDistance distance away from the target in a random direction.
	private Vector2 GetSpawnPosition()
	{
		var rng = new RandomNumberGenerator();
		rng.Randomize();
		return Target.Position + new Vector2(rng.RandfRange(-1, 1), rng.RandfRange(-1, 1)).Normalized() * _spawnDistance;
	}
}