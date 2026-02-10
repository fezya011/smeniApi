using System.Collections.ObjectModel;
using AvaloniaApplication1.Models;
using AvaloniaApplication1.VmTools;

namespace AvaloniaApplication1.ViewModels;

public class EmployeesViewModel : BaseVM
{
    private ObservableCollection<Employee> _employees;
    private Employee _selectedEmployee;

    public ObservableCollection<Employee> Employees
    {
        get => _employees;
        set
        {
            if (Equals(value, _employees)) return;
            _employees = value;
            OnPropertyChanged();
        }
    }

    public Employee SelectedEmployee
    {
        get => _selectedEmployee;
        set
        {
            if (Equals(value, _selectedEmployee)) return;
            _selectedEmployee = value;
            OnPropertyChanged();
        }
    }

    public EmployeesViewModel()
    {
        
    }
}