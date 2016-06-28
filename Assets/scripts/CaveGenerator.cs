using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaveGenerator : MonoBehaviour {

	public GameObject wall;
	List<GameObject> availableWalls;

	int[,] cave;

	public int width;
	public int height;

	public int minSpaceRegionSize;
	public int minWallRegionSize;

	[Range(0, 100)]
	public int fillPercent;
	public int postCleanFillPercent;

	public int numIterations;

	public string seed;
	public bool useSeed;

	struct Tile {
		public int x;
		public int y;

		public Tile (int tileX, int tileY) {
			x = tileX;
			y = tileY;
		}
	}

	// Use this for initialization
	void Start () {
		availableWalls = new List<GameObject> ();
		GenerateCave ();
		InstantiateWalls ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			GenerateCave ();
			RecallWalls ();
			InstantiateWalls ();
		}
	
	}

	void GenerateCave() {
		cave = new int[width, height];
		FillCave ();
		for (int i = 0; i < numIterations; i++) {
			SmoothCave ();
		}

		CleanCave ();

		List<List<Tile>> spaceRegions = GetRegions(0);
		if (spaceRegions.Count != 1 || (float)spaceRegions [0].Count / (width * height) * 100 < postCleanFillPercent) {
			GenerateCave ();
		}
	}

	void FillCave() {
		System.Random rand;
		if (useSeed) {
			rand = new System.Random (seed.GetHashCode ());
		} else {
			rand = new System.Random (Random.Range(0, 100000));
		}

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (rand.Next (0, 100) <= fillPercent || x == 0 || x == width - 1 || y == 0 || y == height - 1) {
					cave [x, y] = 1;
				} else {
					cave [x, y] = 0;
				}
			}
		}
	}

	void SmoothCave() {
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				int numNeighbors = GetNeighbors (x, y);

				if (numNeighbors > 4) {
					cave [x, y] = 1;
				} else if (numNeighbors < 4) {
					cave [x, y] = 0;
				}
			}
		}
	}

	int GetNeighbors(int x, int y) {
		int numNeighbors = 0;
		for (int neighborX = x - 1; neighborX <= x + 1; neighborX++) {
			for (int neighborY = y - 1; neighborY <= y + 1; neighborY++) {
				if (!IsInBounds (neighborX, neighborY)) {
					numNeighbors++;
				} else if (neighborX != x || neighborY != y) {
					numNeighbors += cave [neighborX, neighborY];
				}
			}
		}
		return numNeighbors;
	}

	bool IsInBounds(int x, int y) {
		return x >= 0 && x < width && y >= 0 && y < height;
	}

	List<Tile> GetRegion(int x, int y) {
		int tileType = cave [x, y];

		List<Tile> regionTiles = new List<Tile> ();
		Queue<Tile> tilesToProcess = new Queue<Tile> ();
		int[,] seen = new int[width, height];

		regionTiles.Add (new Tile (x, y));
		tilesToProcess.Enqueue (new Tile (x, y));

		while (tilesToProcess.Count > 0) {
			Tile curr = tilesToProcess.Dequeue ();
			for (int i = curr.x - 1; i <= curr.x + 1; i++) {
				for (int j = curr.y - 1; j <= curr.y + 1; j++) {
					if (IsInBounds (i, j) && (i != x || j!= y) && seen[i, j] == 0 && cave[i, j] == tileType) {
						Tile tileInRegion = new Tile (i, j);
						regionTiles.Add (tileInRegion);
						tilesToProcess.Enqueue (tileInRegion);
						seen [i, j] = 1;
					}
				}
			}
		}

		return regionTiles;
	}

	List<List<Tile>> GetRegions(int tileType) {
		List<List<Tile>> regions = new List<List<Tile>> ();
		int[,] seen = new int[width, height];

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (seen[x, y] == 0 && cave [x, y] == tileType) {
					List<Tile> region = GetRegion (x, y);

					regions.Add (region);

					foreach (Tile tile in region) {
						seen[tile.x, tile.y] = 1;
					}
				}
			}
		}

		return regions;
	}

	private bool SpaceRegionTooSmall(List<Tile> region) {
		return region.Count < minSpaceRegionSize;
	}

	private bool WallRegionTooSmall(List<Tile> region) {
		return region.Count < minWallRegionSize;
	}

	void CleanCave() {
		List<List<Tile>> spaceRegions = GetRegions (0);
		foreach (List<Tile> region in spaceRegions) {
			if (region.Count < minSpaceRegionSize) {
				foreach (Tile tile in region) {
					cave [tile.x, tile.y] = 1;
				}
			}
		}

		List<List<Tile>> wallRegions = GetRegions (1);
		foreach (List<Tile> region in wallRegions) {
			if (region.Count < minWallRegionSize) {
				foreach (Tile tile in region) {
					cave [tile.x, tile.y] = 0;
				}
			}
		}
	}

//	void OnDrawGizmos() {
//		if (cave != null) {
//			for (int x = 0; x < width; x ++) {
//				for (int y = 0; y < height; y ++) {
//					Gizmos.color = (cave[x,y] == 1)?Color.black:Color.white;
//					Vector3 pos = new Vector3(-width/2 + x + .5f,0, -height/2 + y+.5f);
//					Gizmos.DrawCube(pos,Vector3.one);
//				}
//			}
//		}
//	}

	void InstantiateWalls() {
		if (cave != null) {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					if (cave [x, y] == 1) {
						Vector2 wallPos = new Vector2 (x - width / 2, y - height);
						if (availableWalls.Count < 1) {
							Instantiate (wall, wallPos, Quaternion.identity);
						} else {
							GameObject newWall = availableWalls [0];
							availableWalls.RemoveAt (0);
							newWall.transform.position = wallPos;
							newWall.SetActive (true);
						}
					}
				}
			}
		}
	}

	void RecallWalls() {
		availableWalls.Clear ();
		GameObject[] activeWalls = GameObject.FindGameObjectsWithTag ("Wall");

		foreach (GameObject wall in activeWalls) {
			wall.SetActive (false);
			availableWalls.Add (wall);
		}
	}
}
