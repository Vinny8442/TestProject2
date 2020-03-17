using System;
using test.project.Services;
using UnityEditor;
using UnityEngine;

namespace test.project.Controllers
{
	public class BombController : MonoBehaviour, IModelController<BombModel>
	{
		private Rigidbody _rigidbody;

		public void Init(BombModel model)
		{
			Model = model;
			transform.position = model.Position;
			GetComponent<DefaultColorController>()?.SetColor(model.Config.Color);
			_rigidbody = GetComponent<Rigidbody>();
			_rigidbody.velocity = Vector3.zero;
		}

		public BombModel Model { get; private set; }

		private void OnCollisionEnter(Collision other)
		{
			Model?.HandleCollision();
		}

		private void FixedUpdate()
		{
			if (Model == null) return;
			Model.HandlePositionUpdated(transform.position);
			if (_rigidbody.velocity.x != 0 || _rigidbody.velocity.z != 0)
			{
				Selection.SetActiveObjectWithContext(gameObject, null);
				Debug.Break();
			}
		}
	}
}