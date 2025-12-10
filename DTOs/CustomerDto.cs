namespace CustomerOrderApi.DTOs;

public class CustData
{
    public int id { get; set; }
    public string email { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string last_name { get; set; } = "";
    public string Password { get; set; } = "";
    public string CreditCardNumber { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public string SSN { get; set; } = "";
}

public class CreateCustomerRequest
{
    public string Email { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Pwd { get; set; } = "";
}
