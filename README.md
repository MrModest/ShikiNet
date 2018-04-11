# ShikiNet
Shikimori API implementation for .Net Standart 2.0

Не завершено и вряд ли когда-нибудь будет. 

Вдохновился кодом из [этого](https://github.com/Firely-Pasha/JShikiApi) репозитория. 

Стало интересно переписать Core-класс под C#. Желания же переписывать 86(!) сущностей с Java на C# нет, а парсеры справляются неочень.
Так что это "just for fun". :)

Переписан только класс `Shikimori` (у меня он называется [**Api**](https://github.com/MrModest/ShikiNet/blob/master/ShikiNet/Core/Api.cs)), ну и сущность `AccessToken` (у меня она называется [**OAuth2Token**](https://github.com/MrModest/ShikiNet/blob/master/ShikiNet/Entity/OAuth2Token.cs)), которая была нужна для его работы.
