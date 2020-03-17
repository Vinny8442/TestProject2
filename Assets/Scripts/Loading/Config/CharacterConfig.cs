using System.Security.Principal;
using UnityEngine;
using UnityEngine.Serialization;

namespace test.project
{
	[CreateAssetMenu(fileName = "new Character Config", menuName = "__TestProject/CharacterConfig", order = 0)]
	public class CharacterConfig : ScriptableObject, IConfigData
	{
		public Color Color;
		public float Health;
		public float Armour;
		public int Id;
		public string PrefabLinkage;
		public int GetId() => Id;
	}
}