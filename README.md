# Red Blue adventures: Darker times - source code
This is the source code for a sequel to RedBlue adventures: Crash n' brawl. I doubt I will finish it any time soon, but I have done a lot of work to change many things about the source code.
I plan to do some doccumentation for the source code, since there are many messy things about it.
This is more of my gift to everyone else who wishes to make a turn based RPG that can be scaled up. I use scriptable objects to create bases for players/enemies, moves, status effects and even elements.
Keep in mind that this game uses the press turn system from Shin Megami Tensei, which breaks some normal RPG conventions, so it may take some work to change it into something more conventional.
Although I will update this from time to time, I do encourage people to make their own versions of this engine (perhaps even better than it).

## S_battlemanager
This is the heart of the battle system, it manages the flow of the battle from selecting moves, targets, having the user excecture their moves and the press turn system. It's basically a monolith.
