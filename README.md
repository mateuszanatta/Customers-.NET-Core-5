[![LinkedIn][linkedin-shield]][linkedin-url]

<h3 align="center">Customers .NET Core 5</h3>
<p align="center">
    A simple login/customers listing in .NET Core 5 and Entity Framework Core using Ubuntu 20.04 and SQL Server
</p>



## Instalation
1. Clone the repo
    ```sh
    git clone https://github.com/mateuszanatta/Customers-.NET-Core-5.git
    ```
2. Open the Startup.cs file and edit the following lines
    ```sh
    33 string dbUser     = "";
    34 string dbPassword = "";
    35 string dbServer   = "Server=localhost,1433";
    ```
    2. _Include your database Username and Password._
    3. `dbServer = "Server=localhost,1433"` **is the default value for linux environments. For Windows, you may need to change to** `dbServer = "Data Source=(LocalDB\MSSQLLocalDB)"` **,** `dbServer = "Data Source=(LocalDB\v11)"` **or even** `dbServer = "Server=(LocalDB\MSSQLLocalDB)"`|`dbServer = "Server=(LocalDB\v11)"`, **depending on your setup.**

3. Check if you have **permission to read and write** the .\database folder, Database.mdf and Database_log.ldf files.

4. To initialize the project, run
    ```sh
        dotnet run
    ```

5. The first login may take a while, since the database is beeing created and the data from Database.mdf is being loaded.
    
    * _During the fist login you may get a "Connection Time out" message. **Don't worry**, just try again and the Customers form will show up._


[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/mateuszanatta/