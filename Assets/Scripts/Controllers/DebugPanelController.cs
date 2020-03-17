using UnityEngine;
using UnityEngine.UI;

namespace test.project.Controllers
{
	public class DebugPanelController : MonoBehaviour
	{
		[SerializeField] private Text _healthText;
		
		public void SetActive(bool value)
		{
			gameObject.SetActive(value);
		}

		public void SetHealth(float value)
		{
			_healthText.text = $"Health: {value}";
		}

		public void SetPosition(Vector3 position)
		{
			transform.position = position;
		}

		public Vector2 GetSize()
		{
			RectTransform rectTransform = (RectTransform)transform;
			return new Vector2(rectTransform.rect.width, rectTransform.rect.height);
		}
	}
}