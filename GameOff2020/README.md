# MoonlightTraveller
[Gameplay](#Gameplay), [Game Design](#Game-Design), [Technical Design](#Technical-Design)

***

## Gameplay

* Save the core...
* Keys:
  * Movement: W, A, S, D 
  * Space: Jump
  * Q: Roll
  * Punch: 1
  * Slash: 2
  * Shoot: 3
  * Target: Tab
  * Menu: Esc

***

# Game Design 
## Story
Monsters have taken over the world. And you are the last one who wants to go to the moon.
Kill the monsters with moonlight sonata(if its possible spawn them according music).
## Mechanic
Walk/Run, jump basic locomotion.
Target based attack system, directional attack, AoE damage, Quick Near AoE
Monsters come to us and Attacking (maybe unique maybe basic) 
Save the core light or piano or rocket
Carry resource to core and restore building
Learning skills with Music
Reborn with automaticly
When music completed level passed and congratulations
## Score
Killed Enemy Count
Death

***

# Technical Design
## Outline
- Level
	- WorldEnvironment
	- Player(Character+Camera+Abilities)
		- Collision
		- Model+Anims
		- AnimTree
		- CameraController
		- Ability
	- EnemySpawner
		- [Enemy]
	- MusicPlayer(sync with EnemySpawner) 
	- Environment
		- EnvironmentModels
	- GameManager
	- HUD


---
- Enemy(Character+AI+~Abilities)
	- Model+Anims
	- AnimTree
	- Collision~
	- AI