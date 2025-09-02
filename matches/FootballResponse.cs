public class FootballResponse
{
    public List<FootballFixture> Response { get; set; }
}

public class FootballFixture
{
    public FootballTeams Teams { get; set; }
    public FootballGoals Goals { get; set; }
    public FootballLeague League { get; set; }  // Ajout de la ligue
    public string Country { get; set; }         // Ajout du pays
}

public class FootballLeague
{
    public string Name { get; set; }
    public string Country { get; set; } // Cette propriété est utilisée ici
}
public class FootballTeams
{
    public FootballTeam Home { get; set; }
    public FootballTeam Away { get; set; }
}

public class FootballTeam
{
    public string Name { get; set; }
    public string Logo { get; set; } // Assure-toi que tu récupères les logos des équipes
}

public class FootballGoals
{
    public int? Home { get; set; }
    public int? Away { get; set; }
}
