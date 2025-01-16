
- откройте командную строку, создайте папку SampleApp и перейдите в нее
- выполните команду ```code .``` для открытия проекта SampleApp в Visual Code

# Подготовка редактора Visual Code

- установите расширение Material Icons для удобства работы и включите с помощью команды ```>material icon``` по F1

- в настройках settings => exclude: внесите шаблоны для ```bin``` и ```obj```

**Замечание**: при работе со средой .NET можно применять Visual Studio. Выбор редактора для курса - это предпочтение преподавателя.

# Использование dotnet cli для построения архитектуры приложения

- создайте папку ```SampleApp``` и перейдите в нее в командной строке.
- посмотрите с помощью команды ```dotnet new list``` список доступных проектов и создайте проект ```webapi``` с именем ```SampleApp.API``` командой:  ```dotnet new webapi -o SampleApp.API```

**Замечание**: в приложении будет использоваться версия .net 7.0, в версии 8.0 данная команда по умолчанию для работы с API использует ```minimalAPI```, поэтому для включения контроллеров в версии 8.0 надо использовать флаг ```-controllers```. Чтобы проверить версию наберите команду: ```dotnet --version```.

- добавьте файл решения, находясь в папке рабочей директории, командой ```dotnet new sln```

- добавьте в решение проект API - ```dotnet sln add SampleApp.API```

**Замечание**: можно добавить все проекты в решение одной командой ```dotnet sln add (ls -r **/**.csproj)```. Эта команда работает в Powershell.

- добавьте файл .gitignore (на уровне проекта API) - ```dotnet new gitignore```

- создайте файл .gitignore (на уровне рабочей корневой директории) и поместите в gitignore следующие настройки:

```
## git rm -r --cached
.vscode
```

Для проверки работоспособности приложения запустите API: ```dotnet run --project SampleApp.API```.

У вас по конечной точке http://localhost:5290/weatherforecast должен выводится результат в формате json.

**Примечание**: номер порта может быть другим.

- добавьте в решение файл ```readme.md```

- зафиксируйте изменения в git с сообщением: "Создание начального проекта API"

- от мастер-ветки создать ветку ```git switch -c sprint1``` и перейти в нее ```git switch sprint1```. Далее работа будет вестись в этой ветке. Чтобы просмотреть все ветки в проекте применяется команда ```git branch```.


## Разработка домена приложения. Модель пользователя

Создайте в проекте ```SampleApp.API``` папку ```Entities```, в которой создайте класс **User**

```Csharp
public class User{
    public int Id {get; set;}
    public string Name {get ;set;} = String.Empty;
}
```

# Интерфейсы

Создайте папку ```Interfaces``` и поместите следующий интерфейс IUserRepository

```Csharp
public interface IUserRepository
{
   User CreateUser(User user);
   List<User> GetUsers();
   User EditUser(User user, int id);
   bool DeleteUser(int id);
   User FindUserById(int id);
}
```

## Реализация CRUD в UserRepository

Создайте папку ```Repositories``` и поместите там следующий класс UserLocalRepository, который будет имплементировать (реализовывать) интерфейс ```IUserRepository```.

```Csharp
public class UserLocalRepository : IUserRepository

{
    public IList<User> Users { get; set; } = new List<User>();
  
    public User CreateUser(User user)
    {
        user.Id = Guid.NewGuid();
        Users.Add(user);
        return user;
    }

    public bool DeleteUser(Guid id)
    {
        var result = FindUserById(id);
        Users.Remove(result);
        return true;
    }

    public User EditUser(User user, Guid id)
    {
        var result = FindUserById(id);
        result.Name = user.Name;
        return result;
    }

    public User FindUserById(Guid id)
    {
        var result = Users.Where(u => u.Id == id).FirstOrDefault();

        if (result == null)
        {
            throw new Exception($"Нет пользователя с id = {id}");
        }

        return result;
    }

    public List<User> GetUsers()
    {
        return (List<User>)Users;
    }
}
```

**Примечание**: 

- очистите папку ```Controllers``` от файла WeatherForecastController и файл модели. 


# Unit Tests

Для реализации unit-тестирования функциональности методов репозитория создадим проект:

```
dotnet new xunit -o SampleApp.Tests
```

- добавьте проект SampleApp.Tests в решение. Проверьте какие проекты находятся в решении командой dotnet sln list
- добавьте ссылку в проект с тестами на проект API

```
dotnet add .\SampleApp.Tests\ reference .\SampleApp.API
```

- удалите файл ```UnitTest1``` в проекте с тестами

- создайте класс ```UserLocalRepositoryTests``` в тестовом проекте

