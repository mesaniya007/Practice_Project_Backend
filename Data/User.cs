namespace user_crud_api.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? Birth_date{ get; set; }
        public string Password { get; set; }
        public bool Approved { get; set; }
        public decimal Balance { get; set; }
    }
}
