using UnityEngine;

namespace test.project
{
	[CreateAssetMenu(fileName = "new Bomb Config", menuName = "__TestProject/BombConfig", order = 0)]
	public class BombConfig : ScriptableObject, IConfigData
	{
		public Color Color;
		public float DamageRadius;
		public BombDamageDecay DamageDecay;
		public float Damage;
		public float Rarity;
		public int Id;
		public string PrefabLinkage;
		public int GetId() => Id;
	}

	public enum BombDamageDecay
	{
		Constant,
		LinearFade
	}
}