```Csharp
public class UserLocalRepositoryTests

{
    private readonly UserLocalRepository _userLocalRepository;
    public UserLocalRepositoryTests()
    {
        _userLocalRepository = new UserLocalRepository();
    
    }

    [Fact]
    public void CreateUser_ShouldReturnNewUserWithGeneratedId()
    {
        // Arrange
        var newUser = new User { Name = "Test User" };
        // Act
        var createdUser = _userLocalRepository.CreateUser(newUser);

        // Assert
        Assert.NotNull(createdUser);
        Assert.NotEqual(Guid.Empty, createdUser.Id);
        Assert.Equal(newUser.Name, createdUser.Name);
    }

    [Fact]
    public void DeleteUser_ShouldReturnTrueAndRemoveUser()
    {
        // Arrange
        var UserLocalRepository = new UserLocalRepository();
 
        var testUser = new User { Id = Guid.NewGuid(), Name = "Test User" };
        UserLocalRepository.Users.Add(testUser);


        // Act
        bool result = UserLocalRepository.DeleteUser(testUser.Id);
 

        // Assert
        Assert.True(result);
        Assert.Empty(UserLocalRepository.Users);

    }

    [Fact]
    public void EditUser_ShouldUpdateExistingUser()
    {
        // Arrange
        var UserLocalRepository = new UserLocalRepository();
      
        var originalUser = new User { Id = Guid.NewGuid(), Name = "Original User" };
        UserLocalRepository.Users.Add(originalUser);


        // Act
        var editedUser = new User { Id = originalUser.Id, Name = "Edited User" };
        var result = UserLocalRepository.EditUser(editedUser, originalUser.Id);
      

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Edited User", result.Name);
        Assert.Single(UserLocalRepository.Users);
   
    }

    [Fact]
    public void FindUserById_ShouldReturnUserByValidId()
    {
        // Arrange
        var UserLocalRepository = new UserLocalRepository();
      
        var testUser = new User { Id = Guid.NewGuid(), Name = "Test User" };
        UserLocalRepository.Users.Add(testUser);
 

        // Act
        var foundUser = UserLocalRepository.FindUserById(testUser.Id);


        // Assert
        Assert.NotNull(foundUser);
        Assert.Equal(testUser.Id, foundUser.Id);
        Assert.Equal(testUser.Name, foundUser.Name);
    }

    [Fact]
    public void FindUserById_ShouldThrowExceptionForInvalidId()
    {
        // Arrange
        var UserLocalRepository = new UserLocalRepository();
      

        // Act & Assert
        Assert.Throws<Exception>(() => UserLocalRepository.FindUserById(Guid.NewGuid()));

    }

    [Fact]
    public void GetUsers_ShouldReturnAllUsers()
    {
        // Arrange
        var UserLocalRepository = new UserLocalRepository();
      
        var testUser1 = new User { Id = Guid.NewGuid(), Name = "User 1" };
        var testUser2 = new User { Id = Guid.NewGuid(), Name = "User 2" };
        UserLocalRepository.Users.Add(testUser1);
        UserLocalRepository.Users.Add(testUser2);


        // Act
        var users = UserLocalRepository.GetUsers();


        // Assert
        Assert.NotNull(users);
        Assert.Equal(2, users.Count);
        Assert.Contains(testUser1, users);
        Assert.Contains(testUser2, users);
    }

    [Fact]
    public void FindUserById_ShouldThrowExceptionForNonExistentId()
    {
        // Arrange
        var UserLocalRepository = new UserLocalRepository();
 
        // Act & Assert
        Assert.Throws<Exception>(() => UserLocalRepository.FindUserById(Guid.NewGuid()));
       
    }
}
```

**Замечание**: если Visual Code перестанет обнаруживать ошибки в коде, то нажмите F1 и выполните команду в строке ```.NET: Restart Language Server```. Проверьте также наличие расширения C#.

- запуск всех тестов ```dotnet test```
- просмотр все доступных тестов ```dotnet test --list-tests```
- запуск конкретного списка по фильтру ```dotnet test --filter "FullyQualifiedName=SampleApp.Tests.UserLocalRepositoryTests.CreateUser_ShouldReturnNewUserWithGeneratedId" ```


# Создание UsersController для управления пользователями

```Csharp

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase

{
    private readonly IUserRepository _repo;
    public UsersController(IUserRepository repo)

    {
       _repo = repo;
    }

    [HttpPost]
    public ActionResult CreateUser(User user){

        return Ok(_repo.CreateUser(user));

    }
    
    [HttpGet]
    public ActionResult GetUser(){
        return Ok(_repo.GetUsers());
    }
    

    [HttpPut]
    public ActionResult UpdateUser(User user){
       return Ok(_repo.EditUser(user, user.Id));
    }


    [HttpGet("{id}")]
    public ActionResult GetUserById(Guid id){
       return Ok(_repo.FindUserById(id));
    }


    [HttpDelete]
    public ActionResult DeleteUser(Guid id){
        return Ok(_repo.DeleteUser(id));
    }

}
```

