using System.Collections;
using Controller.Input;
using UnityEngine;

namespace Controller.Items
{
    public class PickableController : MonoBehaviour
    {
        private Animator _anim;
        private InputController _inputHandler;
    
        private bool _pickedUp;
        private int _animIDTake;

        void Start ()
        {
            GameObject player = GameObject.FindWithTag("Chiusky");
            _anim = player.GetComponent<Animator>();
            _inputHandler = player.GetComponent<InputController>();
        
            AssignAnimationIDs();
        }
    
        private void AssignAnimationIDs()
        {
            _animIDTake = Animator.StringToHash("take");
        }

        void OnTriggerStay(Collider player) {
            Debug.Log(player.tag);
            if (player.CompareTag("Chiusky"))
            {
                if (_inputHandler.interact && !_pickedUp)
                {
                    _pickedUp = true;
                    StartCoroutine(nameof(PlayAnim));
                }
            }
        }

        private IEnumerator PlayAnim()
        {
            _anim.SetTrigger(_animIDTake);
            yield return new WaitForSeconds(1.3f);
            Destroy(gameObject);
        }
    }
}
