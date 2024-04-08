namespace TestApiJWT.Models
{
    public class AuthModel
    {

        public string Message { get; set; } 
        public bool IsAuthenticated { get; set; }
        public string Username { get; set; }
        public string Enail { get; set; }   
        public List<string> Roles { get; set;}
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
