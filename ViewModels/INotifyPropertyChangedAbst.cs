using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HorasExtras.ViewModels;

public class INotifyPropertyChangedAbs : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Método protegido, de modo que solo las clases derivadas o la propia clase puedan invocar este evento.
        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
