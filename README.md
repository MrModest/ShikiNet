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
