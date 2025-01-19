# Sprint 3 Register and Authentication

# Наследование: базовая модель

Создайте абстрактный класс `Base`, который будет служить целью базовой модели для всех моделей.

```Csharp
public abstract class Base
{
    public int Id { get; set; }
    public DateTime CreatedAt  { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set;} = DateTime.UtcNow;
}
```

Теперь можно отнаследоваться от модели `Base`, а все общие свойства убрать из моделей. Например, класс `User` будет выглядеть следующим образом:

```Csharp
public class User : Base
{
  // свойства
}
```

# Entity Framework Core

Для реализации взаимодействия с реляционной базой данных мы будем использовать ORM - `Entity Framework Core`.

Для этого надо установить пакеты относительно версии .NET, на которой создан проект, следующим способом:

- пакет Microsoft.EntityFrameworkCore
  `dotnet add .\SampleApp.API\ package Microsoft.EntityFrameworkCore -v 9.0.0`

для миграций

- `Microsoft.EntityFrameworkCore.Tools`
- `Microsoft.EntityFrameworkCore.Design`

провайдер для базы данных `PostgreSQL`

- `Npgsql.EntityFrameworkCore.PostgreSQL`

При установке пакетов надо соблюдать версионность относительно версии фреймворка. В данном приложении применяется `net9.0`

**Замечание**: установить пакеты можно несколькими способами:

- dotnet cli
- установка с помощью графических пакетов
- Visual Studio
- через файл `csproj``

В результате добавления пакетов в `SampleApp.API.csproj` будет выглядеть следующим образом:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="FluentValidation" Version="11.11.0" />
    <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="9.0.1" />
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.1" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.1" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="9.0.1" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.1" />
    <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="9.0.3" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="7.2.0" />
  </ItemGroup>

</Project>
```

Далее, создайте папку `Data` и добавьте класс `SampleAppContext`. Это класс будет конфигурировать отображение моделей данных на таблицы в базе данных.

```Csharp
public class SampleAppContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public SampleAppContext(DbContextOptions<SampleAppContext> opt) : base(opt)
    {

    }

}
```

**Замечание**: для внесения строки подключения прямо в класс контекста можно применить метод:

```Csharp
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=SampleAppCourse;Username=;Password=");
    }
```

# Конфигурация строки подключения

В `appsettings.json` внесите новую секцию:

```json
  "ConnectionStrings": {
    "MSSQL": "Server=localhost,1433;Database=SampleApp;Trust Server Certificate=True;MultipleActiveResultSets=true",
    "MSSQLAuth": "Server=localhost,1433;Database=SampleApp;UserId=yourUsername;Password=yourPassword;MultipleActiveResultSets=true;",
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SampleApp;Trusted_Connection=True;MultipleActiveResultSets=true",
    "PostgreSQL": "Host=localhost;Port=5432;Database=SampleApp;Username=postgres;Password=root",
    "SQLite" : "Data Source =SampleApp.db;Foreign Keys=True;"
  }
```

# Настройка контейнера для контекста базы данных

Program.cs

```Csharp
builder.Services.AddDbContext<SampleAppContext>(o => o.UseSqlite(builder.Configuration.GetConnectionString("SQLite")));
//builder.Services.AddDbContext<SampleAppContext>(o => o.UseNpgsql(builder.Configuration["ConnectionStrings:PostgreSQL"]));
```

# Миграции

В рабочей директории создайте первую миграцию.

`dotnet ef migrations add Initial -s SampleApp.API -p SampleApp.API`

Далее, выполните эту миграцию. То есть EF создаст реальные таблицы в базе данных PostgreSQL на основе классов моделей данных указанных в контексте базы данных `SampleAppContext`.

`dotnet ef database update -s SampleApp.API -p SampleApp.API`

> **Замечание**: опция `-s` - это стартовый проект, опция `-p` - это текущий проект. Либо, можно зайти в проект `SampleApp.API` явно и не прописывать данные параметры. Также при создании и выполнении миграции сборка решения должна происходить успешно.

> **Замечание**: если база данных уже существует, то удалите базу данных `dotnet ef database drop -s SampleApp.API -p SampleApp.API`,а затем примените миграцию

> **Замечание**:

- если вы получаете ошибку `Host cant be null`, то надо проверить правильность строки подключения.
- если в ошибке написано об отсутствии базы данных, то надо создать базу даннхы либо на сервере баз данных, либо в конструкторе контекста `SampleAppContext` с помощью метода `Database.EnsureCreated()`:

```Csharp
   public SampleAppContext()
    {
        Database.EnsureCreated();
    }
```

# Настройка хранения паролей

- добавьте в модель `User` новые свойства

```Csharp
    public required string Login {get; set;}
    public required byte[] PasswordHash {get; set;}
    public required byte[] PasswordSalt {get; set;}
```

- теперь создайте новую миграцию по именем `AddLoginAndPasswordToUsers`
- примените миграцию

# Создание репозитория для хранения пользователей в базе данных

