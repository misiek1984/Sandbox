using System;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace MVVMLight.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private int? _number = null;
        private int _numberOfAttempts = 0;

        private Brush _Color = new SolidColorBrush(Colors.Green);
        public Brush Color
        {
            get { return _Color; }
            set
            {
                _Color = value;
                RaisePropertyChanged(() => Color);
            }
        }

        private string _message = "";
        public string Message
        {
            get { return _message; }
            set 
            {
                _message = value;
                RaisePropertyChanged(() => Message);
            }
        }

        private string _input;
        public string Input
        {
            get { return _input; }
            set
            {
                _input = value;
                RaisePropertyChanged(() => Input);
            }
        }

        private RelayCommand _NewNumber;
        public RelayCommand NewNumber
        {
            get
            {
                if(_NewNumber == null)
                    _NewNumber = new RelayCommand(() =>
                        {
                            Messenger.Default.Send<NewNumber>(new NewNumber());
                            
                            _number = (int)(DateTime.Now.Ticks % 1000);
                            _numberOfAttempts = 0;
                            Message = "";
                        });

                return _NewNumber;
            }
        }

        private RelayCommand _Check;
        public RelayCommand Check
        {
            get
            {
                if (_Check == null)
                    _Check = new RelayCommand(() =>
                        {
                            var res = 0;
                            if (Int32.TryParse(_input, out res))
                            {
                                _numberOfAttempts++;

                                if (res == _number)
                                    Message = "Success!";
                                else if (res > _number)
                                    Message = String.Format("Shoould be smaller! Number of attemtps: {0}",
                                                            _numberOfAttempts);
                                else if (res < _number)
                                    Message = String.Format("Shoould be greter! Number of attemtps: {0}",
                                                             _numberOfAttempts);
                            }
                            else
                            {
                                Message = String.Format("'{0}' is not a number!", _input);
                            }
                        },
                    () => _number != null);

                return _Check;
            }
        }

        private RelayCommand _MouseEnter;
        public RelayCommand MouseEnter
        {
            get
            {
                if (_MouseEnter == null)
                    _MouseEnter = new RelayCommand(() =>
                    {
                        Color = new SolidColorBrush(Colors.Red);
                    });

                return _MouseEnter;
            }
        }

        private RelayCommand _MouseLeave;
        public RelayCommand MouseLeave
        {
            get
            {
                if (_MouseLeave == null)
                    _MouseLeave = new RelayCommand(() =>
                        {
                            Color = new SolidColorBrush(Colors.Green);
                        });

                return _MouseLeave;
            }
        }

        public MainViewModel()
        {
            Message = "Let's start";
        }
    }
}