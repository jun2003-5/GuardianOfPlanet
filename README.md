# 🚀 Guardian of Planet

> Explore the cosmos and defend the planets of the Greek gods! Upgrade your spaceship, hunt monsters, and grow stronger on an epic journey through the stars.

A solo-developed idle/action RPG built with Unity and C#. This was my second commercial game, built after learning the fundamentals from my first title, *40075: The Lost Captain*.

---

## 📖 About the Game

Players guide a customizable spaceship positioned at the center of the screen while waves of enemies approach from all 360 degrees. Enemies are engaged automatically as they close in, and players progress by clearing stages, running dungeons, and continuously upgrading their gear and ship.

### Key Features

- **Auto-Hunt Combat** — Enemies approaching from any direction are automatically targeted and engaged, so players can focus on build strategy and progression rather than manual aiming.
- **Gacha System** — A tiered item-pull system spanning Normal, Rare, Epic, Legendary, and Ancient rarities, giving players long-term collection goals.
- **Equipment System** — Gear can be upgraded (enhancement) and synthesized (combining lower-tier items into higher-tier ones) across every rarity tier.
- **Mining** — "Earth miners" assist the player in gathering ore used to power spaceship upgrades — a system carried over conceptually from the mining/gathering loop in my first game, so returning players would find something familiar.
- **Dungeons** — Structured 4-stage dungeon runs that culminate in a boss encounter, offering higher and more concentrated rewards than standard stages.
- **Spaceship Customization** — Multiple distinct ship types, each with its own upgrade path, letting players choose a ship that matches their preferred playstyle.
- **Online Leaderboard** — A global ranking system where players compete for the top spot based on in-game progression.
- **Cloud Save** — Google-account-linked cloud save, replacing the purely local-storage approach used in my previous project to reduce data-loss risk.

---

## 🛠 Tech Stack

- **Engine**: Unity
- **Language**: C# (Visual Studio)
- **Save System**: Google Cloud Save, authenticated via Google account login (server-side persistence instead of local device storage)
- **Art**: Enemy and spaceship sprites/assets purchased from the Unity Asset Store — a deliberate change from my first game, where all art was hand-made, so I could spend more development time on systems and gameplay logic

---

## 👤 Background & Role

- **Developer**: Solo developer — responsible for game design, all gameplay/systems programming, combat and progression balancing, backend (cloud save/leaderboard) integration, and store submission
- **Development period**: ~10 months. Started during summer break at age 21 and continued part-time alongside university coursework. By comparison, my first game (*40075: The Lost Captain*) took 6 months of full-time, dedicated development — this project took longer in calendar time specifically because daily available hours were much lower while balancing classes.
- **Motivation**: After my first game generated real, measurable revenue, I became more serious about game development as more than a hobby. I had also wanted to build a "hunt-and-grow" style idle RPG since childhood — specifically one where the player sits at the center of the screen and enemies close in from every direction to be auto-engaged, rather than a side-scrolling or top-down aim-and-shoot format.
- **Prior experience carried over**: Having already shipped one full game, I found that establishing the overall project structure and core game loop for this title was noticeably faster than starting from zero. The systems that were genuinely new and difficult were the weapon physics, per-enemy collision setup, and balance tuning described below.
- **Foundational training**: I first learned to program at Homestead High School in Silicon Valley, where I was introduced to object-oriented programming and algorithm design. Applying that foundation directly made structuring this project's codebase considerably easier than my first game.

---

## 🔧 Key Technical Challenges

**1. Weapon & Projectile Systems**
The player's spaceship could be equipped with a wide range of weapon types, each requiring its own physics and behavior logic implemented from scratch:
- Twin-shot guns (two simultaneous projectiles)
- A magnetic field weapon that deals damage-per-second to any enemy inside its radius
- Lasers (continuous hit-scan style damage)
- Bombs (area-of-effect on impact)
- Homing missiles (target-tracking projectiles)
- Ricochet bullets that automatically redirect to a new enemy after a hit
- A drone that orbits the player, independently tracking and eliminating nearby enemies

Balancing damage output, projectile speed, and hit registration across this many distinct weapon behaviors — while keeping performance stable with many enemies and projectiles on screen simultaneously — was one of the most demanding parts of the project.

**2. Per-Enemy Collision Design**
Every enemy type had a visually unique shape, so accurate hit detection required manually configuring a custom 2D Rigidbody and Collider fitted to each individual enemy's silhouette. With dozens of enemy types across the game, this was extremely time-consuming and one of the more tedious, repetitive parts of development — but essential for making combat feel fair and precise.

**3. Reward & Progression Balancing**
Gold and item drop rates needed to scale differently depending on stage number, dungeon difficulty, and enemy tier. Getting this scaling right — so early game felt rewarding without making late-game trivial — required a substantial amount of manual tuning and iteration.

**4. Overall Game Balance**
This was, by a wide margin, the hardest problem on the project — and gave me real respect for why large studios employ dedicated balance teams. I had extensive experience *playing* RPGs, but none designing the underlying pacing and difficulty curve myself, which made getting the feel right genuinely difficult. To address this, I ran structured beta tests with friends, collected feedback on pacing, difficulty spikes, and reward satisfaction, and iterated on the balance repeatedly until the game felt fair and engaging to outside players — not just to me.

**5. Cloud Save Integration**
My first game used purely local device storage, which led to repeated data-corruption bugs that took about a month to fully resolve. For this project, I integrated Google Cloud Save, authenticated through Google account login. This was both easier to implement correctly and dramatically more reliable — save-related bug reports dropped significantly compared to the local-storage system in my previous game.

---

## 📸 Screenshots

![Main Screen](screenshots/1.jpg)
![Main Stage](screenshots/2.jpg)
![Item Gacha](screenshots/3.jpg)
![Mining System](screenshots/4.jpg)
![Equipment Upgrade](screenshots/5.jpg)
![Monster Collection](screenshots/6.jpg)
![Game Icon](screenshots/7.png)

---

## 🚀 Release History

| Item | Details |
|---|---|
| Platforms | Google Play Store, Apple App Store |
| Revenue | Google Play Store: ₩96,000 · Apple App Store: $159 (≈ ₩215,600) |
| Post-launch | Continued iteration based on user reviews and beta/live player feedback |
| Current status | Removed from both stores due to a policy change that took effect while the developer was serving mandatory military service; currently not listed on any storefront |

> Note: The game runs and launches correctly from the Unity Editor.
> Note: Apple revenue converted at an approximate exchange rate of $1 ≈ ₩1,356 (September 2026).

---

## 📩 Contact

kimjy5191112@gmail.com

---

## ⚠️ Notice

This repository is shared for portfolio purposes to document my game development experience. No signing keys (keystores), API keys, or other sensitive credentials are included.
