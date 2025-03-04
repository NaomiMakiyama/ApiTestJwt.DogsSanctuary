namespace DogsSanctuary.Db;

public static class RepoDogs
{
    public static readonly List<Dog> Dogs = GetDogs();

    private static List<Dog> GetDogs()
    {
        var dogs = new List<Dog>();

        var dog1 = new Dog
        {
            DogName = "Mike",
            Breed = "SRD",
            BestFriend = "Alan",
            Characteristics = [IsTrained(),IsFriendly(),CanSwin()]
        };
        dogs.Add(dog1);
        
        var dog2 = new Dog
        {
            DogName = "Mustache",
            Breed = "Golden Retriever",
            BestFriend = "Leon",
            Characteristics = [IsFriendly(),CanSwin()]
        };
        dogs.Add(dog2);
        
        var dog3 = new Dog
        {
            DogName = "Pretty",
            Breed = "Puddle",
            BestFriend = "Ada",
            Characteristics = [IsFriendly()]
        };
        dogs.Add(dog3);
        
        var dog4 = new Dog
        {
            DogName = "Sam",
            Breed = "Samoyeda",
            BestFriend = "Samantha",
            Characteristics = [IsTrained(), CanSwin()]
        };
        dogs.Add(dog4);
        
        return dogs;
    }

    private static Characteristic IsTrained() => new(IS_TRAINED);
    private static Characteristic IsFriendly() => new(IS_FRIENDLY);
    private static Characteristic CanSwin() => new(CAN_SWIN);

    public const string IS_TRAINED = "is-trained";
    public const string IS_FRIENDLY = "is-friendly";
    public const string CAN_SWIN = "can-swin";
}