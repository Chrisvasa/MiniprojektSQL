# A C# Timesheet console application

A simple timesheet console application made in C# that uses a Postgresql database.

Allows the user to both manage projects and users from the application.

## Features

- View, create, edit and remove projects

- Create, edit and remove users

## Demo

![](https://i.imgur.com/Lv3dxqF.gif)

## Usage/Examples

The program is a pretty basic application and at launch you are met with:

- A User Menu - Which allows you to select a user, create, edit and remove users from the database.

- A Project Menu - Which allows you to view, edit projects and hours spent for the selected person, create and remove projects from the database.

- An option to Exit the program

**Code Structure:**

| Method Name | Method Description |
| ------------- | ------------- |
| Program.cs | Which only has the purpose to run the program by calling a method in Timesheet.cs |
| Timesheet.cs | The class that contains all the methods for the users and projects. |
| DataAccess.cs  | A class that handles the access to the database and all the different SQL transactions to get or insert data. |
| ProjectModel.cs and PersonModel.cs | These are used to store and to have easier access to the database data. |
| Menu.cs | Prints out all the different menus and allows the user to interact in an easier manner with the application.|
| InputHandler.cs | Makes sure that only correct input is given when navigating the menu system.|


## "Tech Stack"

**Languages:** C# & SQL

**Database:** Postgresql
