namespace SN.Domain
{
    internal class CustomerSocialNetwork : IEntity<int>
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
