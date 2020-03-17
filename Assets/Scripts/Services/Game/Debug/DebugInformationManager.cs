using test.project.Controllers;
using UnityEngine;

namespace test.project.Services
{
	public class DebugInformationManager : IDebugInformationManager, IUpdatable
	{
		private DebugPanelController _panelController;
		private IUnityEventManager _eventManager;
		private Camera _camera;

		public DebugInformationManager(DebugPanelController panelController, Camera camera)
		{
			_camera = camera;
			_panelController = panelController;
		}
		public void Inject(IContainer container)
		{
			_eventManager = container.Get<IUnityEventManager>();
		}

		public void Prepare()
		{
			
		}

		public void Start()
		{
			_eventManager.Add(this);
		}

		public void Reset()
		{
			throw new System.NotImplementedException();
		}

		public void Clear()
		{
			_eventManager.Remove(this);
		}

		public void OnUpdate(float deltaTime)
		{
			Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out var hit, float.MaxValue, LayerMask.GetMask("Chars")))
			{
				CharController charController = hit.collider.GetComponent<CharController>();
				_panelController.SetActive(true);
				_panelController.SetHealth(charController.Model.Health);
				Vector2 panelSize = _panelController.GetSize();
				_panelController.SetPosition(Input.mousePosition + new Vector3(-panelSize.x / 2, panelSize.y));
			}
			else
			{
				_panelController.SetActive(false);
			}
			
		}
	}
}