- запустите API: ```dotnet run --project SampleApp.API``` и в средстве Swagger по адресу ```http://localhost:[port]/swagger/index.html``` и попробуйте выполнить конечную точку для получения всех пользователей.


Вы должны получить ошибку:

```
Unable to resolve service for type 'SampleApp.API.Interfaces.IUserRepository' while attempting to activate 'SampleApp.API.Controllers.UsersController'.
```

Эта ошибка говорит о том, что контроллеру в контруктор требуется реализация интерфейса ```IUserRepository```, которую мы будем получать из контейнера внедрения зависимостей (DI).

# Контейнер внедрения зависимостей (Dependency Injection)

Контроллер ```UsersController``` запрашивает в своем конструкторе 

```Csharp
    private readonly IUserRepository _repo;
    public UsersController(IUserRepository repo)
    {
       _repo = repo;
    }
```

реализацию интерфейса ```IUserRepository```, который ему должен предоставить DI (Dependency Injection) - контейнер внедрения зависимости встроенный во фреймворк ASP Core. Для этого надо зарегистрировать сервис в коллекции сервимов в проекте API.

```Csharp
builder.Services.AddSingleton<IUserRepository, UserLocalRepository>();
```

- запустите проект и проверьте все конечные точки по пути ```http://localhost:[port]/swagger/index.html```



# Validation

## DataAnnotation

При отравке пост запросов надо проверять модель данных на соответствие валидности. Для этого применяются инструменты: встроенное средства проверки ```DataAnnotation``` и пакет ```FluentValidation```.

Атрибут для проверки минимального длины имени
```Csharp
  [MinLength(5,ErrorMessage = "Минимальное длина имени 5")]
  public string Name {get ;set;} = string.Empty;
```

Для создания собственного атрибута валидации DataAnnotation создайте папку ```Validations``` и в ней создайте класс ```UserValidator```

```Csharp
public class MaxLengthAttribute : ValidationAttribute
{
    private readonly int _maxLength;

    public MaxLengthAttribute(int maxLength) : base($"Максимальная длина имени: {maxLength} ")
    {
        _maxLength = maxLength;
    }

    public override bool IsValid(object? value)
    {
        return ((String)value!).Length <= _maxLength;
    }
}
```

Таким образом модель будет выглядеть следующим образом:

```Csharp
public class User
{
    public Guid Id {get ;set;} = Guid.NewGuid();
    
    [MinLength(5,ErrorMessage = "Минимальное длина имени 5")]
    [SampleApp.API.Validations.MaxLength(10)]
    public string Name {get ;set;} = string.Empty;
}
```

## FluentValidation

Установите пакет ```FluentValidation```:

```
dotnet add .\SampleApp.API\ package FluentValidation
```

Установите пакет ```FluentValidation```:

```
dotnet add .\SampleApp.API\ package FluentValidation
```

Создайте в папке ```Validations``` новый класс.

```Csharp
    public class FluentValidator : AbstractValidator<User>
    {
        public FluentValidator()
        {
            RuleFor(u => u.Name).Must(StartsWithCapitalLetter).WithMessage("Имя пользователя должно начинаться с заглавной буквы");
        }
        
        private bool StartsWithCapitalLetter(string username)
        {
            return char.IsUpper(username[0]);
        }
    }
```

Для применения валидатора к конечной точки создания пользователя внесите изменения в код контроллера UsersController: 


```Csharp
        var validator = new FluentValidator();
        var result = validator.Validate(user);
        if(!result.IsValid){
            throw new Exception($"{result.Errors.First().ErrorMessage}");
        }
```


# Postman(Swagger,request.http) для тестирования API

- Способ 1
Протестируйте работу API на примере управления пользователями с помощью встроенного средства Swagger по адресу http://localhost:5290/swagger

- Cпособ 2. Postman

- Способ 3. Запросы .http

Создайте в корнейвой директории папку ```requests``` в которой создайте файл с расширением http. Например, ```getusers.http```


```http
GET http://localhost:5290/User
```

postuser.http
```http
POST http://localhost:5290/User
Content-Type: application/json

{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "Newuser123213131"
}
```

Проверка запросов осуществляется с помощью VS Code.

**Задание 1**: проверьте все методы валидации при отправке POST запроса на создания пользователя во всех средствах тестирования API

**Задание 2**: у пользователя должна быть роль. Создайте модель ```Role```, а также интерфейс, репозиторий, контроллер, валидации, напишите unit-тесты для репозитории RoleRepository.
commit: "Создание RolesController"

**Задание 3**: при запросе post на создание нового ресурса обычно принято отвечать кодом ```201```. Примените метод ```Created``` для возврата ответа типа ```ActionResult```

commit: Реализация статус-кода 201 в методе контроллера для создания пользователя
