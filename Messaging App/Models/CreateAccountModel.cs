namespace MessagingApp.Models
{
    public class CreateAccountModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Phone_Number { get; set; }
        public string Email { get; set; }
        public string DateOfBirth { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}