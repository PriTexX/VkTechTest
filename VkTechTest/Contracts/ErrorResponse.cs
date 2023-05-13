namespace VkTechTest.Contracts;

/// <summary>
/// Ответ с ошибкой
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Возникшая проблема
    /// </summary>
    public string Error { get; }
    public ErrorResponse(string msg)
    {
        Error = msg;
    }
}