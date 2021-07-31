using System;
using Controller.Game;
using Controller.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller.Input
{
	public class InputController : RootController
	{
		public Vector2 move;
		public bool isMove;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool interact;
		public bool aim;
		
		//Events
		public static event Action OnStop;

		//Handlers
		public void OnMove(InputValue value)
		{
			if(Vector2.zero.Equals(value.Get<Vector2>()) && isMove)
			{
				OnStop?.Invoke();
			}
			isMove = !Vector2.zero.Equals(value.Get<Vector2>());
			MoveInput(value.Get<Vector2>());
			
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
		
		public void OnInteract(InputValue value)
		{
			if (value.isPressed)
			{
				ChiuskyController.HandleInteract(value);
			}
		}
		
		public void OnPause(InputValue value)
		{
			if (value.isPressed)
			{
				GameController.HandlePause(value);
			}
		}
		
		public void OnAim(InputValue value)
		{
			ChiuskyController.HandleAim(value);
		}

		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newState)
		{
			jump = newState;
		}

		public void SprintInput(bool newState)
		{
			sprint = newState;
		}
	}
	
}