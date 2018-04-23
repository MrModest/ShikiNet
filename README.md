# ShikiNet
Shikimori API implementation for .Net Standart 2.0

Не завершено и вряд ли когда-нибудь будет. 

Вдохновился кодом из [этого](https://github.com/Firely-Pasha/JShikiApi) репозитория. 

Стало интересно переписать Core-класс под C#. Желания же переписывать 86(!) сущностей с Java на C# нет, а парсеры справляются неочень.
Так что это "just for fun". :)

Переписаны только несколько классов.

* Логика: <br />
[**Shikimori**](https://github.com/Firely-Pasha/JShikiApi/blob/master/src/main/java/net/pagala/JShikiApi/Core/Shikimori.java) (у меня он называется [**Api**](https://github.com/MrModest/ShikiNet/blob/master/ShikiNet/Core/Api.cs)), <br />
[**Animes**](https://github.com/Firely-Pasha/JShikiApi/blob/master/src/main/java/net/pagala/JShikiApi/Core/Animes.java) (частично)

* Сущности: <br />
(Если в списке нет класса, но он есть в папке Entity, то он, скорее всего, пустой)<br />
[**OAuthToken**](https://github.com/Firely-Pasha/JShikiApi/blob/master/src/main/java/net/pagala/JShikiApi/Items/OAuthToken.java) (у меня она называется [**OAuth2Token**](https://github.com/MrModest/ShikiNet/blob/master/ShikiNet/Entity/OAuth2Token.cs))

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
