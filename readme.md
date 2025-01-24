# Стек проекта

node  - 22.12.0
ng - 18.2.8
dotnet - 8.0.400
npm - 11

# План

- создаем в консоли решение и проекты, dotnet cli
- настройка vs code
- интерфейсы, репозитории, контроллеры, di
- подключение базы данных, контекст, миграции, scaffold
- рефакторинг: базовая модель, контроллер
- подключение postgres
- crud на user
- переделываем на асинхронность
- swagger, postman
- git, gitignore, branch, switch
- добавляем dto, automapper
- добавление валидации FluentValidation (модель ответа, middleware)
- проверяем последовательно работу движение потока с помощью debugger

- добавлем новую модель role
- формируем generic repository
- связываем модели
- теперь можно делать сортировка, фильтрацию, поиск
- реализация пагинации

Задание: в отдельной ветке реализовать паттерн спецификация (для включения запросов и связанных данных)

- реализуем механизм генерации и добавления в базу тестовых данных
- настройка jwt

Задание 1: после реализации сrud на user создаем отдельные ветки и тестим
механизм minimalapi, fastapi
Задание 2: в отдельной ветке тестим новую orm dapper, sqlkata, raw sql
Задание 3: поднимаем докер
Задание 4: развертываем в production, через ci/cd
Задание 5: в отдельной ветке реализуем чистую архитектуру
Задание 6: в отдельной ветке реализуем CQRS через Mediatr
Задание 7: реализовать интерфейс только на чистом css


# Заметки

- по ангуляр: вместо конструктора использовать inject, при subscribe применить next, error, complete
- ангуляр: заменяем на @if, @for
- ангуляр: возможно посмотреть на ngx-boostrap
- для https на localhost посмотреть на mkcert
- из курса вставить иллюстрации (скриншоты из видео с компьютера,а не с браузера)
- занести appsettings.json в игнор
- возможно вместо или совместно с локальным репозиторием сделать репозиторий для работы с sqlite
- сделать валидацию на уровне базы данных
- добавить фото
- работать с AutoMapper
- использовать Dto
- паттерн спецификация (для включения запросов и связанных данных)
- обобщенные репозитории

- в ангуляр передавать заголовки headers (плавно перейти к перехватчикам)
- angular: ng-gallery
- ng g enviroment




# Ресурсы для изучения Angular

- https://www.angularjswiki.com/

# Angular

- структура компонента
- привязка данных (к атрибуту и свойству, классу, стилю)
- условные кострукции @if, @switch
- циклы @for
    - - $count: количество элементов коллекции
    - - $first: является ли текущий элемент первым в коллекции
    - - $last: является ли текущий элемент последним в коллекции
    - - $even: является ли индекс текущего элемента четным
    - - $odd: является ли индекс текущего элемента нечетным
- директивы (ngStyle, ngClass, атрибутивные и структурные директивы, ngIf и ng-template, ngFor, ngSwitch)

- сервисы
    - - предоставление данных приложению. Сервис может сам хранить данные в памяти, либо для получения данных может обращаться к какому-нибудь источнику данных, например, к серверу.

    - - сервис может представлять канал взаимодействия между отдельными компонентами приложения

    - - сервис может инкапсулировать бизнес-логику, различные вычислительные задачи, задачи по логгированию, которые лучше выносить из компонентов. Тем самым код компонентов будет сосредоточен непосредственно на работе с представлением. Кроме того, тем самым мы также можем решить проблему повторения кода, если нам потребуется выполнить одну и ту же задачу в разных компонентах и классах

- формы



# Ресурсы

- https://json-generator.com/


# Repository

```Csharp
public async Task<bool> SaveAllAsync(){
    return await db.SaveChangesAsync() > 0
}

public async Task Update(User user){
    return await db.Entry(user).State = EntityState.Modified
}
```

# AutoMapper with Repository

```Csharp
return await _db.Users.Where(u => u.name = name)
                      .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                      .SingleOrDefaultAsync();
```


# .NET

# Presentation
- handing interactions with the outside world
- presenting or displayind data
- translating data
- managing ui and framework-related elements
- manipulating the application layer

# Application
- Executing the application's use cases

# Infrastructure
- Interacting with the persistence solution
- Interacting with other services (web client, message broker)
- Interacting with the underlying machine (system clock, files)
- Identity Concern

# Domain
- defining domain models
- defining domain errors
- executing business logic
- enforcing business rules

# Подходы по работе с ошибками
- Exception
- Result Pattern
- Middleware 


# .NET
+ есть способ централизованно хранить пакеты всего решения

**CQS** - более строкий подход к операциям чтения и записи ( если метод изменяет состояние, то он должен возвращать void)

На основе use-case по функция делать команды и запросы

- можно связать бекенд через домен api.backend.ru



