
namespace CYOA;

public class Player
{
    public string Name { get; set; }
    public Race Race { get; set; }
    public Occupation Occupation { get; set; }
    public int Strength { get; set; }
    public int Agility { get; set; }
    public int Health { get; set; }
    public Weapon Weapon { get; set; }
    public int Gold { get; set; } = 100;
    public Upgrade OwnedUpgrade { get; private set; }

    public bool TryBuyUpgrade(Upgrade upgrade)
    {
        int price = upgrade switch
        {
            Upgrade.MagicalArmor => 50,
            Upgrade.MagicalWeapon => 75,
            _ => throw new ArgumentOutOfRangeException(nameof(upgrade))
        };
        if (OwnedUpgrade != Upgrade.None || Gold < price)
        {
            return false;
        }
        Gold -= price;
        OwnedUpgrade = upgrade;
        return true;
    }

    public Player(
        string name,
        Race race,
        Occupation occupation,
        int strength,
        int agility,
        int health,
        Weapon weapon)
    {
        Name = name;
        Race = race;
        Occupation = occupation;
        Strength = strength;
        Agility = agility;
        Health = health;
        Weapon = weapon;
    }

    private static string Label(Messages? messages, int id, string fallback)
    {
        return messages == null
            ? fallback
            : messages.GetMessage(id);
    }

    public string DisplayPlayerStatistics(Messages? messages = null)
    {
        return "===== PLAYER STATISTICS =====\n" +
            $"{Label(messages, 27, "Name")}: {Name}\n" +
            $"{Label(messages, 28, "Race")}: {Race}\n" +
            $"{Label(messages, 29, "Occupation")}: {Occupation}\n" +
            $"{Label(messages, 30, "Strength")}: {Strength}\n" +
            $"{Label(messages, 31, "Agility")}: {Agility}\n" +
            $"{Label(messages, 32, "Health points")}: {Health}\n" +
            $"{Label(messages, 33, "Weapon")}: {Weapon.type}\n" +
            $"{Label(messages, 34, "Maximum Weapon Damage")}: {Weapon.maxDamage}\n" +
            $"{Label(messages, 85, "Gold")}: {Gold}\n" +
            $"{Label(messages, 86, "Upgrade")}: {Label(messages, 87 + (int)OwnedUpgrade, OwnedUpgrade switch { Upgrade.MagicalArmor => "Magical armor", Upgrade.MagicalWeapon => "Magical weapon", _ => "None" })}\n" +
            $"{Weapon.asciiArt}\n" +
            "================================";
    }
}
