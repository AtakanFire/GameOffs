# MoonlightTraveller
[Gameplay](#Gameplay), [Game Design](#Game-Design), [Technical Design](#Technical-Design)

***

## Gameplay

* Live until Space Shuttle launch with "Moonlight Sonata", when music slow, take notes(parchments​) and gain your health(with music).​
* Keys:
  * Movement: W, A, S, D 
  * Space: Jump
  * Dragon's Breath: 1
  * Sword Slash: 2
  * Rain of Fire: 3
  * Target: Tab
  * Interact: E
  * Q: Roll

***

# Game Design 
## Story
Monsters have taken over the world. And you are the last one who wants to go to the moon.
Kill the monsters with moonlight sonata ~~and spawn them according music~~.
## Mechanic
* Walk/Run, jump basic locomotion.
* Target based attack system, directional attack, AoE damage, Quick Near AoE
* Monsters come to us and basic attack
* Save the character
* Live until Space shuttle launch
* Health is the speed of music.
* When music completed level passed and congratulations
## Score
Killed Enemy Count

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
	- MusicPlayer(~~sync with EnemySpawner~~) 
	- GameManager
	- HUD
	- Navigation(?Why work like that-Godot require a nav with mesh instance and meshes as a child) 
		- NavigationMeshInstance
			- EnvironmentModels
			
> Enemy(Character+AI+~Abilities)
- Model+Anims
- AnimTree
- Collision~
- AI

> Projectile
- Particle

> Scripts aren't organized, project needs to be refactor. :) 

## Comment
Current version needs multiple edits (batch edit). Because we can't create collisions for all selected meshes, we have to create collisions one by one. Also needs UI and UX improvements. (Eg. When mesh information is in one place, mesh actions are in camera. Also Anchor, Anim Tree needs invert anim condition)
Pawn and character can be included to engine. 
Too many 3d tools(Landscape etc.) are missing. Needs more level design(Spatial Editor) hotkeys (Eg. Alt and move: duplicate and move spatial[duplicate and move actor in unreal]) 