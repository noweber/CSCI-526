# Aquarius Emblem - A Chess-Like Tactical Strategy Game

## Game Overview
Welcome to Aquarius Emblem, a captivating turn-based strategy game that unfolds in the cosmic expanse. This game blends the timeless allure of chess with thrilling unit synergies, offering a challenging and immersive experience. Command your fleet of space ships, explore distant planets, and master the art of strategic warfare among the stars.

## Logline
Embark on an interstellar adventure, where chess-like tactics and unit synergies converge in the vast cosmic arena of Aquarius Emblem.

## Short Description
Aquarius Emblem is a unique turn-based strategy game that combines the elegance of chess with a refreshing cosmic twist. Engage in tactical battles with diverse units, each possessing its own movement patterns and abilities. Unleash the potential of unit synergies to outwit your opponents and conquer the cosmic realm. Strategize your moves and lead your space fleet to triumph!

## Target Audience
Aquarius Emblem is designed for both newcomers to the strategy genre and seasoned players. Fans of chess variants seeking a fresh challenge, as well as those who enjoy strategic titles like Advance Wars or Fire Emblem, will find themselves at home in this cosmic journey.

## Key Features
- Chess-like unit movement.
- Diverse units with unique behaviors and abilities.
- Enemy AI for challenging gameplay.
- Engaging and immersive strategic experience.

## Genre
Strategy

## Platforms
- PC
- WebGL

## URL
Play the game at: [Aquarius Emblem on itch.io](https://noweber.itch.io/aquarius-emblem)

## Team Members
- Nicholas Weber (Team Captain, Analytics)
- Ann Yan (AI, Guidance)
- Derek Nguyen (Physics, AI, Guidance)
- Jeffrey Lin (Analytics, Guidance)
- Joshua Maranan (Note Taker, User Interface, Game Feel)
- Zhengren Lin (User Interface, Game Feel)
- Mridul Shah (Analytics (PM), Game Feel)

## Development
This game was developed as part of the CSCI 526 course at the University of Southern California (USC).

## Units
- Fighter: Main attacking unit. The Fighter may move again after capturing an enemy unit. On its own, Fighters may only move in a radius of 1 tile.
- Satellite: Vision unit. If a team moves a unit to a tile adjacent to the Satellite, the Satellite is "captured" by that team and grants vision within 5 tiles. Immobile.
- Support: Supporting unit. When adjacent to an ally Fighter or Scout, the Support buffs the allied unit's movement. May move in a diamond pattern.
- Scout: Mobile vision unit. Grants limited vision in the direction it is facing (based on the last direction it moved). May only move 1 tile in the four cardinal directions.
- Planet: Stationary, but critical unit. The capture of a Planet by the opposing team grants victory to said team.

## Game Mechanics
- Unit Selection: Point-and-click to select and move units on the board.
- Unit Abilities: Each unit type has unique abilities that can be used strategically during gameplay.
- Grid-Based Movement: Units move within a grid-based board similar to Chess.

## Levels
1. Tutorial(s)
   - Tutorial (Fighter): Introduces the main movement of the game using the Fighter unit, along with the main objective.
   - Tutorial (Fighter Ability): Introduces the Fighter's unique ability to chain captures.
   - Tutorial (Support): Introduces the Support unit's unique movement pattern.
   - Tutorial (Support Ability): Introduces the synergy the Support has with the Fighter and Scout units, encouraging players to make use of synergies.
   - Tutorial (Scout Ability): Introduces Fog of War and the Scout unit's unique ability to grant mobile vision.
   - Tutorial (Satellite Ability): Introduces the Satellite as a resource that grants fixed vision in the Fog of War.
2. Level One: Small-sized level allowing players to freely play with unit movement and synergies.
3. Level Two: Medium-sized level that encourages better use of unit movements and provides more opportunities for strategic thinking.
4. Level Three: Large-sized level that requires players to make thorough use of Scouts to find and capture the enemy Planet while protecting their own.
5. Level Four: Notable choke point level, requiring more careful movements or taking significant risks to achieve victory. Subsequent levels will have a large number of units to manage.
6. Level Five: Notable risky point at the center, demanding cautious movements and Scout usage, or taking daring risks to emerge victorious.
7. Level Six: Very large level testing players' thorough knowledge of the game, especially the use of synergy and strategy to secure triumph.

## How to Play
1. Clone the repository to your local machine.
2. Open the project in your preferred Unity editor version.
3. Navigate to the main scene and hit play to start the game.
4. Use the mouse to select and move units on the board.
5. Utilize unit abilities strategically to gain an advantage.
6. Capture the enemy Planet in each level to achieve victory.

## Contributions
Contributions to the Aquarius Emblem project are welcome. If you find any issues or have ideas to enhance the gameplay, feel free to open a pull request.

## Credits
- Game Design and Development: Aquarius Emblem Team

Aquarius Emblem is a collaborative effort brought to you by the talented members of the team. Let the cosmic battle begin! Will you harness the power of unit synergies to conquer the stars in Aquarius Emblem? Challenge yourself and embark on an interstellar adventure that will test your strategic wit and tactical prowess in this captivating chess-like strategy game.
