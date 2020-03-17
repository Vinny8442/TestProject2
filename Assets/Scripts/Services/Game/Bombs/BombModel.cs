using UnityEngine;

namespace test.project.Services
{
	public class BombModel : IModel
	{
		private IBombEventHandler _eventHandler;
		public BombConfig Config { get; private set; }
		public Vector3 Position { get; private set; }

		public BombModel(BombConfig config, Vector3 position, IBombEventHandler eventHandler)
		{
			_eventHandler = eventHandler;
			Config = config;
			Position = position;
		}

		public string PrefabLinkage => Config.PrefabLinkage;

		public interface IBombEventHandler
		{
			void HandleExplosion(BombModel bomb);
		}

		public void HandleCollision()
		{
			_eventHandler.HandleExplosion(this);
		}

		public void HandlePositionUpdated(Vector3 position)
		{
			Position = position;
			if (position.x < -50 || position.z < -50 || position.x > 50 || position.z > 50)
			{
				Debug.Break();
			}
		}

	}
}