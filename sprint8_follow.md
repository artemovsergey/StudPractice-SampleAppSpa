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

**Замечание**: ```CurrentUser```, ```ProfileUser``` и ```IsFollow``` это свойства класса привязанной модели страницы ```Profile```.

Метод ```OnGet``` будет выглядеть следующим образом:

```Csharp
        public async Task<IActionResult> OnGetAsync([FromRoute]int? id)
        {
            var sessionId = HttpContext.Session.GetString("SampleSession");
            ProfileUser = await _db.Users.Include(u => u.Microposts).FirstOrDefaultAsync(m => m.Id == id) as User;   
            CurrentUser = await _db.Users.Include(u => u.Microposts).FirstOrDefaultAsync(m => m.Id.ToString() == sessionId) as User;

            // если текущий пользователь подписан на профиль пользователя
            var result = _db.Relations.Where(r => r.Follower == CurrentUser  && r.Followed == ProfileUser).FirstOrDefault();

            if (result != null) 
            {
              IsFollow = true;
            }
            else 
            {
              IsFollow =false;
            }

            return Page();
        }
```

## Подписка на пользователя

Далее показан код обраюотчика OnPost для страницы ```Profile```:

```Csharp
public async Task<IActionResult> OnPostAsync([FromRoute] int? id)
{
    var sessionId = HttpContext.Session.GetString("SampleSession");
    ProfileUser = await _context.Users.Include(u => u.Microposts).FirstOrDefaultAsync(m => m.Id == id) as User;
    CurrentUser = await _context.Users.Include(u => u.Microposts).FirstOrDefaultAsync(m => m.Id.ToString() == sessionId) as User;

    // если текущий пользователь подписан на профиль пользователя
    var result = _context.Relations.Where(r => r.Follower == CurrentUser && r.Followed == ProfileUser).FirstOrDefault();

    if (result != null)
    {
        IsFollow = true;
    }
    else
    {
        IsFollow = false;
    }

    if (IsFollow == false)
    {
        try
        {
            _context.Relations.Add(new Relation() { FollowerId = CurrentUser.Id, FollowedId = ProfileUser.Id });
            _context.SaveChanges();
            _f.Flash(Types.Success, $"Пользователь {CurrentUser.Name} подписался на {ProfileUser.Name}!", dismissable: true);
        }
        catch(Exception ex)
        {
            _f.Flash(Types.Success, $"{ex.InnerException.Message}", dismissable: true);
        }
    }
    else
    {

        try
        {
            var result2 = _context.Relations.Where(r => r.Follower == CurrentUser && r.Followed == ProfileUser).FirstOrDefault();
            _context.Relations.Remove(result2);
            _context.SaveChanges();
            _f.Flash(Types.Warning, $"Пользователь {CurrentUser.Name} отписался от {ProfileUser.Name}!", dismissable: true);
        }
        catch (Exception ex)
        {
            _f.Flash(Types.Success, $"{ex.Message}", dismissable: true);
        }

       
    }

    return RedirectToPage();
}
```

# Просмотр подписчиков и подписок

Создайте две новые страницы ```Followers``` и ```Followeds```.

На странице ```Profile``` ниже кнопки для подписки/отписки на пользователя разместите дополнительные кнопки, которые будет вести нас на тех, кого читает пользоваетль и тех, кто чиатет пользователя.

```
 <a class="btn btn-primary" asp-page="Followers" asp-route-id=@Model.ProfileUser.Id>Подписчики</a>
 <a class="btn btn-primary" asp-page="Followeds" asp-route-id=@Model.ProfileUser.Id>Подписки</a>
```

## Followeds

Страница Followeds содержит следующий вид:

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

Полный класс для модели ```Followeds``` показан ниже:

```Csharp
public class FollowedsModel : PageModel
{
    #region 
    private readonly SampleContext _context;
    private readonly ILogger<FollowedsModel> _logger;
    private readonly IFlasher _f;
    public FollowedsModel(SampleContext context, ILogger<FollowedsModel> logger, IFlasher f)
    {
        _context = context;
        _logger = logger;
        _f = f;
    }
    #endregion

    public User ProfileUser { get; set; }
    public IEnumerable<User> Followeds { get; set; }

    public void OnGet(int id)
    {
        ProfileUser = _context.Users.Include(u => u.RelationFollowers)
                                    .ThenInclude(r => r.Followed)
                                    .Where(u => u.Id == id)
                                    .FirstOrDefault();

        Followeds = ProfileUser.RelationFollowers.Select(item => item.Followed).ToList();

    }
}
```

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

```Csharp
public class FollowersModel : PageModel
{
    #region 
    private readonly SampleContext _context;
    private readonly ILogger<FollowersModel> _logger;
    private readonly IFlasher _f;
    public FollowersModel(SampleContext context, ILogger<FollowersModel> logger, IFlasher f)
    {
        _context = context;
        _logger = logger;
        _f = f;
    }
    #endregion

    public User ProfileUser { get; set; }
    public IEnumerable<User> Followers { get; set; }

    public void OnGet(int id)
    {
        ProfileUser = _context.Users.Include(u => u.RelationFolloweds)
                                    .ThenInclude(r => r.Follower)
                                    .Where(u => u.Id == id)
                                    .FirstOrDefault();

        Followers = ProfileUser.RelationFolloweds.Select(item => item.Follower).ToList();

    }
}
```

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





