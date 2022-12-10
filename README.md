# ProjectTracker
 Final project for SoftUni C# learning path.
------------------------

The idea of the application is pretty simple.
Project Tracker is an application that helps manage the work process in a working environment.
Everything is organized by Departments, Projects and Employees.
Users can submit tickets(tasks) to projects, track current tasks' progress and view overall info for a given Project and Department.

I got inspired to choose this type of project by <a href="https://www.youtube.com/@CoderFoundry">Coder Foundry</a>.
This was one of my main sources of studiying materials along with SoftUni, Tim Corey and many many more since the begining of my C# journey.

## Application Overview
---

The first thing the user sees is the Login page.

<img src="https://iili.io/HnP3qNe.md.jpg" alt="HnP3qNe.md.jpg" border="0">

It's simple - either login, register or try as Guest by selecting a role.

After the user logs in, the main page is displayed.

<img src="https://iili.io/HnP3EfR.md.jpg" alt="HnP3EfR.md.jpg" border="0">

From here You can navigate and see information Departments, Projects, Tickets and Employees

<img src="https://iili.io/HnP3esf.md.jpg" alt="HnP3esf.md.jpg" border="0">
<img src="https://iili.io/HnP380l.md.jpg" alt="HnP380l.md.jpg" border="0">
<img src="https://iili.io/HnP3vf4.md.jpg" alt="HnP3vf4.md.jpg" border="0">
<img src="https://iili.io/HnP3OWG.md.jpg" alt="HnP3OWG.md.jpg" border="0">

### Admin Panel
---

The Admin panel is accessible for users that are in admin role.

<img src="https://iili.io/HnP3Sg2.md.jpg" alt="HnP3Sg2.md.jpg" border="0">

The admin can manage roles, assign projects and tickets, remove objects and see a list of past objects.


## The application is build using he following technologies:

- ASP.NET Core 6.0
- SQL Server
- EntityFrameworkCore 6.0.1
- EntityFrameworkCore.InMemory 6.0.1
- NUnit
- Moq
- Coverlet