namespace api.Domain.ValueObjects;

public class Address : IEquatable<Address>
{
    public string Street { get; set;} = string.Empty;
    public string City { get; set;} = string.Empty;
    public string State { get; set;} = string.Empty;
    public string PostalCode { get; set;} = string.Empty;
    public string Country { get; set;} = string.Empty;

    public static Address Create(string street, string city, string state, string postalCode, string country) {
        return new Address()
        {
            Street = street,
            City = city,
            State = state,
            PostalCode = postalCode,
            Country = country
        };
    }

    public bool Equals(Address other)
    {
        if (other == null) return false;
        return Street == other.Street &&
               City == other.City &&
               State == other.State &&
               PostalCode == other.PostalCode &&
               Country == other.Country;
    }

    public override bool Equals(object obj) => Equals(obj as Address);
    public override int GetHashCode() => HashCode.Combine(Street, City, State, PostalCode, Country);
}
