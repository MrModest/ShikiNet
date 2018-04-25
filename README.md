# ShikiNet
Shikimori API implementation for .Net Standart 2.0

Не завершено и вряд ли когда-нибудь будет. 

Вдохновился кодом из [этого](https://github.com/Firely-Pasha/JShikiApi) репозитория. 

Стало интересно переписать Core-класс под C#. Желания же переписывать 86(!) сущностей с Java на C# нет, а парсеры справляются неочень.
Так что это "just for fun". :)

# Авторизация
Перед началом использования методов под авторизованным пользователем необходимо проделать следующие действия:

```c#
//Запоняете данные вашего приложения
//Взять их можно тут: https://shikimori.org/oauth/applications/%n%/edit
//Где %n% необходимо заменить на номер вашего приложения (это не client_id)
Api.ClientId = "<Ваш Client ID>";
Api.ClientSecret = "<Ваш Client Secret>";
Api.RedirectUrl = "<Url, на который будет перенаправлен код авторизации>";

//Получаете ссылку, для отправки запроса кода авторизации
var codeRequestUrl = Api.GetAuthorizationCodeRequestUrl(); 

var authCode = //как-то получаете этот код

//Получение и сохранение данных авторизации в Api.OAuth2Token
await Api.RequestTokenAsync(authCode);

//Далее все методы будут непосредственно обращаться к токену (Api.OAuth2Token.AccessToken)
//Таким образом, чтобы не запрашивать код авторизации при каждом запуске приложения,
//имеет смысл сохранить его значение где-нибудь и восстановить при следующем запуске. 

//Если срок действия токена истёк, то его можно обновить.
//(Если не указывать аргумент явно, то метод будет использовать значение
//из Api.OAuth2Token.RefreshToken)
await Api.RefreshTokenAsync(/*optional*/refreshToken); 

//Можно включить автоматическое обновление токена, по истечении срока.
//(По умолчанию выключено.)
Api.AutoRefreshToken = true
```

# Animes

Пример запроса списка аниме, соответствующих заданным фильтрам:
```c#
IEnumerable<Anime> animes = await Animes.GetByFilterAsync(f =>
{
    f.Page = 1;
    f.Limit = 10;
    f.Seasons.Add(new SeasonYear(Season.SPRING, 2018), true); //включающее значение фильтра
    f.Seasons.Add(new SeasonYear(2017), false); //искючающее значение фильтра
    f.Seasons.Add(new SeasonYear(1990, 2010), true); //включающее значение фильтра
    //также можно использовать несколько включающих/исключающих фильтров следующим образом
    f.Seasons.Include(new SeasonYear(2015), new SeasonYear(2016)); //включили 2015 и 2016 года
    f.Seasons.Exclude(new SeasonYear(1998), new SeasonYear(1999)); //исключили 1998 и 1999 года
    f.Score = 7;
    f.Censored = false;
    f.GenreIds.Include(12, 24, 56); //включить несколько жанров
    f.GenreIds.Exclude(1, 2, 3); //исключить несколько жанров
    f.SearchString = "some anime name";
    f.Order = AnimeOrder.POPULARITY; //сортировка по популярности
});

//Если необходимо преобразовать в List<T>
List<Anime> animeList = new List<Anime>(animes);
```

Следующие фильтры являются типами `FilterDictionary<T>`:
* `Statuses`
* `Seasons`
* `Ids`
* `ExcludeIds`
* `MyLists`
* `GenreIds`
* `Kind`
* `Durations`
* `Ratings`
* `StudioIds`
* `PublisherIds`

и могут использовать слующие способы добавления значения:
* `Add(T filter, bool include)` - включает или исключает (в зависимости от значение `include`) значение фильтра
* `Include(params T[] filters)` - включает все значения, перечисленные в аргументах
* `Exclude(params T[] filters)` - исключает все значения, перечисленные в аргументах
