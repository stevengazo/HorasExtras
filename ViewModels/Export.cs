using System;
using System.Collections.ObjectModel;
using HorasExtras.Data;
using HorasExtras.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace HorasExtras.ViewModels;

public class Export : INotifyPropertyChangedAbs
{
    private DateTime _InitialDate { get; set; } = DateTime.Today;
    public DateTime InitialDate
    {
        get
        {
            return _InitialDate;
        }
        set
        {
            _InitialDate = value;
            if (_InitialDate != null)
            {
                OnPropertyChanged(nameof(InitialDate));
            }

        }
    }
    private DateTime _FinalDate { get; set; } = DateTime.Today;
    public DateTime FinalDate
    {
        get
        {
            return _FinalDate;
        }
        set
        {
            _FinalDate = value;
            if (_FinalDate != null)
            {
                OnPropertyChanged(nameof(FinalDate));
            }

        }
    }
    private ObservableCollection<Extras> _extras { get; set; } = new();
    public ObservableCollection<Extras> ListExtras
    {
        get
        {
            return _extras;
        }
        set
        {
            _extras = value;
            if (_extras != null)
            {
                OnPropertyChanged(nameof(ListExtras));
            }
        }
    }

    public Export()
    {
        ISearch = new Command( async()=> SearchExtrasAsync());

    }

    public Command ISearch { get; set; }
    private async Task SearchExtrasAsync()
    {
        ListExtras.Clear();
        using (var db = new ProjectHoursContext())
        {
            var results = await (from i in db.Extras
                                 where i.Day >= InitialDate && i.Day <= FinalDate
                                 orderby i.Day descending
                                 select i)
                                .Include(e => e.Employee)
                                .Include(e => e.Project)
                                .AsNoTracking()
                                .ToListAsync();
            foreach (var extras in results)
            {
                ListExtras.Add(extras);
            }

        }
    }

}
