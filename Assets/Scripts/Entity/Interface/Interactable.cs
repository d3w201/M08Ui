using System.Collections.Generic;
using Entity.Dialog;

namespace Entity.Interface
{
    public interface Interactable
    {
        public void DoInteract();
        
        public List<DialogData> GetDialogData();
    }
}