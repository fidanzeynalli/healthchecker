using System.Text.Json.Serialization;

public class RegisterDto
{
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }
    [JsonPropertyName("lastName")]
    public string LastName  { get; set; }
    [JsonPropertyName("email")]
    public string Email     { get; set; }
    [JsonPropertyName("password")]
    public string Password  { get; set; }
    [JsonPropertyName("confirmPassword")]
    public string ConfirmPassword { get; set; }
    [JsonPropertyName("age")]
    public int    Age       { get; set; }
    [JsonPropertyName("weight")]
    public double Weight    { get; set; }
    [JsonPropertyName("height")]
    public double Height    { get; set; }
} 