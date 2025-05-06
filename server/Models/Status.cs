namespace Arena;

public class Status
{
    public int duration {get; set;}
    public int name {get; set;}
    public int type {get; set;}
    public double damage {get; set;}

    public Status(int duration, int name, int type, double damage)
    {
        this.duration = duration;
        this.name = name;
        this.type = type;
        this.damage = damage;
    }
}