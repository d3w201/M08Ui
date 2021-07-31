using UnityEngine;

namespace Controller.Player
{
    public class PlayerController : RootController
    {
        //Protected-props
        protected float speed;
        protected float targetRotation;
        protected float rotationVelocity;
        protected float verticalVelocity;
        protected float animationBlend;
        protected bool hold;

        //Private-props
        private GameObject _focusItem;

        //Getter & Setter
        public GameObject GetFocusItem()
        {
            return this._focusItem;
        }

        public void SetFocusItem(GameObject item)
        {
            this._focusItem = item;
        }

        public void SetCamera(GameObject cam)
        {
            MainCamera = cam;
        }

        public GameObject GetCamera()
        {
            return MainCamera;
        }
    }
}