- создайте в папке `Repositories` новый класс `UsersLocalRepository`,который будет реализовывать интерфейс `IUserRepository`, который мы определили в `sprint1`, но механизм хранения будет использовать базу данных `SQLite`.

Обратите внимание какую реализацию будет использовать конструктор репозитория и зарегистрируйте в DI нужные классы. Ниже приводится начальная реализация методов для создания пользователя и просмотра всех пользователей.

```Csharp
public class UsersLocalRepository : IUserRepository
{
    private readonly SampleAppContext db;
    public UserRepository(SampleAppContext db)
    {
        db = db;
    }

    public List<User> GetUsers()
    {
        return db.Users.ToList();
    }

    public User CreateUser(User user)
    {
       try
       {
         db.Add(user);
         db.SaveChanges();
         return user;
       }
       catch(SqlTypeException ex)
       {
         throw new SqlTypeException($"Ошибка SQL: {ex.Message}");
       }
       catch (Exception ex)
       {
         throw new Exception($"Ошибка: {ex.Message}");
       }
    }

    // another methods interface without implementation
}
```

Но теперь у нас проблемы с созданием пользователя через POST запрос, потому что невозможно передать в json массив байтов. Т.е объект пользователя не прийдет в контроллер. Выходом из этой ситуации может стать создание нового объекта передачи данных `UserDto`, который содержит нужные поля.


# DTO

Создайте модель для передачи данных `UserDto` в папке `Dto`

```Csharp
public class UserDto
{
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
```

**Замечание**: Эквивалентом и краткой записью для класса со свойствами является тип `record`:

```Csharp
public record UserRecordDto {
  public required string Login {get; init;}
  public required string Password {get; init;}
};
```

- в методе создания пользователя в ```UsersControoler``` замените входную модель ```User user``` на ```UserDto userDto```

- сформируйте из userDto объект user в этом же методе.

```Csharp
var user  = new User(){
        Login = userDto.Login,
        PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password)),
        PasswordSalt = hmac.Key,
        Name = ""
};
```

> Объект hmac заведите из приватного свойства в контроллере ```private HMACSHA256 hmac = new HMACSHA256();```

- потом новый объект ```user``` должен пройти валидацию, но на данный момент валидация настроена на поле Name. Измените валидацию так, чтобы проверялось поле Login

- проверьте работу метода конттроллера по созданию пользователя. Модель ```userDto``` прийдет на вход метода контроллера ```CreateUser```, далее из этой модели будет создан объект ```user```. Потом ```user``` пройдет валидацию и попадет в метод репозитория, который сохранит его в базу данных.



# Seed Data - генерация тестовых данных

- установите библиотеку `Bogus`

- создайте новый контроллер `SeedController` в котором реализуйте следующий метод для генерации пользователей:
- создайте конструктор и внедрите в него контекст работы с базой данных ```SampleAppContext db```

```Csharp
    [HttpGet("generate")]
    public ActionResult SeedUsers(){

        using var hmac = new HMACSHA512();

        Faker<UserRecordDto> _faker = new Faker<UserRecordDto>("en")
            .RuleFor(u => u.Login, f => GenerateLogin(f).Trim())
            .RuleFor(u => u.Password, f => GeneratePassword(f).Trim().Replace(" ",""));

        string GenerateLogin(Faker faker)
        {
           return faker.Random.Word() + faker.Random.Number(3,5);
        }

        string GeneratePassword(Faker faker)
        {
           return faker.Random.Word() + faker.Random.Number(3, 5);
        }

        var users = _faker.Generate(100).Where(u => u.Login.Length > 4 && u.Login.Length <= 10);

        List<User> userToDb = new List<User>();

        try
        {
            foreach (var user in users)
            {
                var u = new User()
                {
                    Login = user.Login,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password)),
                    PasswordSalt = hmac.Key,
                };
                userToDb.Add(u);
            }
            db.Users.AddRange(userToDb);
            db.SaveChanges();
        }
        catch(Exception ex)
        {
           Console.WriteLine($"{ex.InnerException.Message}");
        }

        return Ok(userToDb);
    }
```


# EF Configuration (option)

Настройка каждого атрибута, связей, типов данных в конкретной базе данных

# Reverse engineering (option)

Cуществует обратное проектирование - по готовой базе данных восстановить модели данных - скаффолдинг.

# EF HasData (option)

В методе `OnModelCreating` контекста базы данных можно генерировать тестовые данные, а также применять пользовательские конфигурации.

```Csharp
        protected override void OnModelCreating(ModelBuilder builder){

            builder.ApplyConfigurationsFromAssembly(typeof(SampleAppContext).Assembly);

            builder.Entity<User>().HasData(
                new User(){ Id = 1, Name = "user", ...}
            );

        }
```

# SaveChanges (option)

Переопределение метода для обновление базовой модели

```Csharp
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken){

            foreach(var entry in ChangeTracker.Entries<Base>())
            {
                switch(entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
```

**Задание 1**: создайте базовый API контроллер.

**Задание 2**: можно создать реализацию репозитория пользователей, который будет работать с базой данных SQL Server. Для этого примените пакет `Microsoft.EntityFrameworkCore.SqlServer`.
