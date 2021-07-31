using System.Collections.Generic;
using Controller.Items;
using Entity.Dialog;
using Enumeral;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class ChiuskyController : PlayerController
    {
        //Public-props
        [Header("Chiusky")] [Tooltip("speed in m/s")]
        public float moveSpeed = 2.0f, sprintSpeed = 5.335f, aimSpeed = 0.1f;

        [Tooltip("Acceleration and deceleration")]
        public float speedChangeRate = 10.0f;

        [Tooltip("How fast the character turns to face movement direction")] [Range(0.0f, 0.3f)]
        public float rotationSmoothTime = 0.12f;

        //Update
        private void Update()
        {
            //Debug.Log(EventSystem.currentSelectedGameObject?.name);
            if (GameStatus.play.Equals(GameController.GetStatus()))
            {
                HandleGravity();
                if (!hold)
                {
                    HandleMovement();
                }
            }
        }

        //Handlers
        public void HandleAim(InputValue inputValue)
        {
            if (GameStatus.play.Equals(GameController.GetStatus()))
                Animator.SetBool(AnimIDAim, inputValue.isPressed);
        }

        public void HandleHit()
        {
            Debug.Log("HIT");
        }

        public void HandleInteract(InputValue value)
        {
            if (GameStatus.play.Equals(GameController.GetStatus()))
            {
                if (Animator.GetBool(AnimIDAim))
                {
                    HandleCombat();
                }
                else if (GetFocusItem())
                {
                    HandleStop();
                    var interactable = GetFocusItem().GetComponent<InteractableController>();
                    interactable.DoInteract();
                }
            }
            else if (GameStatus.interact.Equals(GameController.GetStatus()))
            {
                DialogController.Click_Window();
                if (State.Deactivate.Equals(DialogController.state))
                {
                    GameController.SetStatus(GameStatus.play);
                    HandleStart();
                }
            }
        }

        private void HandleCombat()
        {
            Animator.SetTrigger(AnimIDAttack);
        }

        private void HandleGravity()
        {
            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            // ... ??? whatever it works for going up and down stairs / climbs
            if (verticalVelocity < 53.0f)
            {
                verticalVelocity += -15.0f * Time.deltaTime;
            }
        }

        private void HandleMovement()
        {
            //SET INITIAL SPEED
            var targetSpeed = Animator.GetBool(AnimIDAim) ? aimSpeed : InputController.sprint ? sprintSpeed : moveSpeed;
            if (InputController.move == Vector2.zero) targetSpeed = 0.0f;
            speed = targetSpeed;

            //player absolute direction
            var inputDirection = new Vector3(InputController.move.x, 0.0f, InputController.move.y).normalized;

            //acceleration (optional)
            var velocity = CharacterController.velocity;
            var currentHorizontalSpeed = new Vector3(velocity.x, 0.0f, velocity.z).magnitude;
            //Debug.Log($"currentHorizontalSpeed :: {currentHorizontalSpeed}");

            const float speedOffset = 0.1f;
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * 1f, Time.deltaTime * speedChangeRate);
                speed = Mathf.Round(speed * 1000f) / 1000f;
            }
            else
            {
                speed = targetSpeed;
            }
            //end acceleration snippet

            //Rotation
            if (InputController.move != Vector2.zero)
            {
                targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                 CameraController.GetFixedPosition();
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
                    rotationSmoothTime);
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
            else
            {
                speed = 0.0f;
            }

            //MOVE
            var targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
            CharacterController.Move(targetDirection.normalized * (speed * Time.deltaTime) +
                                     new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);

            //AnimationBlend
            animationBlend = Mathf.Lerp(animationBlend, speed, Time.deltaTime * speedChangeRate);
            Animator.SetFloat(AnimIDSpeed, animationBlend);
        }

        private void HandleStop()
        {
            hold = true;
            speed = 0f;
            CharacterController.SimpleMove(Vector3.zero);
            Animator.SetFloat(AnimIDSpeed, 0f);
        }

        private void HandleStart()
        {
            hold = false;
        }
    }
}