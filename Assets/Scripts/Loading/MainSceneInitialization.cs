using test.project.Controllers;
using test.project.Services;
using UnityEngine;

namespace test.project
{
	public class MainSceneInitialization : MonoBehaviour, ISceneInitable
	{
		[SerializeField] private DebugPanelController _debugPanel;
		[SerializeField] private Camera _mainCamera;
		
		private Container _container;

		public void Init(IContainer parent)
		{
			_container = new Container(parent);

			_container.Register<ICharacterManager>(new CharacterManager());
			_container.Register<IBombsManager>(new BombsManager());
			_container.Register<IDebugInformationManager>(new DebugInformationManager(_debugPanel, _mainCamera));

			_container.Inject();
			_container.PrepareAll();
			_container.StartAll();
		}

		public void Clear()
		{
			_container.Clear();
		}
	}
}