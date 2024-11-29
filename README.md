## Horas Extras
Public App build it in .Net Maui for android, for register and manage the extra hours generate in the day. This app allow to the user to generate a complete report about the employees and the hours.

## Technologies
This app is develop it in .Net Maui version 9, and using SQLite.

## Characteristics

- Add employees
- Add and manage the projects
- Add Extras peer project
- Generate a pdf report of the extras 

## Entities of the Database

- Employeee ( Name, LastName, SecondLastName )
- Project ( Name, Type)
- Extras ( Day, Employee, Project, EntryHour, ExitHour, TotalTime)

## Nuget package use it
- Microsoft.EntityFrameworkCore.Sqlite.Core
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.Maui.Controls
- Newtonsoft.Json
