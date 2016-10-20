using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPT_MMAS.Iot.ViewModel
{
    public class PatientsViewModel : Shared.ViewModel.PatientsViewModel
    {
        private ShellViewModel _shellVM { get; set; }

        public void ChangeOperationMode(OperationMode mode)
        {
            _shellVM.CurrentOperation = mode;
        }

        public override void Activate(object parameter)
        {
            base.Activate(parameter);
            _shellVM = (new ViewModelLocator()).Shell;
            
        }
    }
}
