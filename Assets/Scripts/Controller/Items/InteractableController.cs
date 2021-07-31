using Controller.Player;
using Entity.Interface;
using UnityEngine;

namespace Controller.Items
{
    public class InteractableController : RootController
    {
        private void OnTriggerEnter(Collider other)
        {
            if ("Chiusky".Equals(other.gameObject.tag))
            {
                ChiuskyController chiuskyCtrl = other.gameObject.GetComponent<ChiuskyController>();
                chiuskyCtrl.SetFocusItem(this.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if ("Chiusky".Equals(other.gameObject.tag))
            {
                var chiuskyCtrl = other.gameObject.GetComponent<ChiuskyController>();
                if (this.gameObject.Equals(chiuskyCtrl.GetFocusItem()))
                {
                    chiuskyCtrl.SetFocusItem(null);
                }
            }
        }

        public void DoInteract()
        {
            var interactable = this.gameObject.GetComponent<Interactable>();
            DialogController.CustomShow(interactable.GetDialogData());
        }
    }
}