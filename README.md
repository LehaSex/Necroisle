#  Basic Information

**Necroisle** is an exciting open-world game where every adventure is unique thanks to a deterministic procedurally generated map. In this game you will be able to immerse yourself in the fantasy world, exploring it from a third person view from above.

One of the key aspects of Necroisle is its open world, which offers endless possibilities for research. You can explore a variety of locations, meet a variety of characters and complete various quests, revealing the secrets of the world around you.

The game is presented in a cartoon style, which gives it brightness and a unique character. This style creates a unique atmosphere and makes the game world attractive and fascinating.

In addition, Necroisle is a cross-platform game, which means that you can play it on different devices, be it a PC, console or mobile device, saving your progress and enjoying the gameplay anywhere, anytime.

However, the most exciting thing about Necroisle is the support for custom modifications in the LUA language. This allows players to create and add their own elements to the game, making unique changes and expanding the possibilities of the Necroisle world according to their imagination. Thus, each player can contribute to the development of the game, making it even more diverse and exciting for all participants.

## LUA Mod System

The Lua modification system in Unity has several advantages over standard modification methods:

## Features

- 🌈 Flexibility and ease of use
- 🚀 Dynamic loading and updating
- 🧩 Less dependency on compilation
- 📱 Improved scalability and support

In the Lua mod system, which mimics the behavior of MonoBehaviour in Unity, users can define their scripts in the Lua language, which must contain three main functions: `start()`, `update()`, and `ondestroy()`. 
Here is a brief description of each of these functions:

## Functions

- **start()**: This function is called once at the beginning of the lifecycle of the object to which the Lua script is bound. It is used to initialize an object, perform any initial actions or settings.

- **update()**: This function is called every frame and allows you to execute logic that needs to be updated or checked on each frame. This function usually implements the basic game logic, updating the position of objects, checking collisions, etc.

- **ondestroy()**: This function is called before the object is destroyed or removed from the scene. It is used to free up resources, cancel event subscriptions, or perform any final actions before destroying an object.

The mandatory presence of these three functions in Lua scripts ensures structuring and consistency in the development of user modifications and facilitates the integration of these scripts into the gaming environment.

Example of a custom Lua script:

## Example LUA Script

```lua
local text = "Hello World"

function start()
	-- This equals Debug.Log("lua start...")
	print("lua start...")
	print("injected object", GameSingleton)
	-- Find class GameSingleton 
	GameSingletonComponent= GameSingleton:GetComponent(typeof(CS.Necroisle.GameSingleton))
	print("GameSingletonComponent", GameSingletonComponent)
end

function update()
	-- Call Function From GameSingleton class
	GameSingletonComponent:CallDebugLog(text)
	-- Change position of GameSingleton object
	local pos = GameSingleton.transform
	pos.position = pos.position + CS.UnityEngine.Vector3(0, 1, 0)
end

function ondestroy()
    print("lua destroy")
end
```


