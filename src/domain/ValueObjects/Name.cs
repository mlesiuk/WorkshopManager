using workshopManager.Domain.Common;

namespace workshopManager.Domain.ValueObjects;

public sealed class Name : ValueObject
{
    public string FirstName { get; } = string.Empty;
    public string LastName { get; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";

    public Name(string firstName, string lastName)
    {
        if (string.IsNullOrEmpty(firstName))
            throw new ArgumentException($"'{nameof(firstName)}' can not be null or empty.", nameof(firstName));

        if (string.IsNullOrEmpty(lastName))
            throw new ArgumentException($"'{nameof(lastName)}' can not be null or empty.", nameof(lastName));

        FirstName = firstName;
        LastName = lastName;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }
}
