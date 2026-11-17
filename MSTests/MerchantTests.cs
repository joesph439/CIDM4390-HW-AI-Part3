using CYOA;

namespace CYOA.Tests;

[TestClass]
public class MerchantTests
{
    private const string Create = "1\nElf\nEmber\nThief\nroll\nroll\nroll\n2\n";

    private static Messages Load(string language)
    {
        var messages = new Messages { CurrentLanguage = language };
        for (DirectoryInfo? directory = new(AppContext.BaseDirectory);
             directory != null; directory = directory.Parent)
        {
            string path = Path.Combine(directory.FullName, "MessageFiles", language + ".json");
            if (File.Exists(path))
            {
                messages.ReadDictionary(path);
                return messages;
            }
        }
        throw new FileNotFoundException("Message files not found.");
    }

    private static Player NewPlayer() => new("Ember", Race.Elf, Occupation.Thief,
        20, 0, 100, new Weapon("Dagger", 12, "art"));

    [TestMethod]
    [DataRow(Upgrade.MagicalArmor, 50)]
    [DataRow(Upgrade.MagicalWeapon, 75)]
    public void Purchase_DeductsPrice_AndBlocksBothFurtherChoices(Upgrade upgrade, int price)
    {
        Player player = NewPlayer();
        Weapon original = player.Weapon;
        Assert.IsTrue(player.TryBuyUpgrade(upgrade));
        Assert.AreEqual(100 - price, player.Gold);
        Assert.AreEqual(upgrade, player.OwnedUpgrade);
        Assert.IsFalse(player.TryBuyUpgrade(Upgrade.MagicalArmor));
        Assert.IsFalse(player.TryBuyUpgrade(Upgrade.MagicalWeapon));
        Assert.AreEqual(100 - price, player.Gold);
        Assert.AreSame(original, player.Weapon);
        Assert.AreEqual(12, original.maxDamage);
        Assert.AreEqual("Dagger", original.type);
        Assert.AreEqual("art", original.asciiArt);
    }

    [TestMethod]
    [DataRow(Upgrade.MagicalArmor, 50)]
    [DataRow(Upgrade.MagicalWeapon, 75)]
    public void Purchase_InsufficientAndExactGold(Upgrade upgrade, int price)
    {
        Player player = NewPlayer();
        player.Gold = price - 1;
        Assert.IsFalse(player.TryBuyUpgrade(upgrade));
        Assert.AreEqual(price - 1, player.Gold);
        Assert.AreEqual(Upgrade.None, player.OwnedUpgrade);
        player.Gold = price;
        Assert.IsTrue(player.TryBuyUpgrade(upgrade));
        Assert.AreEqual(0, player.Gold);
    }

    [TestMethod]
    [DataRow("English", "Magical armor", "Magical weapon", "Gold")]
    [DataRow("French", "Armure magique", "Arme magique", "Or")]
    [DataRow("Spanish", "Armadura mágica", "Arma mágica", "Oro")]
    public void Encounter_LocalizedDeclineRevisitPurchaseAndBlockedRevisit(
        string language, string armor, string weapon, string gold)
    {
        Messages messages = Load(language);
        var output = new StringWriter();
        var game = new Game(messages, new StringReader(Create + "s\n3\ns\nx\n 1 \ns\ne\n"), output);
        game.StartGame();
        Assert.AreEqual(50, game.Player!.Gold);
        Assert.AreEqual(Upgrade.MagicalArmor, game.Player.OwnedUpgrade);
        string text = output.ToString();
        StringAssert.Contains(text, armor);
        StringAssert.Contains(text, weapon);
        StringAssert.Contains(text, messages.GetMessage(52));
        StringAssert.Contains(text, messages.GetMessage(84));
        StringAssert.Contains(text, messages.FormatMessage(81, ("gold", 50)));
        string stats = game.Player.DisplayPlayerStatistics(messages);
        StringAssert.Contains(stats, gold + ": 50");
        StringAssert.Contains(stats, armor);
        for (int id = 80; id <= 90; id++)
            Assert.AreNotEqual($"[{id}]", messages.GetMessage(id));
    }

