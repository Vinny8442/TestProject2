using System.Collections.Generic;
using test.project.Controllers;

namespace test.project.Services
{
	public class CharacterFactory : ModelViewFactory<CharController, CharacterModel>
	{
		private static int Counter = 0;
		private readonly Dictionary<string, PrefabsPool> _pools = new Dictionary<string, PrefabsPool>();

		public CharacterFactory(IPrefabsPoolingService poolingService) : base(poolingService)
		{
		}
	}
}