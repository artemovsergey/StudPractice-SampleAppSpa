# Начало работы

- откройте командную строку, создайте папку SampleApp и перейдите в нее
- выполните команду ```code .``` для открытия проекта SampleApp в Visual Code

# Подготовка редактора Visual Code
- File - Autosave

**Замечание**: при работе со средой .NET можно применять Visual Studio. Выбор редактора для курса - это предпочтение преподавателя.

# Использование dotnet cli для построения архитектуры приложения

- создайте папку ```SampleApp``` и перейдите в нее в командной строке.
- посмотрите с помощью команды ```dotnet new list``` список доступных проектов и создайте проект ```webapi``` с именем ```SampleApp.API``` командой:  ```dotnet new webapi -o SampleApp.API```

**Замечание**: в приложении будет использоваться версия .net 7, в версии 8 или 9 данная команда по умолчанию не подключает функциональность для работы с контроллерами API, поэтому для включения контроллеров в версии надо использовать флаг ```-controllers```. Чтобы проверить версию наберите команду: ```dotnet --version```.

- добавьте файл решения, находясь в папке рабочей директории, командой ```dotnet new sln```

- добавьте в решение проект API - ```dotnet sln add SampleApp.API```

**Замечание**: можно добавить все проекты в решение одной командой ```dotnet sln add (ls -r **/**.csproj)```. Эта команда работает в Powershell.

- добавьте файл ```.gitignore``` (на уровне проекта API) - ```dotnet new gitignore```

- создайте файл ```.gitignore``` (на уровне рабочей корневой директории) и поместите в ```.gitignore``` следующие настройки:

```
## git rm -r --cached
.vscode
```

Для проверки работоспособности приложения запустите API: ```dotnet run --project SampleApp.API```.
**Замечание**: для горячей перезагрузки сервера примените команду ```dotnet watch run --project SampleApp.API```.


У вас по конечной точке http://localhost:5290/weatherforecast должен выводится результат в формате json.

**Примечание**: номер порта может быть другим.

- добавьте в решение файл ```readme.md```

- фиксация изменений в git с сообщением: "Создание начального проекта API" 

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

Создайте папку ```Repositories``` и поместите там следующий класс ```UsersMemoryRepository```, который будет имплементировать (реализовывать) интерфейс ```IUserRepository```.

```Csharp
public class UsersMemoryRepository : IUserRepository
{
    public IList<User> Users { get; set; } = new List<User>();
  
    public User CreateUser(User user)
    {
        user.Id = 1;
        Users.Add(user);
        return user;
    }

    public bool DeleteUser(int id)
    {
        var result = FindUserById(id);
        Users.Remove(result);
        return true;
    }

    public User EditUser(User user, int id)
    {
        var result = FindUserById(id);
        result.Name = user.Name;
        return result;
    }

    public User FindUserById(int id)
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

**Примечание**: очистите папку ```Controllers``` от файла WeatherForecastController и файл модели WeatherForecast. 


# Создание UsersController для управления пользователями

- создайте папку Controllers

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
    public ActionResult GetUserById(int id){
       return Ok(_repo.FindUserById(id));
    }

    [HttpDelete]
    public ActionResult DeleteUser(int id){
        return Ok(_repo.DeleteUser(id));
    }

}
```

- запустите API: ```dotnet run --project SampleApp.API``` и в средстве Swagger по адресу ```http://localhost:[port]/swagger/index.html``` попробуйте выполнить конечную точку для получения всех пользователей.

**Замечание**: для .net9 по умолчанию swagger отсутствует, поэтому надо его подключить пакетом ```Swashbuckle.AspNetCore``` и настроить в Program.cs:

```Csharp
builder.Services.AddSwaggerGen();
//...
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
```

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

реализацию интерфейса ```IUserRepository```, который ему должен предоставить DI (Dependency Injection) - контейнер внедрения зависимости встроенный во фреймворк ASP Core. Для этого надо зарегистрировать сервис в коллекции сервиcов в проекте API.

```Csharp
builder.Services.AddSingleton<IUserRepository, UsersMemoryRepository>();
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

Для создания собственного атрибута валидации DataAnnotation создайте папку ```Validations``` и в ней создайте класс ```UserNameMaxLengthValidation```

```Csharp
public class UserNameMaxLengthValidation : ValidationAttribute
{
    private readonly int _maxLength;

    public UserNameMaxLengthValidation(int maxLength) : base($"Максимальная длина имени: {maxLength} ")
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
    public int Id {get ;set;}
    
    [MinLength(5,ErrorMessage = "Минимальное длина имени 5")]
    [SampleApp.API.Validations.UserNameMaxLengthValidation(10)]
    public string Name {get ;set;} = string.Empty;
}
```

## FluentValidation

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

Для применения валидатора к конечной точки создания пользователя внесите изменения в код контроллера ```UsersController```: 


```Csharp
        var validator = new FluentValidator();
        var result = validator.Validate(user);
        if(!result.IsValid){
            throw new Exception($"{result.Errors.First().ErrorMessage}");
        }
```


# Инструменты для тестирования API

1. Протестируйте работу API на примере управления пользователями с помощью встроенного средства Swagger по адресу http://localhost:5137/swagger

2. Postman

3. Запросы .http

Создайте в корнейвой директории папку ```requests``` в которой создайте файл с расширением http. Например, ```getusers.http```


```
@api = http://localhost:5137
GET {{api}}/Users
```

postuser.http
```
@api = http://localhost:5137
POST {{api}}/Users
Content-Type:  application/json

{
  "id": 0,
  "name": "String" 
}
```

Проверка запросов осуществляется с помощью VS Code.

**Задание 1**: проверьте все методы валидации при отправке POST запроса на создания пользователя во всех средствах тестирования API

**Задание 2**: у пользователя должна быть роль. Создайте модель ```Role```, а также интерфейс, репозиторий, контроллер, валидации.

Фиксация изменений в git: "Создание RolesController"

**Задание 3**: при запросе post на создание нового ресурса обычно принято отвечать кодом ```201```. Примените метод ```Created``` для возврата ответа типа ```ActionResult```

Фиксация изменений в git: Реализация статус-кода 201 в методе контроллера для создания пользователя

# Rebase sprint1 в master

- зафиксируйте sprint1
- перейдите в master
- выполните команду git rebase sprint1


# Рефакторинг

- установите расширение Material Icons для удобства работы и включите с помощью команды ```>material icon``` по F1
- в настройках settings => exclude: внесите шаблоны для ```bin``` и ```obj```


# Возможные ошибки и их решения
## Ошибка скачивания пакетов с nuget.org

- dotnet nuget locals all --clear
- dotnet dev-certs https --check --trust

 или удалить nuget.config и перезагрузить обязательно