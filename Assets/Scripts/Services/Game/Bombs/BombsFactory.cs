using test.project.Controllers;

namespace test.project.Services
{
	public class BombsFactory : ModelViewFactory<BombController, BombModel>
	{
		public BombsFactory(IPrefabsPoolingService poolingService) : base(poolingService)
		{
		}

	}
}