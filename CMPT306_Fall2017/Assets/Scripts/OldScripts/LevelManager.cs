using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	public static LevelManager Instance{ get; private set;}

	public Player Player { get; private set; }
	public CameraController Camera { get; private set; }
	public TimeSpan RunningTime { get { return DateTime.UtcNow - _started; } }

	public int CurrentTimeBonus
	{
		get
		{
			var secondDifference = (int)(BonusCutOffSeconds-RunningTime.TotalSeconds);
			return Mathf.Max (0, secondDifference) * BonusSecondMultiplier;
		}
	}

	private List<Checkpoint> _checkpoints;
	private int _currentCheckPointIndex;
	private DateTime _started;
	private int _savedPoints;


	public Checkpoint DebugSpawn;
	public int BonusCutOffSeconds;
	public int BonusSecondMultiplier;

	public void Awake()
	{
		Instance = this;
	}

	public void Start()
	{
		_checkpoints = FindObjectsOfType<Checkpoint>().OrderBy(t => t.transform.position.x).ToList();	
		_currentCheckPointIndex = _checkpoints.Count > 0 ? 0 : -1;

		Player = FindObjectOfType<Player> ();
		Camera = FindObjectOfType<CameraController> ();

		_started = DateTime.UtcNow;

		var listeners = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerRespawnListener>();
		foreach (var listener in listeners) 
		{
			for (var i = _checkpoints.Count - 1; i >= 0; i--) 
			{
				var distance = ((MonoBehaviour)listener).transform.position.x - _checkpoints [i].transform.position.x;
				if (distance < 0)
					continue;

				_checkpoints [i].AssignObjectToCheckpoint (listener);
				break;
			}
		}

#if UNITY_EDITOR
		if (DebugSpawn != null)
			DebugSpawn.SpawnPlayer (Player);
		else if (_currentCheckPointIndex != -1)
			_checkpoints [_currentCheckPointIndex].SpawnPlayer (Player);

#else
		if (_currentCheckPointIndex != 1)
			_checkpoints[_currentCheckPointIndex].SpawnPlayer(Player);
#endif


	}

	public void Update()
	{
		var isAtLastCheckpoint = _currentCheckPointIndex + 1 >= _checkpoints.Count;
		if (isAtLastCheckpoint)
			return;

		var distanceToNextCheckpoint = _checkpoints [_currentCheckPointIndex + 1].transform.position.x - Player.transform.position.x;
		if (distanceToNextCheckpoint >= 0)
			return;

		_checkpoints [_currentCheckPointIndex].PlayerLeftCheckpoint ();
		_currentCheckPointIndex++;
		_checkpoints [_currentCheckPointIndex].PlayerHitCheckpoint ();

		GameManager.Instance.AddPoints (CurrentTimeBonus);
		_savedPoints = GameManager.Instance.Points;
		_started = DateTime.UtcNow;

	}

	public void KillPlayer()
	{
		StartCoroutine(KillPlayerCo());
	}

	private IEnumerator KillPlayerCo()
	{
		Player.Kill();
		Camera.IsFollowing = false;
		yield return new WaitForSeconds(2f);

		Camera.IsFollowing = true;

		if (_currentCheckPointIndex != -1)
			_checkpoints [_currentCheckPointIndex].SpawnPlayer (Player);

		_started = DateTime.UtcNow;
		GameManager.Instance.ResetPoints (_savedPoints);
	}
}
