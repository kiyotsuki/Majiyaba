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
			requestMove = true;
			targetPoint = point;

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
			addAngle = angle + rotation.eulerAngles.y;


			addAngle = Mathf.Atan2(target.x, target.z) ;

			rotation.SetEulerAngles(0, addAngle, 0);

			gameObject.transform.rotation = rotation;
		}

		public void Start()
		{
			body = gameObject.GetComponent<Rigidbody>();
		}

		public void Update()
		{
			if(requestMove)
			{
				Vector3 pos = gameObject.transform.position;

				var dist = Vector2.Distance(new Vector2(pos.x, pos.z), new Vector2(targetPoint.x, targetPoint.z));
				if(dist < moveSpeed * Time.deltaTime)
				{
					requestMove = false;
				}
				else
				{
					var velocity = targetPoint - pos;
					velocity = velocity.normalized * moveSpeed;
					body.velocity = velocity;
				}
			}
		}

		private Rigidbody body = null;

		private bool requestMove = false;
		private Vector3 targetPoint;
		private float addAngle = 0;

		private float rotateTimer = 0;

		[SerializeField]
		private float moveSpeed = 2;

		[SerializeField]
		private float rotateSpeed = 1;
	}
}