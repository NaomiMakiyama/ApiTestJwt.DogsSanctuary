namespace DogsSanctuary.Db;

public static class RepoDogs
{
    public static List<Dog> Dogs = GetDogs();
    public static List<Dog> GetDogs()
    {
        var dogs = new List<Dog>();

        var dog1 = new Dog
        {
            DogName = "Mike",
            Breed = "SRD",
            BestFriend = "Alan"
        };
        dogs.Add(dog1);
        
        var dog2 = new Dog
        {
            DogName = "Mustache",
            Breed = "Border Collie",
            BestFriend = "Leon"
        };
        dogs.Add(dog2);
        
        var dog3 = new Dog
        {
            DogName = "Sam",
            Breed = "Puddle",
            BestFriend = "Ada"
        };
        dogs.Add(dog3);
        
        return dogs;
    }
}