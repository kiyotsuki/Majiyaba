using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Majiyaba
{
	public class ActorMove : MonoBehaviour
	{
		public bool IsReqestMove()
		{
			return requestMove;
		}

		public void RequestRotation()
		{

		}

		public void RequestMove(Vector3 point)
		{
			requestMove = true;
			targetPoint = point;

			var diff = point - transform.position;
			var rot = Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg;

			targetRotation = Quaternion.Euler(0, rot, 0);
			startRotation = transform.rotation;
			rotateTimer = 0;			
		}

		public void Start()
		{
			animator = GetComponent<Animator>();
			body = GetComponent<Rigidbody>();
		}

		public void Update()
		{
			float speed = 0;
			if(requestMove)
			{
				if(rotateTimer < rotateSpeed)
				{
					rotateTimer += Time.deltaTime;
					var rate = rotateTimer / rotateSpeed;

					transform.rotation = Quaternion.Lerp(startRotation, targetRotation, rate);
				}


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
					speed = moveSpeed;
					body.velocity = velocity;
				}

				animator.SetFloat("MoveSpeed", speed);
			}
		}

		private Animator animator = null;
		private Rigidbody body = null;

		private bool requestMove = false;
		
		private Vector3 targetPoint;

		private Quaternion startRotation;
		private Quaternion targetRotation;

		private float rotateTimer = 0;

		[SerializeField]
		private float moveSpeed = 2;

		[SerializeField]
		private float rotateSpeed = 0.3f;
	}
}