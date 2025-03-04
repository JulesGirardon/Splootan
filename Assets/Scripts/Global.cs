using UnityEngine;

public static class Global
{
    public static int musicVolume = 50;
    public static int sfxVolume = 50;

    public static Nuzzle[] nuzzles = new Nuzzle[]
    {
        new Nuzzle { title = "Nuzzle A", description="Ceci est la buse 1" },
        new Nuzzle { title = "Nuzzle B", description="Ceci est la buse 2" },
        new Nuzzle { title = "Nuzzle C", description="Ceci est la buse 3" }
    };
}
