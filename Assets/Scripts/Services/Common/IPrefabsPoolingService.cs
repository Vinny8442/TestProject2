namespace test.project.Services
{
	public interface IPrefabsPoolingService : IService
	{
		PrefabsPool GetPool(string prefabLinkage);
	}
}