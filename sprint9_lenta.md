# Лента сообщений

## Работа с временем публикации сообщения

Создайте в папке Application класс Time.cs в который поместите следующий метод:

```Csharp
public static  class Time
{

    public static string HumanView(DateTime yourDate)
    {

        const int SECOND = 1;
        const int MINUTE = 60 * SECOND;
        const int HOUR = 60 * MINUTE;
        const int DAY = 24 * HOUR;
        const int MONTH = 30 * DAY;

        var ts = new TimeSpan(DateTime.UtcNow.Ticks - yourDate.Ticks);
        double delta = Math.Abs(ts.TotalSeconds);

        if (delta < 1 * MINUTE)
            return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

        if (delta < 2 * MINUTE)
            return "a minute ago";

        if (delta < 45 * MINUTE)
            return ts.Minutes + " minutes ago";

        if (delta < 90 * MINUTE)
            return "an hour ago";

        if (delta < 24 * HOUR)
            return ts.Hours + " hours ago";

        if (delta < 48 * HOUR)
            return "yesterday";

        if (delta < 30 * DAY)
            return ts.Days + " days ago";

        if (delta < 12 * MONTH)
        {
            int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
            return months <= 1 ? "one month ago" : months + " months ago";
        }
        else
        {
            int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "one year ago" : years + " years ago";
        }

    }

}
```

Реализация полноценной ленты сообщений пользователя состоит в выводе сообщений самого пользователя и тех людей на кого он подписан. При этом сообщения должны быть отсортированы по дате публикации сообщения.

```
 <div class="col-md-8">
     <h3>
         Cообщения @Model.Messages.Count()
     </h3>
     <ol class="microposts">

         @foreach (var post in Model.Messages.OrderByDescending(p => p.CreatedAt) )
         {
             <p>
                
                @Time.HumanView(post.CreatedAt)
                 <b>@post.User.Name: </b> @post.Content
                 
                  <a asp-page="Index" asp-page-handler="Delete" asp-route-id="@post.Id" class="btn btn-info">Удалить</a>

             </p>
         }

     </ol>
 </div>
```

Также в классе модели страницы ```Index``` добавляются вспомогательные свойства:

```Csharp
        public IEnumerable<User> Followeds { get; set; }
        public List<User> Users { get; set; } = new();
        public List<Micropost> Messages { get; set; } = new();
```

Измененный метод OnGet для страницы ```Index```:

```Csharp
public async Task<IActionResult> OnGetAsync()
{

    sessionId = HttpContext.Session.GetString("SampleSession");

    if (sessionId != null)
    {
        User = await _db.Users.Include(u => u.Microposts)
                              .Include(u => u.RelationFollowers).ThenInclude(r => r.Followed)
                              .FirstOrDefaultAsync(m => m.Id == Convert.ToInt32(sessionId));

        Followeds = User.RelationFollowers.Select(item => item.Followed).ToList();
        
        Users.AddRange(Followeds);
        Users.Add(User);

        foreach (var u in Users)
        {
            _db.Entry(u).Collection(u => u.Microposts).Load();
            Messages.AddRange(u.Microposts);
        }
        return Page();
    }
    else
    {
        return RedirectToPage("Auth");
    }
    
}
```











