# Настройка безопасности приложения

## Валидация модели

Переделайте форму регистрации следующим образом:

```html
 <form method="post" class="form-signin" role="form">
     
     <div asp-validation-summary="All"></div>

     <span asp-validation-for="Input.Name"></span>
     <input type="text" asp-for="Input.Name" class="form-control" placeholder="Имя" autofocus>
     
     <span asp-validation-for="Input.Email"></span>
     <input type="email" asp-for="Input.Email" class="form-control" placeholder="Почта" autofocus>
     
     <span asp-validation-for="Input.Password"></span>
     <input type="password" asp-for="Input.Password" class="form-control" placeholder="Пароль" required autofocus>
     
     <span asp-validation-for="Input.PasswordConfirmation"></span>
     <input type="password" asp-for="Input.PasswordConfirmation" class="form-control" placeholder="Повторите пароль" required>
     
     <button class="btn btn-lg btn-primary btn-block" type="submit">
         Создание пользователя
     </button>
 </form>
```

Код обработчика формы регистрации OnPost также изменится:

```Csharp
public IActionResult OnPost(User Input) // либо через свойство, либо через параметр
{

    //ModelState.AddModelError(string.Empty,"Cannot convert currency to itself");

    if (!ModelState.IsValid)
    {
        _f.Flash(Types.Danger, $"Валидация не пройдена!", dismissable: true);
        return Page();
    }

    if (!Input.IsPasswordConfirmation())
    {
        _f.Flash(Types.Warning, $"Пароли должны совпадать!", dismissable: true);
        return Page();
    }

    try
    {
        _db.Users.Add(Input);
        _db.SaveChanges();

        _f.Flash(Types.Success, $"Пользователь {Input.Name} зарегистрирован!", dismissable: true);
        return RedirectToPage("./Auth");

    }
    catch (Exception ex)
    {
        _logger.LogError($"{ex.Message}");
        return RedirectToPage("./Sign");
    }
}
```

## Шифрование пароля в базе данных

В папке Application создайте метод расширения в классе ```UserExtension```:

```Csharp
public static string HashPassword(this Models.User user,string password)
{
    // Реализация хеширования пароля с использованием MD5
    using (MD5 md5 = MD5.Create())
    {
        byte[] inputBytes = Encoding.ASCII.GetBytes(password);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        // Конвертируем байты обратно в строку
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hashBytes.Length; i++)
        {
            sb.Append(hashBytes[i].ToString("X2"));
        }
        return sb.ToString();
    }
}
```

Теперь вы можете шифровать пароль следующим образом при регистрации перез сохранением в базу данных:

```Chsarp
 Input.Password = Input.HashPassword(Input.Password);
 Input.PasswordConfirmation = Input.HashPassword(Input.PasswordConfirmation);
```



