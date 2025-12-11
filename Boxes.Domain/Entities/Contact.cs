namespace Boxes.Domain.Entities
{
    public class Contact
    {
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string? Phone { get; private set; }

        private Contact() { }

        public Contact(string name, string email, string? phone = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required", nameof(name));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required", nameof(email));

            Name = name;
            Email = email;
            Phone = phone;
        }
    }
}
