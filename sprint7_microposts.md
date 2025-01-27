# Микросообщения пользователей

У нас пока не реализована возможность создания микросообщений с помощью веб-браузера – она появится позже, но это не мешает нам
реализовать их отображение. Следуя по стопам Twitter, мы будем отображать микросообщения не на отдельной странице с их
списком, а непосредственно на странице профиля пользователя. Начнем с довольно простых шаблонов добавления микросообщений в профиль
пользователя. 

# Содание модели микросообщения

- создать модель ```Micropost```
- создать навигационные свойства
- отредактировать миграцию для приминения правил каскадного удаления всех сообщений при удалении пользователя
- добавить новую коллекцию данных в ```SampleappContext```



# Добавление микросообщений на страницу профиля пользователя

Profile.cshtml

```Csharp
<div class="row">

    <div class="col-md-4">
        <h2>
            @Model.ProfileUser.Name
            Cообщения @Model.ProfileUser.Microposts.Count()
        </h2>
    </div>

    <div class="col-md-8">

        <ol class="microposts">
            @foreach (var post in Model.ProfileUser.Microposts)
            {
                <p>
                    <b>@post.User.Name: </b> @post.Content
                    <a asp-page="Profile" asp-page-handler="Delete" asp-route-id="@Model.ProfileUser.Id" asp-route-messageid="@post.Id" class="btn btn-danger">Удалить</a>
                </p>
            }
        </ol>
    </div>


</div>
```

В связанной модели обработчик GET запроса будет представляться следующим образом:

```Csharp
        public void OnGet([FromRoute] int id)
        {
            var sessionId = HttpContext.Session.GetString("SampleSession");
            ProfileUser = _db.Users.Where(u => u.Id == id).Include(u => u.Microposts).FirstOrDefault();
            CurrentUser = _db.Users.Where(u => u.Id == Convert.ToInt32(sessionId)).FirstOrDefault();
        }
```

Стили css для микросообщений:

```css
.microposts {
    list-style: none;
    padding: 0;

    li {
        padding: 10px 0;
        border-top: 1px solid #e8e8e8;
    }

    .user {
        margin-top: 5em;
        padding-top: 0;
    }

    .content {
        display: block;
        margin-left: 60px;

        img {
            display: block;
            padding: 5px 0;
        }
    }

    .timestamp {
        color: gray;
        display: block;
        margin-left: 60px;
    }

    .gravatar {
        float: left;
        margin-right: 10px;
        margin-top: 5px;
    }
}

aside {
    textarea {
        height: 100px;
        margin-bottom: 5px;
    }
}

span.picture {
    margin-top: 10px;

    input {
        border: 0;
    }
}
```

# Создание микросообщениями

Мы закончили работу над моделью данных и шаблонами отображения
микросообщений, теперь обратим наше внимание на веб-интерфейс для их
создания. В этом разделе мы также увидим первый намек на поток (ленту)
сообщений. Наконец, так же как с пользователями, мы сделаем возможным
удаление микросообщений через веб-интерфейс. 

Добавление формы для создания микросообщений на главной странице. При этом форма должна появляться, когда пользователь авторизирован. 

**Задание**: реализуйте отображение формы при заданном условии. Используйте для этого условие.

Далее, если пользователь залогинился покажите ему форму c левой стороны. Вставте форму в контейнер ```class="col-md-4"```

```html
        <aside>
            <section class="micropost_form">
                <form action="" method="post" class="form-signin" role="form">
                    <textarea rows="4" cols="50" type="text" name="message" required autofocus></textarea>
                    <button class="btn btn-lg btn-primary btn-block" type="submit" name="submit">Отправить</button>
                </form>
            </section>
        </aside>
```

Код обработчика формы:

```Csharp
public async Task<IActionResult> OnPostAsync(string message)
        {
            var sessionId = HttpContext.Session.GetString("SampleSession");
            CurrentUser = await _db.Users.Include(u => u.Microposts).FirstOrDefaultAsync(m => m.Id == Convert.ToInt32(sessionId));

            if (!string.IsNullOrWhiteSpace(message))
            {
                var m = new Micropost()
                {
                    Content = message,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    UserId = CurrentUser.Id
                };

                try
                {
                    _db.Microposts.Add(m);
                    _db.SaveChanges();
                    _f.Flash(Types.Success, $"Tweet!", dismissable: true);
                    return RedirectToPage();
                }
                catch (Exception ex)
                {
                    _log.Log(LogLevel.Error, $"Ошибка создания сообщения: {ex.InnerException.Message}");
                }


                return Page();
            }
            else
            {
                return Page();
            }

        }
```


# Удаление микросообщений