    [TestMethod]
    [DataRow("English")]
    [DataRow("French")]
    [DataRow("Spanish")]
    public void Encounter_InsufficientGold_ThenCheaperPurchase(string language)
    {
        Messages messages = Load(language);
        var output = new StringWriter();
        var game = new Game(messages, new StringReader(Create + "e\ns\n2\n1\ne\n"), output);
        game.StartGame();
        game.Player!.Gold = 50;
        game.StartAdventure(() => Assert.Fail("Must not attack."));
        StringAssert.Contains(output.ToString(), messages.GetMessage(83));
        Assert.AreEqual(0, game.Player.Gold);
        Assert.AreEqual(Upgrade.MagicalArmor, game.Player.OwnedUpgrade);
    }

    [TestMethod]
    [DataRow("English")]
    [DataRow("French")]
    [DataRow("Spanish")]
    public void Encounter_WeaponConfirmationAndStatistics(string language)
    {
        Messages messages = Load(language);
        var output = new StringWriter();
        var game = new Game(messages, new StringReader(Create + "s\n2\ne\n"), output);
        game.StartGame();
        Assert.AreEqual(25, game.Player!.Gold);
        StringAssert.Contains(output.ToString(), messages.FormatMessage(82, ("gold", 25)));
        StringAssert.Contains(game.Player.DisplayPlayerStatistics(messages), messages.GetMessage(89));
    }

    [TestMethod]
    public void Encounter_DeclineAndEndOfInput_DoNotChangeState()
    {
        var game = new Game(Load("English"), new StringReader(Create + "s\n3\ns\n"), new StringWriter());
        Assert.ThrowsExactly<EndOfStreamException>(() => game.StartGame());
        Assert.AreEqual(100, game.Player!.Gold);
        Assert.AreEqual(Upgrade.None, game.Player.OwnedUpgrade);
    }

    [TestMethod]
    public void NewGame_ResetsGoldAndUpgrade()
    {
        var game = new Game(Load("English"),
            new StringReader(Create + "s\n2\ne\n" + Create + "e\n"), new StringWriter());
        game.StartGame();
        Assert.AreEqual(Upgrade.MagicalWeapon, game.Player!.OwnedUpgrade);
        game.StartGame();
        Assert.AreEqual(100, game.Player!.Gold);
        Assert.AreEqual(Upgrade.None, game.Player.OwnedUpgrade);
        StringAssert.Contains(game.Player.DisplayPlayerStatistics(), "Upgrade: None");
    }

    [TestMethod]
    [DataRow(2, 0)]
    [DataRow(3, 0)]
    [DataRow(7, 4)]
    public void Armor_ReducesDamageWithoutHealing(int rolledDamage, int expectedDamage)
    {
        Player player = NewPlayer();
        player.TryBuyUpgrade(Upgrade.MagicalArmor);
        var dragon = new Dragon("Test") { DragonStrength = 20 };
        var rolls = new Queue<int>(new[] { 1, 20, rolledDamage });
        var combat = new Combat(output: new StringWriter(), roll: _ => rolls.Dequeue());
        combat.DragonAttack(dragon, player);
        Assert.AreEqual(100 - expectedDamage, player.Health);
    }

    [TestMethod]
    [DataRow(Upgrade.None, 20, 0, 4)]
    [DataRow(Upgrade.MagicalWeapon, 20, 0, 9)]
    [DataRow(Upgrade.MagicalWeapon, 0, 0, 0)]
    [DataRow(Upgrade.MagicalWeapon, 20, 20, 0)]
    public void Weapon_AddsFiveOnlyToSuccessfulUndefendedAttacks(
        Upgrade upgrade, int strength, int defense, int expectedDamage)
    {
        Player player = NewPlayer();
        player.Strength = strength;
        if (upgrade != Upgrade.None) player.TryBuyUpgrade(upgrade);
        var dragon = new Dragon("Test") { DragonHealth = 100, DragonAgility = defense };
        var rolls = new Queue<int>(new[] { 1, 20, 4 });
        new Combat(output: new StringWriter(), roll: _ => rolls.Dequeue()).PlayerAttack(player, dragon);
        Assert.AreEqual(100 - expectedDamage, dragon.DragonHealth);
    }
}
