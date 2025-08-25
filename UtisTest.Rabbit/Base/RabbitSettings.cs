namespace UtisTestTask.Rabbit.Base;

public class RabbitSettings
{
    public const string ConfigurationSection = "RabbitSettings"; 
    public string Host { get; set; } = "";
    public int Port { get; set; }
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
    public string TaskOverdueQueue { get; set; } = "";
}