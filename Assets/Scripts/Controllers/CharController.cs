using test.project.Services;
using UnityEngine;

namespace test.project.Controllers
{
	public class CharController : MonoBehaviour, IModelController<CharacterModel>
	{
		public CharacterModel Model { get; private set; }

		public void SetColor(Color color)
		{
			GetComponent<DefaultColorController>()?.SetColor(color);
		}

		public void Init(CharacterModel character)
		{
			Model = character;
			SetColor(character.Config.Color);
			transform.position = character.Position;
		}

	}
}