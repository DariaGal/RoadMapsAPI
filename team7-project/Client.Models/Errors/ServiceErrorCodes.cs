namespace Client.Models.Errors
{
    /// <summary>
    /// Коды ошибок
    /// </summary>
    public static class ServiceErrorCodes
    {
        /// <summary>
        /// Системная ошибка
        /// </summary>
        public const string System = "system";

        /// <summary>
        /// Не найдено
        /// </summary>
        public const string NotFound = "not-found";

        /// <summary>
        /// Нет доступа
        /// </summary>
        public const string Forbidden = "auth:forbidden";

        /// <summary>
        /// Не авторизован
        /// </summary>
        public const string Unauthorized = "auth:unauthorized";

        /// <summary>
        /// Некорректные учетные данные
        /// </summary>
        public const string InvalidCredentials = "auth:invalid-credentials";

        /// <summary>
        /// Некорректный запрос
        /// </summary>
        public const string BadRequest = "bad-request";

        /// <summary>
        /// Ошибка валидации
        /// </summary>
        public const string ValidationError = "validation:error";
    }
}
