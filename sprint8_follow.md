# Подписки и подписчики

## Проверка подписки

Для реализации функциональности следования за пользователем и читать его сообщения нам необходима создать кнопку с помощью которой можно будет осуществлять подписку на сообщения пользователя. На странице ```Profile``` ниже имени пользователя разместите форму:

```
        <form method="post">

            @{
                if(Model.CurrentUser != Model.ProfileUser)
                {
                    if (!Model.IsFollow)
                    {
                        <button class="btn btn-success" type="submit">Подписаться</button>
                    }
                    else
                    {
                        <button class="btn btn-danger" type="submit">Отписаться</button>
                    }
                }

             }

        </form>
```

При этом форма будет показываться только в том случае, если текущий пользователь и залогиненный пользователь не совпадают. Далее, если пользователь уже подписан, то кнопка будет "Отписаться" и наоборот.


# Просмотр подписчиков и подписок

Создайте две новые страницы ```Followers``` и ```Followeds```.

На странице ```Profile``` ниже кнопки для подписки/отписки на пользователя разместите дополнительные кнопки, которые будет вести нас на тех, кого читает пользоваетль и тех, кто чиатет пользователя.

```
 <a class="btn btn-primary" asp-page="Followers" asp-route-id=@Model.ProfileUser.Id>Подписчики</a>
 <a class="btn btn-primary" asp-page="Followeds" asp-route-id=@Model.ProfileUser.Id>Подписки</a>
```

## Followeds

Страница Followeds содержит следующий вид:


## Followers

Страница ```Followers```:

```
<h1> Подписки @Model.ProfileUser.Name </h1>

<ul>
    @{
        foreach (var user in Model.Followeds)
        {
            <li> <a class="btn btn-success" asp-page="Profile" asp-route-id="@user.Id">@user.Name</a> </li>
        }
    }
</ul>
```

Полный код класса модели для страницы ```Followes```:


# Триггер для удаления пользователей

```sql
CREATE TRIGGER dbo.DeleteCascadeRelations ON dbo.Users
INSTEAD OF DELETE 
AS
BEGIN
   DELETE FROM dbo.Relations WHERE FollowerId IN (SELECT Id FROM deleted) OR FollowedId IN (SELECT Id FROM deleted);
   DELETE FROM dbo.Users WHERE Id IN (SELECT Id FROM deleted);
END
```





