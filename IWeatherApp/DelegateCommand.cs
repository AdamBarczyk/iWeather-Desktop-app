using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IWeatherApp
{
    class DelegateCommand : ICommand
    {
        private SimpleEventHandler handler;
        private bool isEnabled = true;
        public event EventHandler CanExecuteChanged;
        public delegate void SimpleEventHandler();

        public DelegateCommand(SimpleEventHandler handler)
        {
            this.handler = handler;
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
        }

        void ICommand.Execute(object org)
        {
            this.handler();
        }

        bool ICommand.CanExecute(object org)
        {
            // jesli zwraca false, to nie jest mozliwe naciesniecie przycisku - z czego to wynika?
            return this.IsEnabled;
        }

        private void OnCanExecuteChanged()
        {
            if (this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