Последнее, что осталось добавить возможность удаления сообщений. Так же как в случае с удалением учетных записей мы реализуем это добавлением ссылок «delete» (удалить). Но, в отличие от учетных записей, удаление которых было доступно только администраторам, микросообщения могут удаляться лишь их авторами. Ниже представлен код для кнопки удаления:

```Csharp
<a asp-page="Index" asp-page-handler="Delete" asp-route-id="@post.Id" class="btn btn-info">Удалить</a>
```

Обработчик по кнопке удаления:

```Csharp
        public async Task<IActionResult> OnGetDeleteAsync([FromQuery] int messageid)
        {
            var sessionId = HttpContext.Session.GetString("SampleSession");
            CurrentUser = await _db.Users.Include(u => u.Microposts).FirstOrDefaultAsync(m => m.Id == Convert.ToInt32(sessionId));

            try
            {
                Micropost m = _db.Microposts.Find(messageid);
                _db.Microposts.Remove(m);
                _db.SaveChanges();
                _log.Log(LogLevel.Error, $"Удалено сообщение \"{m.Content}\" пользователя {CurrentUser.Name}!");
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _log.Log(LogLevel.Error, $"Ошибка удаления сообщения: {ex.InnerException}");
                _log.Log(LogLevel.Error, $"Модель привязки из маршрута: {messageid}");
            }

            return Page();

        }
```



# Прото-лента

Добавление сообщений пользователя на главную страницу в ленту сообщений:

```html
<div class="row">

            <div class="col-md-4">
                <h2>
                    @Model.CurrentUser.Name
                    <p>
                        Cообщения @Model.CurrentUser.Microposts.Count()
                    </p
                </h2>

                <aside>
                    <section class="micropost_form">
                        <form action="" method="post" class="form-signin" role="form">
                            <textarea rows="4" cols="50" type="text" name="message" required autofocus></textarea>
                            <button class="btn btn-lg btn-primary btn-block" type="submit" name="submit">Отправить</button>
                        </form>
                    </section>
                </aside>

            </div>

            <div class="col-md-8">

                <ol class="microposts">
                    @foreach (var post in Model.CurrentUser.Microposts)
                    {
                        <p>
                            <b>@post.User.Name: </b> @post.Content
                            <a asp-page="Index" asp-page-handler="Delete" asp-route-id="@Model.CurrentUser.Id" asp-route-messageid="@post.Id" class="btn btn-danger">Удалить</a>
                        </p>
                    }
                </ol>
            </div>
        </div>
```

Код обработчика модели метода OnGet:

```Csharp
        public void OnGet()
        {
            var sessionId = HttpContext.Session.GetString("SampleSession");
            CurrentUser = _db.Users.Where(u => u.Id == Convert.ToInt32(sessionId)).Include(u => u.Microposts).FirstOrDefault();
        }
```

Код обработчика модели метода OnPost на странице Index:

```Csharp
public async Task<IActionResult> OnPostAsync(string message)
        {
            var sessionId = HttpContext.Session.GetString("SampleSession");
            CurrentUser = await _db.Users.Include(u => u.Microposts).FirstOrDefaultAsync(m => m.Id == Convert.ToInt32(sessionId));

            if (!string.IsNullOrWhiteSpace(message))
            {
                var m = new Micropost()
                {
                    Content = message,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    UserId = CurrentUser.Id,
                    //User = this.User
                };

                try
                {
                    _db.Microposts.Add(m);
                    _db.SaveChanges();
                    _f.Flash(Types.Success, $"Tweet!", dismissable: true);
                    return RedirectToPage();
                }
                catch (Exception ex)
                {
                    _log.Log(LogLevel.Error, $"Ошибка создания сообщения: {ex.InnerException.Message}");
                }


                return Page();
            }
            else
            {
                return Page();
            }

        }
```


Обработчик метода на удаление сообщений пользователя:

```Csharp
public async Task<IActionResult> OnGetDeleteAsync([FromQuery] int messageid)
        {
            var sessionId = HttpContext.Session.GetString("SampleSession");
            CurrentUser = await _db.Users.Include(u => u.Microposts).FirstOrDefaultAsync(m => m.Id == Convert.ToInt32(sessionId));

            try
            {
                Micropost m = _db.Microposts.Find(messageid);
                _db.Microposts.Remove(m);
                _db.SaveChanges();
                _log.Log(LogLevel.Error, $"Удалено сообщение \"{m.Content}\" пользователя {CurrentUser.Name}!");
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _log.Log(LogLevel.Error, $"Ошибка удаления сообщения: {ex.InnerException}");
                _log.Log(LogLevel.Error, $"Модель привязки из маршрута: {messageid}");
            }

            return Page();

        }
```




**Задание**:

- На главной странице должна появляться лента сообщений, если пользователь залогинен
- Список пользователей должен отображаться только, если пользователь залогинен.
- Реализуйте возможность удаления пользователей только, если пользователь сам админ.
