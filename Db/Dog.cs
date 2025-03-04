namespace DogsSanctuary.Db;

public class Dog
{
    public required string DogName { get; init; }
    public string? Breed { get; init; }
    public required string BestFriend { get; init; }
    public Characteristic[] Characteristics { get; init; } = null!;
}