using UnityEngine;
using UnityEngine.TileMap;
using System;
using System.Collections;

[RequireComponent (typeof (Camera))]
public class TileMapFollowCamera : MonoBehaviour {

	[HideInInspector]
	private GameObject player = null;
	[HideInInspector]
	private Grid grid = null;
	[HideInInspector]
	private TileMap tileMap = null;

	public string playerTag = "Player";
	public string mapTag = "Cave";

	void Start () {
		player = GameObject.FindWithTag (playerTag);
		var go = GameObject.FindWithTag (mapTag);
		if (go != null)
		{
			tileMap = go.GetComponent<TileMap> ();
			grid = tileMap.LayoutGrid;
		}
			
	}
	
	void Update () {
		if (player == null || tileMap == null || grid == null)
			return;

		var mainCamera = GetComponent<Camera> ();
		var height = mainCamera.orthographicSize;
		var width = mainCamera.aspect * mainCamera.orthographicSize;

		var tileMapSize = new Vector2(grid.cellSize.x * tileMap.size.x, grid.cellSize.y * tileMap.size.y);

		var playerPosition = player.transform.position;
		var x = Math.Min(tileMapSize.x - width, Math.Max(width, playerPosition.x));
		var y = Math.Min(tileMapSize.y - height, Math.Max(height, playerPosition.y));

		mainCamera.transform.position = new Vector3(x, y, mainCamera.transform.position.z);
	}
}
