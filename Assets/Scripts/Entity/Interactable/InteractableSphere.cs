using System.Collections.Generic;
using Entity.Dialog;
using UnityEngine;

namespace Entity.Interactable
{
    public class InteractableSphere : MonoBehaviour, Interface.Interactable 
    {
        public void DoInteract()
        {
            throw new System.NotImplementedException();
        }

        public List<DialogData> GetDialogData()
        {
            var dialogList = new List<DialogData>();
            var text1 = new DialogData("s e l e c t a b l e");
            text1.SelectList.Add("Correct", "one");
            text1.SelectList.Add("Wrong", "two");
            text1.Callback = () => { Debug.Log("test"); };
            dialogList.Add(text1);
            return dialogList;
        }
    }
}
