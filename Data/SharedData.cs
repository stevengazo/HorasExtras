using System;
using HorasExtras.Models;

namespace HorasExtras.Data;

public static class SharedData
{

    public static Project SelectedProject { get; set; } = new Project();
    public static Extras SelectedExtra { get; set; } = new Extras();

}
