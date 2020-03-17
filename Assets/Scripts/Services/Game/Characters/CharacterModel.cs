using UnityEngine;

namespace test.project.Services
{
	public class CharacterModel : IModel
	{
		public float Health { get; private set; }
		public CharacterConfig Config { get; private set; }
		public float TotalHealth => Config.Health;
		public Vector3 Position { get; private set; }
		public bool IsDead => Health <= 0;
		private float _armour;
		private CharacterEventHandler _eventHandler;

		public CharacterModel(CharacterConfig config, Vector3 position, CharacterEventHandler eventHandler)
		{
			_eventHandler = eventHandler;
			Config = config;
			Health = config.Health;
			_armour = config.Armour;
			Position = position;
		}

		public void ReceiveDamage(float damage)
		{
			var isDead = IsDead;
			Health -= damage * (1 - _armour);
			if (!isDead && IsDead)
			{
				_eventHandler.HandleIsDead(this);
			}
		}

		public interface CharacterEventHandler
		{
			void HandleIsDead(CharacterModel character);
		}

		public string PrefabLinkage => Config.PrefabLinkage;
		public int Id { get; set; }
	}
}