# Library
A test task for Sitecore
"LibraryApp" contains a few projects:
* Data - it is a class library for classes which I map with tables of database;
* Infrastruture - project for interfaces which are used in this solution;
* Services - business logic of this project;
* ViewModels - it is like DTO pattern;

To connect to a database:
View->Server Explorer->Data Connections(Right Click and choise in context menu "Add connection").
In field "Server name" I write "(LocalDB)\v11.0". File libr.mdf is located in ~/WEB/App_Data. Enter the path for this file to field "Database file name". Database is ready for use. After connection is established, click him. There is field Data Source in the "Properties" window. Copy this and insert into variable "conn". Located path for this variable: "Services/Ninject/NinjectControllerFactory.cs", in the function "Add bindings".

