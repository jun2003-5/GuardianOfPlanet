# 🚀 Guardian of Planet

> Explore the cosmos and defend the planets of the Greek gods! Upgrade your spaceship, hunt monsters, and grow stronger on an epic journey through the stars.

A solo-developed idle/action RPG built with Unity and C#. This was my second commercial game, built after learning the fundamentals from my first title, *40075: The Lost Captain*.

---

## 📖 About the Game

Players guide a customizable spaceship through waves of enemies approaching from all directions (360°), automatically engaging targets while progressing through stages, dungeons, and gear upgrades.

### Key Features

- **Auto-Hunt System** — Enemies are automatically engaged as they approach, letting players focus on strategy and progression.
- **Gacha System** — Collect items ranging from Normal to Ancient rarity.
- **Equipment System** — Upgrade and synthesize gear across multiple rarity tiers.
- **Mining** — Gather ore with the help of "Earth miners" to power spaceship upgrades (a mechanic carried over from my first game to give returning players a familiar feel).
- **Dungeons** — 4-stage dungeon runs culminating in boss fights, with substantial rewards.
- **Spaceship Customization** — Multiple ship types, each upgradeable and chosen to fit the player's playstyle.
- **Online Leaderboard** — Global ranking system for player competition.
- **Cloud Save** — Google-account-based cloud save, replacing the local-storage approach from my previous project.

---

## 🛠 Tech Stack

- **Engine**: Unity
- **Language**: C# (Visual Studio)
- **Save System**: Google Cloud Save (account-linked)
- **Art**: Sprites and ship/enemy assets sourced from the Unity Asset Store (unlike my first game, art was not hand-drawn for this project)

---

## 👤 Background & Role

- **Developer**: Solo developer — game design, programming, systems balancing, and integration all handled independently
- **Development period**: ~10 months, developed during summer break and alongside university coursework (my first game, *40075: The Lost Captain*, took 6 months full-time; this one took longer due to a much lighter daily time budget while in school)
- **Motivation**: After my first game generated real revenue, I wanted to build the "hunt-and-grow" style idle RPG I'd wanted to make since childhood — a game where the player sits at the center of the screen while enemies approach from every direction and get auto-engaged.
- **Foundational training**: I first learned to code at Homestead High School in Silicon Valley, where I picked up object-oriented programming and algorithm design — skills that made this project noticeably easier to structure than my first.

---

## 🔧 Key Technical Challenges

**1. Weapon & Projectile Systems**
The player's spaceship could equip a variety of weapons — twin-shot guns, a magnetic field (damage-per-second to anything inside it), lasers, bombs, homing missiles, and bullets that ricochet to a new enemy on hit. Implementing the physics and collision logic for each weapon type from scratch was one of the most demanding parts of development.

**2. Per-Enemy Collision Design**
Every enemy type had a unique shape, which meant manually configuring a custom 2D Rigidbody/collider for each one to get accurate hit detection. This was extremely time-consuming and one of the more tedious parts of the project.

**3. Reward & Progression Balancing**
Gold and item rewards needed to scale differently across stages and difficulty tiers, which required careful tuning to keep progression feeling fair and rewarding.

**4. Game Balance**
This was, by far, the hardest problem to solve — and gave me a real appreciation for why large studios have dedicated balance teams. Since I had only ever played RPGs and never designed one, tuning difficulty and pacing to feel satisfying was genuinely difficult. I ran beta tests with friends and iterated on pacing and difficulty based on their feedback until the game felt right.

**5. Cloud Save Integration**
Having struggled with local save/data-corruption bugs on my first game, I switched to Google Cloud Save for this project. It was easier to implement and dramatically reduced save-related errors compared to a local-only system.

---

## 🚀 Release History

| Item | Details |
|---|---|
| Platforms | Google Play Store, Apple App Store |
| Post-launch | Iterated based on user reviews and feedback after release |
| Current status | Taken down from stores due to a policy change while the developer was serving mandatory military service; currently not listed on any store |

> Note: The game runs correctly when launched from the Unity Editor.

---

## 📩 Contact

jungames0519@gmail.com

---

## ⚠️ Notice

This repository is shared for portfolio purposes to document my game development experience. No signing keys (keystores), API keys, or other sensitive credentials are included.
