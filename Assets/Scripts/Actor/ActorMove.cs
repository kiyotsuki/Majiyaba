using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Majiyaba
{
	public class ActorMove : MonoBehaviour
	{
		public void ReqestMove(Vector3 point)
		{
			_requestMove = true;
			_targetPoint = point;

			var target = point - gameObject.transform.position;
			target.y = 0;
			target.Normalize();

			var forward = gameObject.transform.forward;
			forward.y = 0;
			forward.Normalize();

			var angle = Mathf.Acos( Vector3.Dot(forward, target));

			var cross = Vector3.Cross(forward, target);
			if (cross.y < 0) angle *= -1;



			var rotation = gameObject.transform.rotation;
			_addAngle = angle + rotation.eulerAngles.y;


			_addAngle = Mathf.Atan2(target.x, target.z) ;

			rotation.SetEulerAngles(0, _addAngle, 0);

			gameObject.transform.rotation = rotation;
		}

		public void Start()
		{
			_body = gameObject.GetComponent<Rigidbody>();
		}

		public void Update()
		{
			if(_requestMove)
			{
				Vector3 pos = gameObject.transform.position;

				var dist = Vector2.Distance(new Vector2(pos.x, pos.z), new Vector2(_targetPoint.x, _targetPoint.z));
				if(dist < _moveSpeed * Time.deltaTime)
				{
					_requestMove = false;
				}
				else
				{
					var velocity = _targetPoint - pos;
					velocity = velocity.normalized * _moveSpeed;
					_body.velocity = velocity;
				}
			}
		}

		private Rigidbody _body = null;

		private bool _requestMove = false;
		private Vector3 _targetPoint;
		private float _addAngle = 0;

		private float _rotateTimer = 0;

		[SerializeField]
		private float _moveSpeed = 2;

		[SerializeField]
		private float _rotateSpeed = 1;
	}
}