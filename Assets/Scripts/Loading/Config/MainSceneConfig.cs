using UnityEngine;

namespace test.project
{
	[CreateAssetMenu(fileName = "new Main Scene Config", menuName = "__TestProject/MainSceneConfig", order = 0)]
	public class MainSceneConfig : ScriptableObject, IConfigData
	{
		public int MaxChars;
		public float CharsSpawnTimeout;
		public int MaxBombs;
		public float BombSpawnTimeout;

		public int[] CharacterIds;
		public int[] BombIds;

		public Rect BombSpawnArea;
		public Rect CharSpawnArea;
		public int GetId() => 0;
	}
}