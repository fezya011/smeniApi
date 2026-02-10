using System.Collections.ObjectModel;
using AvaloniaApplication1.Models;
using AvaloniaApplication1.VmTools;

namespace AvaloniaApplication1.ViewModels;

public class ShiftsViewModel : BaseVM
{
    private ObservableCollection<Shift> _shifts;

    public ObservableCollection<Shift> Shifts
    {
        get => _shifts;
        set
        {
            if (Equals(value, _shifts)) return;
            _shifts = value;
            OnPropertyChanged();
        }
    }

    public ShiftsViewModel()
    {
        
    }
}