Вопросы по бекенд:
1. Можно ли подключить Serilog один раз для всех проектов?
2. Попробовать реализовать разные типы аутентификации
3. Как работать с Identity и ролями

# Хранение фото
https://cloudinary.com


# Sprint 1 Introduction
Реализовать базовую функциональность API и иметь начальное представление о:
1. Использование dotnet CLI
2. Конечная точка контроллера API
3. Entity Framework
4. Структура проекта API
5. Конфигурация и переменные окружения
6. Git


# Sprint 2 Introduction in Angular
1. Использование Angular CLI
2. Как создать новое приложение Angular
3. Файлы проекта Angular
4. Процесс загрузки Angular
5. Использование клиентской службы Angular HTTP
6. Запуск приложения Angular по HTTPS
7. Как добавлять пакеты с помощью NPM

# Sprint 3 Register and Authentication
Implement basic authentication in our app and have an understanding of:
1. How to store passwords in the Database
2. Using inheritance in C# - DRY
3. Using the C# debugger
4. Using Data Transfer Object (DTOs)
5. Validation
6. JSON Web Tokens (JWT)
7. Using services in C#
8. Middleware
9. Extension methods - DRY

# Sprint 4 Register and Authentication in Angular
Реализуйте в приложении функции входа и регистрации, а также понимание:
1. Создание компонентов с помощью Angular CLI
2. Использование шаблонов форм Angular
3. Использование сервисов Angular
4. Понимание Observables
5. Использование структурных директив Angular для условного отображения элементов на странице
6. Связь компонентов от родительского к дочернему
7. Связь компонентов от дочернего компонента к родительскому

# Sprint 5
Implenet routing in our Angular app and have an understanding of:
1. Angular routing
2. Adding a bootstrap theme
3. Using Angular route guards
4. Using a Shared Module ?

# Sprint 6
Implement global error handling in both the API and the Angular application. Also to have an understanding of:
1. API Middleware
2. Angular Interceptors
3. Troubleshooting exceptions

# Sprint 7
Implement further functionality into our API and gain an understanding of:
1. Entity Framework Relationships
2. EF Conventions
3. Seeding data into the Database
4. The repository pattern
5. Using AutoMapper

# Sprint 8
Implement the components that make up the user interface in our client application and gain an understanding of:
1. Using Typescript types
2. Using the Interceptors to send JWT tokens
3. Using bootstrap for styling
4. Basic css tricks to enhance the look
5. Using a 3rd party photo gallery
6. Using Route params

# Sprint 9
Impelent persistence when updating resources in the API and gaining an understanding of
1. Angular Template forms
2. The CanDeactivate Route Guard
3. The @ViewChild decorator
4. Persisting changes to the API
5. Adding loading indicators to the client app
6. Caching data in Angular services

# Sprint 10
Implenets photo upload functionality in the application and gain an understanding of the followind
1. Photo storage options
2. Adding related entities
3. Using a 3rd party API
4. Using the Debugger (again)
5. Updating and deleting resources
6. What to return when creating resources in a REST based API

# Sprint 11
Implement more advanced forms using Reactive Forms in Angular and understand how to
1. Use Reactive Forms
2. Use Angular Validations for inputs
3. Implements custom validators
4. Implement reusable form controls
5. Working the Date inputs

# Sprint 12 
Implement paging, sorting, filtering and gain an understanding of the following
1. How to implement pagination on the API & client
2. Deferred Execution using IQueryable
3. How to implement filtering on the API & client
4. How to implement sorting on the API & client
5. Using Action Filters
6. Adding a TimeAgo pipe
7. Implement caching in the client for paginated resources


# Sprint 13 
Implemets the 'like user' functionality and gain an understanding of the following
1. Many to many relationships
2. Configuring entities in the DbContext
# Sprint 14
Implement the Messaging functionality and gain an understanding of the following
1. More many to many relationss
2. Using query params in Angular
3. Using Route resolvers in Angular


# Sprint 15
Refactor our code to use ASP.NET Identity and gain an understanding of the following
1. Using .NET Identity
2. Role managment
3. Policy based autorisation ```UserManager<T>```, ```SignInManager<T>```,```RoleManager<T>```

# Sprint 16
Implement SignalR into our application and understand how to
1. Use and set up SignalR on both the API and the client
2. Implement online presence
3. Implement live chat between users

# Sprint 17
Implement the Unit of work pattern and gain an understanding of the following
1. The Unit of Work pattern
2. Optimising queries to the DB
3. Adding a confirm dialog service
4. Finishing touches

# Sprint 18
Actually publish the app and gain an understanding of how to:
1. Prepare the app for publiccation
2. What to consider before publishing
3. Switching DBs
4. Serving static content from the API server
5. Publishing to Heroku (or analog)