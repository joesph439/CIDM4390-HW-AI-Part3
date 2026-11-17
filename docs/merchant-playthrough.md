# Merchant playthrough

Run `dotnet run` and repeat these checks with language options 1 (English),
2 (French), and 3 (Spanish). Character creation also accepts English answers.

1. Create a character, choose any race and occupation, roll the three stats,
   and display statistics. Confirm 100 gold and no upgrade.
2. Proceed on the adventure and travel south (`s`). Confirm the encounter,
   item descriptions, prices (50 and 75), and balance use the selected language.
3. Enter an invalid choice, then `3` to leave. Confirm the menu repeats on
   invalid input and leaving returns to the direction menu.
4. Travel south again and buy armor (`1`). Confirm the protection description,
   remaining 50 gold, and return to the direction menu.
5. Travel south again. Confirm the merchant explains that another purchase
   is blocked, then returns to the direction menu.
6. Start a fresh run. Confirm 100 gold and no upgrade, then travel south and
   buy the weapon (`2`). Confirm the enchantment description and 25 gold left.
7. Travel north (`n`) and attack (`a`). Confirm combat proceeds normally
   with the current weapon's artwork. The automated tests verify exact
   damage effects without depending on random rolls.

Insufficient funds and exact-price purchases are covered by MSTest because
normal play starts with enough gold for either item and permits only one purchase.

Validation performed: scripted console playthroughs in all three languages
cover character statistics, invalid input, declining, revisiting, buying
each upgrade, and blocked repeat purchases. Human review of translation
naturalness remains a manual check.
