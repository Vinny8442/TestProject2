using UnityEngine;
using UnityEngine.SceneManagement;

namespace test.project
{
	[CreateAssetMenu(fileName = "New Game Config", menuName = "__TestProject/GameConfig", order = 0)]
	public class GameConfig : ScriptableObject, IConfigData
	{
		public string PreloaderScene;
		public string MainScene;
		
		public int GetId() => 0;
	}

	public enum MoveMethod
	{
		MovePosition,
		ForcesWithDrag,
		SetVelocityThenStop
	}
}